using HAGSJP.WeCasa.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

// Add configurations
builder.Configuration.AddJsonFile("appsettings.json", true, true);

// builder.Configuration.AddJsonFile("config.json", optional: false, reloadOnChange: true);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();

// wecasa.page -> wecasa.page:7131 -> localhost:5153

// localhost:7130 -> localhost:44411 