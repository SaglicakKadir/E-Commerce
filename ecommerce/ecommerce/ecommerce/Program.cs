using Microsoft.EntityFrameworkCore;
using System.Globalization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ecommerce.Models.EcommerceContext>(x => x.UseSqlServer("Data Source = LAPTOP-K7VMCFFS\\KADORYZ; Initial Catalog = EvCommerce; Integrated Security = True"));
builder.Services.AddDbContext<ecommerce.Areas.Admin.Models.UserContext >(x => x.UseSqlServer("Data Source = LAPTOP-K7VMCFFS\\KADORYZ; Initial Catalog = EvCommerce; Integrated Security = True"));
builder.Services.AddSession();

var app = builder.Build();


CultureInfo cultureInfo = new CultureInfo("tr-TR");
cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
