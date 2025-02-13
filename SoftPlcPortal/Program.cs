using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using SoftPlcPortal.Application.Services;
using SoftPlcPortal.Components;
using SoftPlcPortal.Infrastructure.Database;
using SoftPlcPortal.Infrastructure.Plc;
using SoftPlcPortal.Infrastructure.SoftPlc;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<AppDbContext>((services, config) =>
{
    config.UseSqlite("Data Source=softplc-portal.db");
});

builder.Services.AddHttpClient();
builder.Services.AddSingleton<SoftPlcClientFactory>();
builder.Services.AddSingleton<S7ClientFactory>();

builder.Services.AddScoped<PlcConfigService>();
builder.Services.AddScoped<DataBlocksService>();
builder.Services.AddScoped<DbFieldsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


using (var scope = app.Services.CreateScope())
{
    using (var context = scope.ServiceProvider.GetRequiredService<AppDbContext>())
    {
        context.Database.Migrate();
    }
}

app.Run();
