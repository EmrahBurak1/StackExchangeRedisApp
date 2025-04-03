using StackExchangeRedisApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<RedisService>(); //Uygulama aya�a kalkt���nda bir nesne �rne�i olu�turmas� yeterli her istekte olu�turmas�n� istemedi�imiz i�in AddSingleton yapabiliriz.

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

// Bu �ekilde RedisService i�in bir instance yarat�l�p o service i�inde yaz�lan connect methodu program ba�lat�ld���nda �a��r�lm�� olur.
using (var scope = app.Services.CreateScope())
{
    var redisService = scope.ServiceProvider.GetRequiredService<RedisService>();
    redisService.Connect();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
