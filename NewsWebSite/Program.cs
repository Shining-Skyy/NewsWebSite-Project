using Application.Categorys.GetMenuItem;
using Application.Comments;
using Application.HomePageService;
using Application.Interfaces.Contexts;
using Application.Posts.FavoritePostService;
using Application.Posts.GetPostPDP;
using Application.Posts.GetPostPLP;
using Application.UriComposer;
using Application.Visitors.VisitorOnline;
using Infrastructures.IdentityConfigs;
using Infrastructures.MappingProfile;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using NewsWebSite.Hubs;
using NewsWebSite.Utilities.Middlewares;
using NLog.Web;
using Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

// Add authorization services to the application.
builder.Services.AddAuthorization();

// Configure application cookie settings.
builder.Services.ConfigureApplicationCookie(option =>
{
    option.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    option.LoginPath = "/Home/Login";
    option.AccessDeniedPath = "/Account/AccesDenied";
    option.SlidingExpiration = true;
});

// For cookie sharing between endpoints.
string pathToDirctory = builder.Configuration["CookieKey:Path"];
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(pathToDirctory)) // Persist data protection keys to the 
    .SetApplicationName("SharedCookieApp");

// Configure shared cookie settings.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = ".AspNet.SharedCookie";
    options.Cookie.Path = "/";
});

// Configure Redis cache using the connection string from configuration.
builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = builder.Configuration["RedisCache:Configuration"];
});

// Add SignalR services for real-time web functionality.
builder.Services.AddSignalR();

// Register various services with dependency injection.
builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();
builder.Services.AddScoped<IIdentityDatabaseContext, IdentityDataBaseContext>();
builder.Services.AddTransient(typeof(IMongoDbContext<>), typeof(MongoDbContext<>));
builder.Services.AddTransient<IVisitorOnlineService, VisitorOnlineService>();
builder.Services.AddTransient<IGetMenuItemService, GetMenuItemService>();
builder.Services.AddTransient<IUriComposerService, UriComposerService>();
builder.Services.AddTransient<IGetPostPLPService, GetPostPLPService>();
builder.Services.AddTransient<IGetPostPDPService, GetPostPDPService>();
builder.Services.AddTransient<IFavoritePostService, FavoritePostService>();
builder.Services.AddTransient<IHomePageService, HomePageService>();
builder.Services.AddTransient<ICommentsService, CommentsService>();

// Configure AutoMapper for object mapping.
builder.Services.AddAutoMapper(typeof(CategoryMappingProfile));
builder.Services.AddAutoMapper(typeof(CommentMappingProfile));

builder.Services.AddSession(); // Add session services to the application.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Middleware to set visitor ID for tracking.
app.UseSetVisitorId();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Enable session middleware.
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map SignalR hub for online visitors.
app.MapHub<OnlineVisitorHub>("/chathub");

app.Run();
