using Application.Comments;
using Application.Interfaces.Contexts;
using Application.Posts.GetPostPDP;
using Application.Posts.GetPostPLP;
using Application.UriComposer;
using Infrastructures.IdentityConfigs;
using Infrastructures.MappingProfile;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using Persistence.Contexts;
using System.Text;
using WebApi.Validator;

var builder = WebApplication.CreateBuilder(args);

#region Connection String
builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();
builder.Services.AddScoped<IIdentityDatabaseContext, IdentityDataBaseContext>();

string connection = builder.Configuration["ConnectionString:SqlServer"];
builder.Services.AddDbContext<DataBaseContext>(option => option.UseSqlServer(connection));

builder.Services.AddIdentityService(builder.Configuration);
#endregion

builder.Logging.ClearProviders();
builder.Host.UseNLog();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddScoped<TokenValidate, TokenValidate>();

builder.Services.AddAuthentication(Options =>
{
    // Set the default schemes for signing in, authenticating, and challenging
    Options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(configureOptions =>
{
    // Configure JWT Bearer token options
    configureOptions.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = builder.Configuration["JWtConfig:issuer"],
        ValidAudience = builder.Configuration["JWtConfig:audience"],
        // Set the signing key used to validate the token's signature
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWtConfig:Key"])),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
    };
    configureOptions.SaveToken = true;

    // Configure events for JWT Bearer authentication
    configureOptions.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            //log 
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var tokenValidatorService = context.HttpContext.RequestServices.GetRequiredService<TokenValidate>();
            return tokenValidatorService.Execute(context);
        },
        OnChallenge = context =>
        {
            return Task.CompletedTask;
        },
        OnMessageReceived = context =>
        {
            return Task.CompletedTask;
        },
        OnForbidden = context =>
        {
            return Task.CompletedTask;
        }
    };
});

// Configure Swagger for API documentation
builder.Services.AddSwaggerGen(c =>
{
    // Include XML comments for better documentation
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "WebApi.xml"), true);

    // Define a security scheme for JWT authentication
    var security = new OpenApiSecurityScheme
    {
        Name = "JWT Auth",
        Description = "Enter your token **",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    // Add the security definition to Swagger
    c.AddSecurityDefinition(security.Reference.Id, security);

    // Add security requirements for the API
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { security , new string[]{ } }
    });
});

builder.Services.AddTransient<IGetPostPLPService, GetPostPLPService>();
builder.Services.AddTransient<IGetPostPDPService, GetPostPDPService>();
builder.Services.AddTransient<IUriComposerService, UriComposerService>();
builder.Services.AddTransient<ICommentsService, CommentsService>();
builder.Services.AddTransient<ICommentsService, CommentsService>();

builder.Services.AddAutoMapper(typeof(CommentMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
