using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User_Identity_App.Data;
using User_Identity_App.Helpers;
using User_Identity_App.Interfaces;
using User_Identity_App.Models;
using User_Identity_App.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
builder.Configuration.GetConnectionString("conn")));
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddTransient<ISendGridEmail, SendGridEmail>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration.GetSection("SendGrid"));
builder.Services.AddAuthentication()
    .AddFacebook(options =>
    {
        options.AppId = "116271057977051";
        options.AppSecret = "768eef5e9acbf168043959913e2fc3fe";
    })
    .AddGoogle(options => {
        options.ClientId = "261139706036-p5sdtuvgq07bjfdtlma975aln61me1sa.apps.googleusercontent.com";
        options.ClientSecret = "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";
    });

builder.Services.Configure<IdentityOptions>(op =>
{
    op.Password.RequireLowercase = true;
    op.Password.RequireUppercase = true;
    op.Password.RequiredLength = 6;
    op.Password.RequiredUniqueChars = 1;
    op.Password.RequireDigit = true;
    op.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    op.Lockout.MaxFailedAccessAttempts = 5;
    op.SignIn.RequireConfirmedEmail = false;
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
