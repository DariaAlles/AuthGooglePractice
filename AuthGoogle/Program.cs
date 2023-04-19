using System.Security.Claims;
using AuthGoogle.Data;
using AuthGoogle.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Google;


var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureServices(services =>
{
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("Users"));
});


builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(config =>
{
    config.UseInMemoryDatabase("Memory");
}).AddIdentity<ApplicationUser, ApplicationRole>(config =>
{
    config.Password.RequireDigit = false;
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequiredLength = 6;
    config.Password.RequireUppercase = false;
    config.Password.RequireLowercase = false;

})
.AddEntityFrameworkStores<ApplicationDbContext>();



builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = "13494994995-vubv51q43nlgdrrf4hoftgmfpq9u1mt7.apps.googleusercontent.com";
        options.ClientSecret = "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";
        options.CallbackPath = new PathString("/signin-google");
    });

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Admin/Login";
    config.AccessDeniedPath = "/Home/AccessDenied";
});



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", builder =>
    {
        builder.RequireClaim(ClaimTypes.Role, "Admin");
    });

    options.AddPolicy("Manager", builder =>
    {
        builder.RequireAssertion(x =>
       x.User.HasClaim(ClaimTypes.Role, "Manager") ||
       x.User.HasClaim(ClaimTypes.Role, "Admin"));
    });
}
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    var user = new ApplicationUser
    {
        UserName = "IAdmin",
        FirstName = "Ad",
        LastName = "min"
    };

    var user2 = new ApplicationUser
    {
        UserName = "IManager",
        FirstName = "Mene",
        LastName = "ger"
    };

    var result = userManager.CreateAsync(user, "123qwerty").GetAwaiter().GetResult();
    var result2 = userManager.CreateAsync(user2, "123qwerty").GetAwaiter().GetResult();
    if (result.Succeeded)
    {
        userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Admin")).GetAwaiter().GetResult();
        userManager.AddClaimAsync(user2, new Claim(ClaimTypes.Role, "Manager")).GetAwaiter().GetResult();
        ;
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.MapRazorPages();

app.Run();
