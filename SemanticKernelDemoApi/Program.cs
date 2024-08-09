using Microsoft.AspNetCore.Mvc;
using SemanticKernelDemoApi.Services;
using SemanticKernelDemoApi.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<KernelService>();

builder.Services.AddOptions<OpenAISettings>()
            .Configure<IConfiguration>((settings, configuration) => configuration.GetSection(OpenAISettings.SectionKey).Bind(settings))
            .ValidateDataAnnotations();
builder.Services.AddOptions<EmailSettings>()
            .Configure<IConfiguration>((settings, configuration) => configuration.GetSection(EmailSettings.SectionKey).Bind(settings))
            .ValidateDataAnnotations();

var corsPolicyName = "CorsPolicy";

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: corsPolicyName, builder =>
    {
        builder.AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true)
            .AllowAnyOrigin();
    });
});

var app = builder.Build();
app.UseCors(corsPolicyName);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/chat", async (KernelService service, [FromQuery] string msg) =>
{
    return await service.Execute(msg);
})
.WithName("Chat")
.WithOpenApi();

app.Run();
