using TodoListApp.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<ITodoListWebApiService, TodoListWebApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7055");
});

builder.Services.AddHttpClient<ITodoTaskWebApiService, TodoTaskWebApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7055");
});

builder.Services.AddHttpClient<ISearchWebApiService, SearchWebApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7055");
});

builder.Services.AddHttpClient<ITodoTagWebApiService, TodoTagWebApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7055");
});

builder.Services.AddHttpClient<ITodoTaskCommentWebApiService, TodoTaskCommentWebApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7055");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
