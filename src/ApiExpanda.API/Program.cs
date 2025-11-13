using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ApiExpanda.Application.Interfaces;
using ApiExpanda.Infrastructure.Repositories;
using ApiExpanda.Application.Mappings;
using Mapster;
using MapsterMapper;
using ApiExpanda.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.CodeAnalysis.Options;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ApiExpanda.Domain.Entities;
using ApiExpanda.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("ConexionSql");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseSqlServer(connectionString)
  .UseSeeding((context, _) =>
  {
      var appContext = (ApplicationDbContext)context;

      DataSeeder.SeedData(appContext);

  })
);

builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 1024 * 1024; // 1 MB
    options.UseCaseSensitivePaths = true;
});

// ============= LEGACY REPOSITORIES (Comentado - usar versiones modulares) =============
// builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
// builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// ============= MÓDULO CATALOGOS - REPOSITORIOS =============
builder.Services.AddScoped<ApiExpanda.Application.Modules.Catalogos.Interfaces.ICategoryRepository, ApiExpanda.Infrastructure.Modules.Catalogos.Repositories.CategoryRepository>();
builder.Services.AddScoped<ApiExpanda.Application.Modules.Catalogos.Interfaces.IProductRepository, ApiExpanda.Infrastructure.Modules.Catalogos.Repositories.ProductRepository>();
builder.Services.AddScoped<ApiExpanda.Application.Modules.Catalogos.Interfaces.ICompanyRepository, ApiExpanda.Infrastructure.Modules.Catalogos.Repositories.CompanyRepository>();

// ============= LEGACY SERVICES (Comentado - usar versiones modulares) =============
// builder.Services.AddScoped<ApiExpanda.Application.Services.Interfaces.ICategoryService, ApiExpanda.Infrastructure.Services.CategoryService>();
// builder.Services.AddScoped<ApiExpanda.Application.Services.Interfaces.IProductService, ApiExpanda.Infrastructure.Services.ProductService>();
builder.Services.AddScoped<ApiExpanda.Application.Services.Interfaces.IUserService, ApiExpanda.Infrastructure.Services.UserService>();

// ============= MÓDULO CATALOGOS - SERVICIOS =============
builder.Services.AddScoped<ApiExpanda.Application.Modules.Catalogos.Services.Interfaces.ICategoryService, ApiExpanda.Infrastructure.Modules.Catalogos.Services.CategoryService>();
builder.Services.AddScoped<ApiExpanda.Application.Modules.Catalogos.Services.Interfaces.IProductService, ApiExpanda.Infrastructure.Modules.Catalogos.Services.ProductService>();
builder.Services.AddScoped<ApiExpanda.Application.Modules.Catalogos.Services.Interfaces.ICompanyService, ApiExpanda.Infrastructure.Modules.Catalogos.Services.CompanyService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Configuración de políticas de contraseña: ajustar según las reglas de seguridad de tu proyecto.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false; // Permitir contraseñas sin minúsculas
    options.Password.RequireUppercase = false; // Permitir contraseñas sin mayúsculas
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwtOptions =>
{
    jwtOptions.RequireHttpsMetadata = false;
    jwtOptions.SaveToken = true;
    jwtOptions.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("AppSettings:SecretKey")!)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers(Options =>
{
    Options.CacheProfiles.Add(CacheProfiles.Default30, CacheProfiles.Default30Profile);
    Options.CacheProfiles.Add(CacheProfiles.Default20, CacheProfiles.Default20Profile);
});

// Configurar Mapster
var mapsterConfig = TypeAdapterConfig.GlobalSettings;

// ============= LEGACY MAPPINGS (Comentado - usar versiones modulares) =============
// ApiExpanda.Application.Mappings.CategoryMapping.RegisterMappings(mapsterConfig);
// ApiExpanda.Application.Mappings.ProductMapping.RegisterMappings(mapsterConfig);
ApiExpanda.Application.Mappings.UserMapping.RegisterMappings(mapsterConfig);

// ============= MÓDULO CATALOGOS - MAPPINGS =============
ApiExpanda.Application.Modules.Catalogos.Mappings.CategoryMapping.RegisterMappings(mapsterConfig);
ApiExpanda.Application.Modules.Catalogos.Mappings.ProductMapping.RegisterMappings(mapsterConfig);
ApiExpanda.Application.Modules.Catalogos.Mappings.CompanyMapping.RegisterMappings(mapsterConfig);

builder.Services.AddSingleton(mapsterConfig);
// Registrar MapsterMapper ServiceMapper como IMapper
builder.Services.AddScoped<MapsterMapper.IMapper, MapsterMapper.ServiceMapper>();
// Registrar Api Explorer y Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Configurar Custom SchemaId para evitar conflictos entre DTOs legacy y modulares
    options.CustomSchemaIds(type => type.FullName?.Replace("+", "."));

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Nuestra API utiliza la Autenticación JWT usando el esquema Bearer. \n\r\n\r" +
                      "Ingresa la palabra a continuación el token generado en login.\n\r\n\r" +
                      "Ejemplo: \"12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API Expanda V1",
        Description = "API Expanda para la gestión de productos y categorías. Versión 1",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Soporte API Expanda",
            Email = "soporte@example.com",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Licencia API Expanda",
            Url = new Uri("https://example.com/license")
        }
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "API Expanda V2",
        Description = "API Expanda para la gestión de productos y categorías. Versión 2",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Soporte API Expanda",
            Email = "soporte@example.com",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Licencia API Expanda",
            Url = new Uri("https://example.com/license")
        }
    });
});
var apiVersioningBuilder = builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    //options.ApiVersionReader = ApiVersionReader.Combine( new QueryStringApiVersionReader("api-version"));
});

apiVersioningBuilder.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(PolicyNames.AllowSpecificOrigin, policy =>
    {
        // AllowAnyOrigin en lugar de WithOrigins("*") para permitir todos los orígenes
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    // Registrar los endpoints de Swagger por cada versión detectada por ApiVersioning
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseCors(PolicyNames.AllowSpecificOrigin);
app.UseResponseCaching();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
