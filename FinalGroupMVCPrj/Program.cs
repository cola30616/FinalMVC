using FinalGroupMVCPrj;
using FinalGroupMVCPrj.Data;
using FinalGroupMVCPrj.Hubs;
using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Services;
using MailKit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//CORS設定
builder.Services.AddCors(options =>
{ options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()); }
);
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var LifeShareLearnConnectionString = builder.Configuration.GetConnectionString("LifeShareLearn") ?? throw new InvalidOperationException("Connection string ' LifeShareLearn' not found.");
builder.Services.AddDbContext<LifeShareLearnContext>(options =>
    options.UseSqlServer(LifeShareLearnConnectionString));
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        //���n�J�ɷ|�۰ʾɨ�o�Ӻ�}
        option.LoginPath = new PathString("/Home/Login");
        //�n�J���S���v���|�ɨ�o�Ӻ�}
        option.AccessDeniedPath = new PathString("/Home/AccessDenied");
    });

//����������w�]�n���n�J���A�A���D[AllowAnonymous]
builder.Services.AddMvc(options => options.Filters.Add(new AuthorizeFilter()));

builder.Services.AddSignalR();


builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<FinalGroupMVCPrj.Services.IMailService, FinalGroupMVCPrj.Services.MailService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapHub<TeacherMsgHub>("/teacherMsgHub");
app.MapHub<PushMsgHub>("/pushMsgHub");

app.Run();