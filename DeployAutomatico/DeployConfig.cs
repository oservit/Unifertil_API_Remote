using DeployAutomatico;

public class DeployConfig
{
    public string Name { get; set; } = string.Empty;

    // .NET
    public string DotNetProjectPath { get; set; } = string.Empty;
    public string DotNetPublishOutputPath { get; set; } = string.Empty;
    public string RemoteServiceName { get; set; } = string.Empty;
    public string DotNetRemoteDeployPath { get; set; } = string.Empty;

    // React
    public string ReactProjectPath { get; set; } = string.Empty;
    public string ReactPublishOutputPath { get; set; } = string.Empty;
    public string ReactApiUrl { get; set; } = string.Empty;
    public string ReactRemoteDeployPath { get; set; } = string.Empty;

    // Comum
    public string RemoteHost { get; set; } = string.Empty;
    public int RemotePort { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<string> IgnoreFiles { get; set; } = new();

    public OpcaoDeployEnum Opcao { get; set; } = OpcaoDeployEnum.Completo;
}
