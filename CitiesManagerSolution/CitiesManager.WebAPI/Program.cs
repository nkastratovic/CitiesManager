using CitiesManager.Core.Identity;
using CitiesManager.Core.ServiceContracts;
using CitiesManager.Core.Services;
using CitiesManager.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => {
  options.Filters.Add(new ProducesAttribute("application/json"));
  options.Filters.Add(new ConsumesAttribute("application/json"));

  //Authorization policy
  var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
  options.Filters.Add(new AuthorizeFilter(policy));
})
 .AddXmlSerializerFormatters();

builder.Services.AddTransient<IJwtService, JwtService>();

//Enable versioning in Web API controllers
builder.Services.AddApiVersioning(config =>
{
  config.ApiVersionReader = new UrlSegmentApiVersionReader(); //Reads version number from request url at "apiVersion" constraint

  //config.ApiVersionReader = new QueryStringApiVersionReader(); //Reads version number from request query string called "api-version". Eg: api-version=1.0

  //config.ApiVersionReader = new HeaderApiVersionReader("api-version"); //Reads version number from request header called "api-version". Eg: api-version: 1.0

  config.DefaultApiVersion = new ApiVersion(1, 0);
  config.AssumeDefaultVersionWhenUnspecified = true;
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var provider = builder.Configuration.GetValue("DbProvider", "SqlServer");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{

  if (provider == "PostgreSql")
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnectionString")).UseSnakeCaseNamingConvention();
  else if(provider == "Sqlite")
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteDataSource"));
  else
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnectionString"));
});



//Swagger
builder.Services.AddEndpointsApiExplorer(); //Generates description for all endpoints


builder.Services.AddSwaggerGen(options => {
  options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));

  options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Cities Web API", Version = "1.0" });

  options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Cities Web API", Version = "2.0" });

  //Add Swagger Authorize button.
  options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
  {
    Name = "Authorization",
    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer"
  });
  options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[]{}
        }
    });

}); //generates OpenAPI specification


builder.Services.AddVersionedApiExplorer(options => {
  options.GroupNameFormat = "'v'VVV"; //v1
  options.SubstituteApiVersionInUrl = true;
});



//Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
  options.Password.RequiredLength = 5;
  options.Password.RequireNonAlphanumeric = false;
  options.Password.RequireUppercase = false;
  options.Password.RequireLowercase = true;
  options.Password.RequireDigit = true;
})
 .AddEntityFrameworkStores<ApplicationDbContext>()
 .AddDefaultTokenProviders()
 .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
 .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>()
 ;


//JWT
builder.Services.AddAuthentication(options => {
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
 .AddJwtBearer(options => {
   options.TokenValidationParameters = new TokenValidationParameters() {
     ValidateAudience = true,
     ValidAudience = builder.Configuration["Jwt:Audience"],
     ValidateIssuer = true,
     ValidIssuer = builder.Configuration["Jwt:Issuer"],
     ValidateLifetime = true,
     ValidateIssuerSigningKey = true,
     IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
   };
 });

builder.Services.AddAuthorization(options => {
});


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger(); //creates endpoint for swagger.json
app.UseSwaggerUI(options  =>
{
  options.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
  options.SwaggerEndpoint("/swagger/v2/swagger.json", "2.0");
}); //creates swagger UI for testing all Web API endpoints / action methods
app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
