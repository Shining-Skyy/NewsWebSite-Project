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
using Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region ConnectionString
string connection = builder.Configuration["ConnectionString:SqlServer"];
builder.Services.AddDbContext<DataBaseContext>(option => option.UseSqlServer(connection));

builder.Services.AddIdentityService(builder.Configuration);
#endregion

builder.Services.AddAuthorization();
builder.Services.ConfigureApplicationCookie(option =>
{
    option.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    option.LoginPath = "/Home/Login";
    option.AccessDeniedPath = "/Account/AccesDenied";
    option.SlidingExpiration = true;
});

string pathToDirctory = builder.Configuration["CookieKey:Path"];
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(pathToDirctory))
    .SetApplicationName("SharedCookieApp");

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = ".AspNet.SharedCookie";
    options.Cookie.Path = "/";
});

builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = builder.Configuration["RedisCache:Configuration"];
});

builder.Services.AddSignalR();

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

builder.Services.AddAutoMapper(typeof(CategoryMappingProfile));
builder.Services.AddAutoMapper(typeof(CommentMappingProfile));

builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSetVisitorId();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<OnlineVisitorHub>("/chathub");

app.Run();
