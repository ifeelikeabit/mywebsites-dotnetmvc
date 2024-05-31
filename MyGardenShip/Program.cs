using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MyGardenShip.Data;
using MyGardenShip.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IEmailSender, SmtpEmailSender>(i =>
new SmtpEmailSender(
    builder.Configuration["EmailSender:Host"],
    builder.Configuration.GetValue<int>("EmailSender:Port"),
    builder.Configuration.GetValue<bool>("EmailSender:EnableSSL"),
    builder.Configuration["EmailSender:Username"],
    builder.Configuration["EmailSender:Password"]
));


builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<IdentityContext>(options =>
{
    var config = builder.Configuration;//appsettins.jsondaki verileri alır.(Geliştirme aşamasında appsettings.Develoment.json dan veri alınır.)
     options.UseSqlServer(builder.Configuration.GetConnectionString("SQLserver"));//Database bağlantı yazısı alınarak database e bağlanır (Mssql Somee)
});
builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();//Identityye User ve Role ile context eklenir.
builder.Services.Configure<IdentityOptions>(options =>
{
    //Kullanıcı için gerekli olan özelliklerin yeterliliklerinin ayarlanması.

    //Şifre için
    options.Password.RequiredLength = 8; // Gerekli olan en kısa uzunluk
    options.Password.RequireNonAlphanumeric = false;//Özel karakter gereksinimi
    options.Password.RequireLowercase = false;//Küçük harf gereksinimi
    options.Password.RequireUppercase = false;//Büyük harf gereksinimi
    options.Password.RequireDigit = false;//Sayı gereksinimi

    //Kullanıcı için
    options.User.RequireUniqueEmail = true;//Benzersiz başka bir deyişle başkalarında olmayan email gereksinimi
    //Hesap için
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //Başarısız hesap açma istekleri sonucunda hesabın kaç dakika kitleneceği
    options.Lockout.MaxFailedAccessAttempts = 5;//Hesabın kitlenmesi için gereken yanlış hesap açma isteği sayısı
    options.Lockout.AllowedForNewUsers = true;//Hesap kitleme sisteminin yeni kullanıcılarda gerek olup olmadığı

    //Giriş için
    options.SignIn.RequireConfirmedEmail = true;//Giriş yapmak için onaylı hesap gereksinimi

});

builder.Services.ConfigureApplicationCookie(options =>
{
    //Çerez özelliklerinin ayarlanması
    options.LoginPath = "/User/Login";//Giriş sayfasının yolu
    // options.AccessDeniedPath=""; //Eğer erişim reddedilirse yönlendirileceği sayfanın yolu(Bu projede gerek yoktu)
    options.SlidingExpiration = true;//Çerezin süresi geçmeden kullanıcı siteye girerse çerezin iptal süresini sıfırlar
    options.ExpireTimeSpan = TimeSpan.FromDays(10);//Çerezin iptal süresi

});

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
IdentitySeedData.IdentityTestUser(app);
app.Run();
