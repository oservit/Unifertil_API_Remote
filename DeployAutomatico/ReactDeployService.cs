using Renci.SshNet;
using System.Diagnostics;
using System.Text;
using Libs;

namespace DeployAutomatico
{
    public class ReactDeployService
    {
        private readonly DeployConfig _config;

        public ReactDeployService(DeployConfig config)
        {
            _config = config;
        }

        public void Run()
        {
            Build();
            ClearRemoteDirectory();
            UploadDist();
        }

        private void Build()
        {
            Console.WriteLine("\n[Executando Build]\n");

            var envPath = Path.Combine(_config.ReactProjectPath, ".env.production");
            if (!File.Exists(envPath))
                throw new FileNotFoundException("Arquivo .env.production não encontrado.", envPath);

            Console.WriteLine($"Caminho do projeto React: {_config.ReactProjectPath}");
            Console.WriteLine($"Caminho do arquivo .env.production: {envPath}");
            Console.WriteLine($"URL da API para o build: {_config.ReactApiUrl}");

            var originalEnv = File.ReadAllText(envPath);

            try
            {
                var lines = File.ReadAllLines(envPath);
                bool updated = false;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("VITE_API_URL="))
                    {
                        lines[i] = $"VITE_API_URL={_config.ReactApiUrl}";
                        updated = true;
                        break;
                    }
                }

                if (!updated)
                {
                    Console.WriteLine("VITE_API_URL não encontrado no .env.production. Adicionando no final.");
                    lines = lines.Append($"VITE_API_URL={_config.ReactApiUrl}").ToArray();
                }

                File.WriteAllLines(envPath, lines);

                var psi = new ProcessStartInfo
                {
                    FileName = OperatingSystem.IsWindows() ? "cmd.exe" : "/bin/bash",
                    Arguments = OperatingSystem.IsWindows() ? "/c yarn build" : "-c \"yarn build\"",
                    WorkingDirectory = _config.ReactProjectPath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = new Process();
                process.StartInfo = psi;

                var stdErrBuilder = new StringBuilder();
                var stdOutBuilder = new StringBuilder();

                process.OutputDataReceived += (_, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        stdOutBuilder.AppendLine(e.Data);
                };

                process.ErrorDataReceived += (_, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        stdErrBuilder.AppendLine(e.Data);
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    Console.WriteLine("Erro ao executar o build:");
                    Console.WriteLine(stdErrBuilder.ToString());
                    throw new Exception("Build do frontend falhou.");
                }
            }
            finally
            {
                File.WriteAllText(envPath, originalEnv);
            }
        }

        private void ClearRemoteDirectory()
        {
            using var ssh = new SshClient(_config.RemoteHost, _config.RemotePort, _config.Username, Crypto.Decrypt(_config.Password));
            ssh.Connect();

            ssh.RunCommand($"rm -rf {_config.ReactRemoteDeployPath}");
            ssh.RunCommand($"mkdir -p {_config.ReactRemoteDeployPath}/assets/relevos");

            ssh.Disconnect();
        }

        private void UploadDist()
        {
            Console.WriteLine("\n[Enviando arquivos]\n");

            var fullDistPath = Path.GetFullPath(Path.Combine(_config.ReactProjectPath, _config.ReactPublishOutputPath));
            var files = Directory.GetFiles(fullDistPath, "*", SearchOption.AllDirectories);

            using var client = new SftpClient(_config.RemoteHost, _config.RemotePort, _config.Username, Crypto.Decrypt(_config.Password));
            client.Connect();

            foreach (var file in files)
            {
                var relativePath = Path.GetRelativePath(fullDistPath, file);
                var remoteFilePath = Path.Combine(_config.ReactRemoteDeployPath, relativePath).Replace("\\", "/");
                var remoteDir = Path.GetDirectoryName(remoteFilePath)!.Replace("\\", "/");

                if (!client.Exists(remoteDir))
                    CreateRemoteDirectory(client, remoteDir);

                using var fs = File.OpenRead(file);
                client.UploadFile(fs, remoteFilePath, true);
                Console.WriteLine($"Enviado: {relativePath}");
            }

            client.Disconnect();
        }

        private void CreateRemoteDirectory(SftpClient client, string path)
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
    }
}
