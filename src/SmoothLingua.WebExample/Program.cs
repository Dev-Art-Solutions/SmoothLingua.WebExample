using Newtonsoft.Json;
using SmoothLingua.Conversations;
using SmoothLingua.WebExample.Components;
using SmoothLingua.WebExample.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder
    .Services
    .AddSingleton<IChatService, ChatService>()
    .AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

var chatService = app.Services.GetService<IChatService>()!;
var domain = chatService.GetDomain();
await chatService.Train(domain, default);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
