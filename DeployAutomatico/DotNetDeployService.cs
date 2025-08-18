using Renci.SshNet;
using System.Diagnostics;
using Libs;

public class DotNetDeployService
{
    private readonly DeployConfig _config;

    public DotNetDeployService(DeployConfig config)
    {
        _config = config;
    }

    public void Run()
    {
        Publish();
        StopRemoteService();
        UploadAndReplace();
        CleanupLocal();
        RestartService();
    }

    private void Publish()
    {
        try
        {
            var fullProjectPath = Path.GetFullPath(_config.DotNetProjectPath);
            var projectDir = Path.GetDirectoryName(fullProjectPath)!;
            var publishDir = Path.Combine(projectDir, _config.DotNetPublishOutputPath);
            var fullPublishPath = Path.GetFullPath(publishDir);

            Console.WriteLine($"\n[Iniciando Publish]\n");
            Console.WriteLine($"Projeto: {fullProjectPath}");
            Console.WriteLine($"Diretorio: {fullPublishPath}");

            if (!Directory.Exists(projectDir))
                throw new DirectoryNotFoundException($"Diretório do projeto não encontrado: {projectDir}");

            if (Directory.Exists(fullPublishPath))
                Directory.Delete(fullPublishPath, true);

            Directory.CreateDirectory(fullPublishPath);

            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"publish \"{fullProjectPath}\" -o \"{fullPublishPath}\" -c Release",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = projectDir
            };

            var process = Process.Start(psi)!;
            process.WaitForExit();

            var error = process.StandardError.ReadToEnd();
            if (!string.IsNullOrWhiteSpace(error))
                Console.WriteLine($"Erro no publish: {error}");
        }

        catch
        {
            throw;
        }
    }

    private void UploadAndReplace()
    {
        try
        {
            Console.WriteLine("\n[Enviando arquivos por SSH]\n");

            using var client = new SftpClient(_config.RemoteHost, _config.RemotePort, _config.Username, Crypto.Decrypt(_config.Password));
            client.Connect();

            var projectDir = Path.GetDirectoryName(Path.GetFullPath(_config.DotNetProjectPath))!;
            var fullPublishPath = Path.Combine(projectDir, _config.DotNetPublishOutputPath);
            var localFiles = Directory.GetFiles(fullPublishPath, "*", SearchOption.AllDirectories);

            foreach (var file in localFiles)
            {
                var relativePath = Path.GetRelativePath(fullPublishPath, file);
                if (_config.IgnoreFiles.Contains(relativePath)) continue;

                var remoteFilePath = Path.Combine(_config.DotNetRemoteDeployPath, relativePath).Replace("\\", "/");
                var remoteDir = Path.GetDirectoryName(remoteFilePath)!.Replace("\\", "/");

                if (!client.Exists(remoteDir))
                    CreateRemoteDirectory(client, remoteDir);

                using var fs = File.OpenRead(file);
                client.UploadFile(fs, remoteFilePath, true);
                Console.WriteLine($"Enviado: {relativePath}");
            }

            client.Disconnect();
        }

        catch
        {
            throw;
        }
    }

    private void CleanupLocal()
    {
        var projectDir = Path.GetDirectoryName(Path.GetFullPath(_config.DotNetProjectPath))!;
        var fullPublishPath = Path.Combine(projectDir, _config.DotNetPublishOutputPath);

        if (Directory.Exists(fullPublishPath))
            Directory.Delete(fullPublishPath, true);
    }

    private void StopRemoteService()
    {
        try
        {
            Console.WriteLine("\n[Verificando processo remoto existente]\n");
            using var ssh = new SshClient(_config.RemoteHost, _config.RemotePort, _config.Username, Crypto.Decrypt(_config.Password));
            ssh.Connect();

            var grepCmd = $"ps aux | grep {_config.RemoteServiceName} | grep -v grep";
            var result = ssh.RunCommand(grepCmd);

            if (!string.IsNullOrWhiteSpace(result.Result))
            {
                Console.WriteLine("Processo encontrado e será encerrado:");
                Console.WriteLine(result.Result);

                var lines = result.Result.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2 && int.TryParse(parts[1], out var pid))
                    {
                        ssh.RunCommand($"kill {pid}");
                        Console.WriteLine($"Processo {pid} encerrado.");
                    }
                }
            }
            else
                Console.WriteLine("Nenhum processo correspondente foi encontrado.");

            ssh.Disconnect();
        }

        catch
        {
            throw;
        }
    }

    private void RestartService()
    {
        try
        {
            Console.WriteLine("\n[Reiniciando aplicação remotamente]\n");

            using var ssh = new SshClient(_config.RemoteHost, _config.RemotePort, _config.Username, Crypto.Decrypt(_config.Password));
            ssh.Connect();

            var cdResult = ssh.RunCommand($"cd {_config.DotNetRemoteDeployPath}");
            if (!string.IsNullOrWhiteSpace(cdResult.Error))
            {
                Console.WriteLine("Erro ao acessar diretório remoto:");
                Console.WriteLine(cdResult.Error);
                ssh.Disconnect();
                return;
            }

            var remoteRunCommand = $"cd {_config.DotNetRemoteDeployPath}; ASPNETCORE_ENVIRONMENT=Production nohup dotnet {_config.RemoteServiceName} > /dev/null 2>&1 &";
            ssh.RunCommand(remoteRunCommand);

            Thread.Sleep(3000);

            var grepCmd = $"ps aux | grep '[d]otnet {_config.RemoteServiceName}'";
            var result = ssh.RunCommand(grepCmd);
            if (!string.IsNullOrWhiteSpace(result.Result))
            {
                Console.WriteLine("Novo processo iniciado:");
                Console.WriteLine(result.Result);
            }
            else
            {
                Console.WriteLine("Nenhum processo foi encontrado após reinício.");
            }

            ssh.Disconnect();
        }

        catch
        {
            throw;
        }
    }

    private void CreateRemoteDirectory(SftpClient client, string path)
    {
        try
        {
            var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var current = "/";
            foreach (var segment in segments)
            {
                current = Path.Combine(current, segment).Replace("\\", "/");
                if (!client.Exists(current))
                    client.CreateDirectory(current);
            }
        }

        catch
        {
            throw;
        }
    }
}
