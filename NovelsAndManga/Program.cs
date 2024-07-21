using Microsoft.EntityFrameworkCore;
using NovelsManga.DataAccess.Data;
using NovelsManga.DataAccess.Repository;
using NovelsManga.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using NovelsManga.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MangaNovelDatabase>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MangaNovelsConnection")));
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<MangaNovelDatabase>();
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<MangaNovelDatabase>();
builder.Services.AddRazorPages();
builder.Services.AddRouting(options=> options.LowercaseUrls = true);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
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
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Main}/{controller=Home}/{action=Index}/{id?}");

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//        name: "areas",
//        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

//    endpoints.MapControllerRoute(
//        name: "default",
//        pattern: "{area=Main}/{controller=Home}/{action=Index}/{id?}");
//});

app.Run();
