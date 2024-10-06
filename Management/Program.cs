using Application.Banners;
using Application.Categorys.CategoryTypes;
using Application.Interfaces.Contexts;
using Application.Posts.AddNewPost;
using Application.Posts.AddNewPost.Dto;
using Application.Posts.AddNewPost.Validator;
using Application.Posts.FavoritePostService;
using Application.Posts.GetPostPLP;
using Application.Posts.PostServices;
using Application.Services.Email;
using Application.Services.Google;
using Application.Services.Sms;
using Application.UriComposer;
using Application.Visitors.VisitorOnline;
using Domain.Users;
using FluentValidation;
using Infrastructures.ExternalApi.ImageServer;
using Infrastructures.IdentityConfigs;
using Infrastructures.MappingProfile;
using Management.Hubs;
using Management.MappingProfiles;
using Management.Utilities.Middlewares;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

#region ConnectionString
string connection = builder.Configuration["ConnectionString:SqlServer"];
builder.Services.AddDbContext<DataBaseContext>(option => option.UseSqlServer(connection));

builder.Services.AddIdentityService(builder.Configuration);
#endregion

builder.Services.AddAuthorization();
builder.Services.ConfigureApplicationCookie(option =>
{
    option.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    option.LoginPath = "/Account/Login";
    option.AccessDeniedPath = "/Account/AccesDenied";
    option.SlidingExpiration = true;
});
string pathToDirctory = "C:\\Users\\Ebrahim\\AppData\\Roaming\\Microsoft\\UserSecrets\\KEY";
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(pathToDirctory))
    .SetApplicationName("SharedCookieApp");

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = ".AspNet.SharedCookie";
    options.Cookie.Path = "/";
});

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["AuthenticationGoogle:ClientId"];
        options.ClientSecret = builder.Configuration["AuthenticationGoogle:ClientSecret"];
    });

builder.Services.AddSignalR();

builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();

builder.Services.AddTransient<EmailService>();
builder.Services.AddTransient<SmsService>();
builder.Services.AddTransient<GoogleRecaptcha>();
builder.Services.AddTransient(typeof(IMongoDbContext<>), typeof(MongoDbContext<>));
builder.Services.AddTransient<IVisitorOnlineService, VisitorOnlineService>();
builder.Services.AddTransient<ICategoryTypeService, CategoryTypeService>();
builder.Services.AddTransient<IAddNewPostService, AddNewPostService>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<IImageUploadService, ImageUploadService>();
builder.Services.AddTransient<IUriComposerService, UriComposerService>();
builder.Services.AddTransient<IFavoritePostService, FavoritePostService>();
builder.Services.AddTransient<IBannersService, BannersService>();

builder.Services.AddAutoMapper(typeof(CategoryMappingProfile));
builder.Services.AddAutoMapper(typeof(ManagementVmMappingProfile));
builder.Services.AddAutoMapper(typeof(PostMappingProfile));

builder.Services.AddTransient<IValidator<AddNewPostDto>, AddNewPostDtoValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSetVisitorId();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<OnlineVisitorHub>("/chatHub");

app.Run();
