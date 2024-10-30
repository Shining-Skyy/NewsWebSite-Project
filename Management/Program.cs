using Application.Banners;
using Application.Categorys.CategoryTypes;
using Application.Comments;
using Application.Interfaces.Contexts;
using Application.Posts.AddNewPost;
using Application.Posts.AddNewPost.Dto;
using Application.Posts.AddNewPost.Validator;
using Application.Posts.FavoritePostService;
using Application.Posts.PostServices;
using Application.Services.Email;
using Application.Services.Google;
using Application.Services.Sms;
using Application.UriComposer;
using Application.Visitors.VisitorOnline;
using FluentValidation;
using Infrastructures.ExternalApi.ImageServer;
using Infrastructures.IdentityConfigs;
using Infrastructures.MappingProfile;
using Management.MappingProfiles;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

#region ConnectionString
string connection = builder.Configuration["ConnectionString:SqlServer"];
// Register the database context with the SQL Server connection.
builder.Services.AddDbContext<DataBaseContext>(option => option.UseSqlServer(connection));

// Add identity services for user authentication and management.
builder.Services.AddIdentityService(builder.Configuration);
#endregion

// Clear default logging providers to use NLog.
builder.Logging.ClearProviders();
builder.Host.UseNLog();

// Configure authorization policies.
builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("AdminUsers", policy =>
    {
        policy.RequireRole("Admin");
    });
});

// Configure application cookie settings.
builder.Services.ConfigureApplicationCookie(option =>
{
    option.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    option.LoginPath = "/Account/Login";
    option.AccessDeniedPath = "/Account/AccesDenied";
    option.SlidingExpiration = true;
});

// For cookie sharing between endpoints.
string pathToDirctory = "C:\\Users\\Ebrahim\\AppData\\Roaming\\Microsoft\\UserSecrets\\KEY";
// Configure data protection to persist keys to the file system.
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(pathToDirctory))
    .SetApplicationName("SharedCookieApp");

// Configure shared cookie settings.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = ".AspNet.SharedCookie";
    options.Cookie.Path = "/";
});

// Configure Google authentication.
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["AuthenticationGoogle:ClientId"];
        options.ClientSecret = builder.Configuration["AuthenticationGoogle:ClientSecret"];
    });

// Add SignalR services for real-time web functionality.
builder.Services.AddSignalR();

// Register various services with dependency injection.
builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();
builder.Services.AddScoped<IIdentityDatabaseContext, IdentityDataBaseContext>();
builder.Services.AddTransient(typeof(IMongoDbContext<>), typeof(MongoDbContext<>));
builder.Services.AddTransient<EmailService>();
builder.Services.AddTransient<SmsService>();
builder.Services.AddTransient<GoogleRecaptcha>();
builder.Services.AddTransient<IVisitorOnlineService, VisitorOnlineService>();
builder.Services.AddTransient<ICategoryTypeService, CategoryTypeService>();
builder.Services.AddTransient<IAddNewPostService, AddNewPostService>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<IImageUploadService, ImageUploadService>();
builder.Services.AddTransient<IUriComposerService, UriComposerService>();
builder.Services.AddTransient<IFavoritePostService, FavoritePostService>();
builder.Services.AddTransient<IBannersService, BannersService>();
builder.Services.AddTransient<ICommentsService, CommentsService>();

// Configure AutoMapper for object mapping.
builder.Services.AddAutoMapper(typeof(CategoryMappingProfile));
builder.Services.AddAutoMapper(typeof(ManagementVmMappingProfile));
builder.Services.AddAutoMapper(typeof(PostMappingProfile));

// Register a validator for the AddNewPostDto.
builder.Services.AddTransient<IValidator<AddNewPostDto>, AddNewPostDtoValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
