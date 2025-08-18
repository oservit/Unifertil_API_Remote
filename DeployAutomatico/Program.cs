using DeployAutomatico;
using System.Text.Json;
class Program
{
    static void Main(string[] args)
    {
        try
        {
            var configs = LoadConfigs("deploysettings.json");

            Console.WriteLine("Selecione o ambiente para deploy:");
            for (int i = 0; i < configs.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {configs[i].Name}");
            }

            Console.Write("Opção: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out var index) || index < 1 || index > configs.Count)
            {
                Console.WriteLine("Opção inválida.");
                return;
            }

            var config = configs[index - 1];

            switch (config.Opcao)
            {
                case OpcaoDeployEnum.Backend:
                    RunDotNetDeploy(config);
                    break;

                case OpcaoDeployEnum.Frontend:
                    RunReactDeploy(config);
                    break;

                case OpcaoDeployEnum.Completo:
                default:
                    RunDotNetDeploy(config);
                    RunReactDeploy(config);
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[Erro]: {ex.Message}");
        }
    }

    static void RunDotNetDeploy(DeployConfig config)
    {
        Console.WriteLine("\n[Iniciando Deploy do Backend]");
        var deployer = new DotNetDeployService(config);
        deployer.Run();
    }

    static void RunReactDeploy(DeployConfig config)
    {
        Console.WriteLine("\n[Iniciando Deploy do Frontend]");
        var deployer = new ReactDeployService(config);
        deployer.Run();
    }

    static List<DeployConfig> LoadConfigs(string path)
    {
        var fullPath = Path.Combine(AppContext.BaseDirectory, path);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Arquivo de configuração '{fullPath}' não encontrado.");

        var json = File.ReadAllText(fullPath);
        return JsonSerializer.Deserialize<List<DeployConfig>>(json)!;
    }
}
