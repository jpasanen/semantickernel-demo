namespace SemanticKernelDemoApi.Settings;

using System.ComponentModel.DataAnnotations;

public class EmailSettings
{
    public const string SectionKey = "Email";

    /// <summary>
    /// Gets or sets the connection string from Azure Communication Service.
    /// </summary>
    [Required]
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sender email from Azure Communication Service.
    /// </summary>
    [Required]
    public string Sender { get; set; } = string.Empty;
}
