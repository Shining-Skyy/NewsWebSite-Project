using Application.Categorys.GetMenuItem;
using Application.Interfaces.Contexts;
using Application.Posts.AddNewPost;
using Infrastructures.MappingProfile;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region ConnectionString
string connection = builder.Configuration["ConnectionString:SqlServer"];
builder.Services.AddDbContext<DataBaseContext>(option => option.UseSqlServer(connection));
#endregion

builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();

builder.Services.AddTransient<IGetMenuItemService, GetMenuItemService>();
builder.Services.AddTransient<IAddNewPostService, AddNewPostService>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
