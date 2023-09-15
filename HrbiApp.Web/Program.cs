using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DBContext;
using HrbiApp.Web.Areas.Common;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Localization;
using System.Reflection;
using HrbiApp.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddRazorPages();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc()
  .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
  .AddViewLocalization()
  .AddDataAnnotationsLocalization(o =>
  {
      o.DataAnnotationLocalizerProvider = (type, factory) =>
  {
      //return factory.Create(typeof(SharedResource));
      var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName!);
      return factory.Create(nameof(SharedResource), assemblyName.Name!);
  };
  });


builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
}
    )
    .AddEntityFrameworkStores<ApplicationDBContext>();
builder.Services.AddControllersWithViews()
    .AddViewLocalization().AddDataAnnotationsLocalization(
    o =>
    {
        o.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            return factory.Create(typeof(SharedResource));
        };
    });

builder.Services.AddScoped<ExcptionHandler>();
builder.Services.AddScoped<Validators>();
builder.Services.AddScoped<NotificationCenter>();
builder.Services.AddScoped<CoreServices>();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
            new CultureInfo("en-US"),
            new CultureInfo("ar")
        };
    supportedCultures[1].DateTimeFormat = supportedCultures[0].DateTimeFormat;
    options.DefaultRequestCulture = new RequestCulture(culture: "ar", uiCulture: "ar");
    options.DefaultRequestCulture.UICulture.DateTimeFormat = supportedCultures[0].DateTimeFormat;
    options.DefaultRequestCulture.Culture.DateTimeFormat = supportedCultures[0].DateTimeFormat;
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.RequestCultureProviders = new List<IRequestCultureProvider>
        {
            new QueryStringRequestCultureProvider(),
            new CookieRequestCultureProvider()
        };



});


var app = builder.Build();
var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);

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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
