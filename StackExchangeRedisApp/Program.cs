using StackExchangeRedisApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<RedisService>(); //Uygulama ayaða kalktýðýnda bir nesne örneði oluþturmasý yeterli her istekte oluþturmasýný istemediðimiz için AddSingleton yapabiliriz.

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

// Bu þekilde RedisService için bir instance yaratýlýp o service içinde yazýlan connect methodu program baþlatýldýðýnda çaðýrýlmýþ olur.
using (var scope = app.Services.CreateScope())
{
    var redisService = scope.ServiceProvider.GetRequiredService<RedisService>();
    redisService.Connect();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
