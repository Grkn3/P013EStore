using Microsoft.AspNetCore.Authentication.Cookies;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Admin/Login"; // oturum açmayan kullanıcıların giriş için gönderileceği adres 
    x.LogoutPath = "/Admin/Logout";
    x.AccessDeniedPath = "/AccessDenied"; // yetkilendirme ile ekrana erişim hakk olmayan kullanıcıların gönderileceği sayfa
    x.Cookie.Name = "Administrator"; // Oluşacak Kukinin ismi 
    x.Cookie.MaxAge = TimeSpan.FromDays(1); // Oluşsacak kukinin yaşam süresi (1 gün)

}); // oturum işlemleri için

//Uygulama admin paneli için admin yetkilendirme ayarları
builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminPolicy", p => p.RequireClaim("Role", "Admin")); // admin paneline giriş yapma yetkisine sahip olanları bu kuralla kontrol edeceğiz.
    x.AddPolicy("UserPolicy", p => p.RequireClaim("Role", "User")); // admin dışında yetkilendirme kullanırsak bu kuralı kullanabiliriz(siteye üye girişi yapanları ön yüzde bir panele eriştirme gibi)
});

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
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
            name: "admin",
            pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}"
          );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
