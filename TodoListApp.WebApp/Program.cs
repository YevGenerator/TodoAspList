using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApp.Data;
using TodoListApp.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("UsersDbConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<UsersDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
});

builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<JwtAuthorizationHandler>();
builder.Services.AddTransient<IEmailService, SmtpEmailService>();
builder.Services.AddHttpClient<ITodoListWebApiService, TodoListWebApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7055");
}).AddHttpMessageHandler<JwtAuthorizationHandler>();

builder.Services.AddHttpClient<ITodoTaskWebApiService, TodoTaskWebApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7055");
}).AddHttpMessageHandler<JwtAuthorizationHandler>();

builder.Services.AddHttpClient<ISearchWebApiService, SearchWebApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7055");
}).AddHttpMessageHandler<JwtAuthorizationHandler>();

builder.Services.AddHttpClient<ITodoTagWebApiService, TodoTagWebApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7055");
}).AddHttpMessageHandler<JwtAuthorizationHandler>();

builder.Services.AddHttpClient<ITodoTaskCommentWebApiService, TodoTaskCommentWebApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7055");
}).AddHttpMessageHandler<JwtAuthorizationHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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

await app.RunAsync();
