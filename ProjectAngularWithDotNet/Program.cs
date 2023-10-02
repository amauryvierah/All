var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

/*Use this for auth*/
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
        o.ClientId = "449692155788-t9bnik5ptqkt328outt198qu8arveffd.apps.googleusercontent.com";
        o.ClientSecret = "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";
        o.SaveTokens = true;
        o.Events.OnRedirectToAuthorizationEndpoint = context =>
        {
            context.Response.Redirect($"{context.RedirectUri}&prompt=consent");
            return Task.CompletedTask;
        };
    });
////////////////////

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

/*Use this for auth*/
app.UseAuthentication();
app.UseAuthorization();
////////////////////

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
