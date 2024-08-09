using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Plugins.OpenApi;
using SemanticKernelDemoApi.Settings;

namespace SemanticKernelDemoApi.Services;

// Kernel service for communicating with the AI model.
public class KernelService
{
    private readonly Kernel _kernel;
    private readonly OpenAIPromptExecutionSettings _openAIPromptExecutionSettings;
    private readonly ChatHistory _chatHistory;

    public KernelService(IOptionsMonitor<OpenAISettings> options)
    {
        // Create kernel instance and add Azure OpenAI chat completion service.
        var builder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(
            options.CurrentValue.DeploymentName,
            options.CurrentValue.Endpoint,
            options.CurrentValue.ApiKey);

        builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));

        // Build the kernel.
        _kernel = builder.Build();

#pragma warning disable SKEXP0040 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        _kernel.ImportPluginFromOpenApiAsync(
           pluginName: "products",
           uri: new Uri("https://localhost:7194/swagger/v1/swagger.json"),
           executionParameters: new OpenApiFunctionExecutionParameters()
           {
               EnablePayloadNamespacing = true
           }
        ).GetAwaiter().GetResult();
#pragma warning restore SKEXP0040 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        _chatHistory = new ChatHistory();

        // We want to enable auto invoking functions to be able to automatically utilize backend APIs.
        _openAIPromptExecutionSettings = new()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        };
    }

    public async Task<string> Execute(string message)
    {
        // Add user prompt to chat history.
        _chatHistory.AddMessage(AuthorRole.User, message);

        // Create chat completion service instance.
        var chatCompletionService = _kernel.Services.GetRequiredService<IChatCompletionService>();

        // Get the response from the AI.
        var result = await chatCompletionService.GetChatMessageContentAsync(
            _chatHistory,
            executionSettings: _openAIPromptExecutionSettings,
            kernel: _kernel
        );

        return result.ToString();
    }
}
