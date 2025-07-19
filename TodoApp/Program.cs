using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using TodoApp.Areas.Identity.Data;
using TodoApp.Services;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TodoAppContext") ?? throw new InvalidOperationException("Connection string 'TodoAppContext' not found.")));
var connectionString = builder.Configuration.GetConnectionString("TodoAppIdentityDbContextConnection") ?? throw new InvalidOperationException("Connection string 'TodoAppIdentityDbContextConnection' not found.");;

builder.Services.AddDbContext<TodoAppIdentityDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<TodoAppIdentityDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IEmailSender, EmailSender>();

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

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapRazorPages();

app.Run();