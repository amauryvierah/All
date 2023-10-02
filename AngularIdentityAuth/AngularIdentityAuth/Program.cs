var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services
    .AddAuthentication(o =>
    {
        o.DefaultScheme = "Application";
        o.DefaultSignInScheme = "External";
    })
    .AddCookie("Application")
    .AddCookie("External", options => {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.Cookie.IsEssential = true;
        options.Events.OnSigningIn = async (context) =>
        {
            context.Properties.ExpiresUtc = DateTimeOffset.Now.AddMinutes(30);
            context.CookieOptions.Expires = context.Properties.ExpiresUtc?.ToUniversalTime();
        };
    })
    .AddGoogle(o =>
    {
        o.ClientId = "449692155788-gr91ge1qn5l6voopms16tth1tehp8s67.apps.googleusercontent.com";
        o.ClientSecret = "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";
        o.SaveTokens = true;
        o.Events.OnRedirectToAuthorizationEndpoint = context =>
        {
            context.Response.Redirect($"{context.RedirectUri}&prompt=consent");
            return Task.CompletedTask;
        };
    });

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute("identity", "/Account/{action=Login}");
//    endpoints.MapControllerRoute("weather", "/WeatherForecast/{action=Get}");
//});

app.MapFallbackToFile("index.html"); ;

app.Run();
