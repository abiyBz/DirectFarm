using DirectFarm.Infrastracture.Dependency;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var assemblies = new List<Assembly>()
            {
                // Add your assembly references here (e.g., Assembly.GetExecutingAssembly())
                Assembly.Load("DirectFarm.Core"),
                // ... (list other assemblies)
            };

builder.Services.AddInfrastractureConfiguration(builder.Configuration);
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
    });

});
// Order of these extentions is important since bus configuration depends on fulfillment configuration
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies.ToArray()));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
    });

});

//builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.Audience = builder.Configuration["Authentication:Audience"];
        o.MetadataAddress = builder.Configuration["Authentication:MetadataAddress"];
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero,
        };

        o.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                // Access the resource_access part of the token
                var resourceAccess = context.Principal.Claims.FirstOrDefault(c => c.Type == "resource_access")?.Value;
                if (!string.IsNullOrEmpty(resourceAccess))
                {
                    var resourceAccessJson = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, List<string>>>>(resourceAccess);
                    if (resourceAccessJson != null && resourceAccessJson.TryGetValue("directClient", out var clientRoles))
                    {
                        foreach (var role in clientRoles["roles"])
                        {
                            context.Principal.Identities.First().AddClaim(new Claim(ClaimTypes.Role, role));
                        }
                    }
                }
                return Task.CompletedTask;
            }
        };
    });
// Add this to configure authorization policies if needed
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("roles", "admin"));
    options.AddPolicy("ClientOnly", policy => policy.RequireClaim("roles", "client"));
    options.AddPolicy("ManagerOnly", policy => policy.RequireClaim("roles","manager"));
    // Add more policies as needed
});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//                .AddJwtBearer(o =>
//                {
//                    o.RequireHttpsMetadata = false;
//                    o.Audience = builder.Configuration["Authentication:Audience"];
//                    o.MetadataAddress = builder.Configuration["Authentication:MetadataAddress"];
//                    o.TokenValidationParameters = new TokenValidationParameters
//                    {
//                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//                        ValidAudience = builder.Configuration["Jwt:Audience"],
//                        ClockSkew = TimeSpan.Zero,
//                    };
//                });
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen(o =>
{

    o.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));
    o.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(builder.Configuration["Keycloak:AuthorizationUrl"]!),
                Scopes = new Dictionary<string, string> {

                                { "openid", "openid" },
                                { "profile", "profile" }
                            }
            }
        }
    });
    var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Keycloak",
                                Type = ReferenceType.SecurityScheme
                            },

                            In = ParameterLocation.Header,
                            Name = "Bearer",
                            Scheme = "Bearer"
                        },
                        []
                    }

                };

    o.AddSecurityRequirement(securityRequirement);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
