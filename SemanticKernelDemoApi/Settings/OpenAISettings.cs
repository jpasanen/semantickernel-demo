using System.ComponentModel.DataAnnotations;

namespace SemanticKernelDemoApi.Settings;

public class OpenAISettings
{
    public const string SectionKey = "OpenAI";

    /// <summary>
    /// Gets or sets the deployment name from Azure OpenAI Service.
    /// </summary>
    [Required]
    public string DeploymentName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the endpoint from Azure OpenAI Service.
    /// </summary>
    [Required]
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the api key from Azure OpenAI Service.
    /// </summary>
    [Required]
    public string ApiKey { get; set; } = string.Empty;
}
