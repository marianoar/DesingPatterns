using DesignPatternsASP.Configuration;
using Tools.Earn;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<MyConfig>(builder.Configuration.GetSection("MyConfig")); // from appsettings
builder.Services.AddTransient((factory) =>
{
    return new LocalEarnFactory(builder.Configuration.GetSection("MyConfig").GetValue<decimal>("LocalPercentage"));
});
builder.Services.AddTransient((factory) =>
{
    // son dos parametros en el ctor
    return new ForeignEarnFactory(
        builder.Configuration.GetSection("MyConfig").GetValue<decimal>("ForeignPercentage"),
        builder.Configuration.GetSection("MyConfig").GetValue<decimal>("Extra"));
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
