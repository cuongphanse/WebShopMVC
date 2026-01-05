using Microsoft.AspNetCore.Authentication.Cookies;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("Mail:Gmail"));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(p =>
{
    p.LoginPath = "/auth/login";
    p.LogoutPath = "/auth/logout";
    p.AccessDeniedPath = "/auth/denied";
    p.ExpireTimeSpan = TimeSpan.FromDays(30);
    p.Cookie.Name = "cuongphan";
});

var app = builder.Build();

// app.MapGet("/", () => "Hello World!");
app.UseStaticFiles(); // su dung css vs js
app.MapControllerRoute(name: "dashboard", pattern: "{area:exists}/{controller=home}/{action=index}/{id?}");
app.MapDefaultControllerRoute();
app.Run();
