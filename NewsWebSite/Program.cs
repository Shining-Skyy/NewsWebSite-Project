using Application.Categorys.GetMenuItem;
using Application.HomePageService;
using Application.Interfaces.Contexts;
using Application.Posts.AddNewPost;
using Application.Posts.FavoritePostService;
using Application.Posts.GetPostPDP;
using Application.Posts.GetPostPLP;
using Application.Posts.PostServices;
using Application.UriComposer;
using Infrastructures.ExternalApi.ImageServer;
using Infrastructures.IdentityConfigs;
using Infrastructures.MappingProfile;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
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

builder.Services.ConfigureApplicationCookie(options => {
    options.Cookie.Name = ".AspNet.SharedCookie";
    options.Cookie.Path = "/";
});

builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();

builder.Services.AddTransient<IGetMenuItemService, GetMenuItemService>();
builder.Services.AddTransient<IUriComposerService, UriComposerService>();
builder.Services.AddTransient<IGetPostPLPService, GetPostPLPService>();
builder.Services.AddTransient<IGetPostPDPService, GetPostPDPService>();
builder.Services.AddTransient<IFavoritePostService, FavoritePostService>();
builder.Services.AddTransient<IHomePageService, HomePageService>();

builder.Services.AddAutoMapper(typeof(CategoryMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
