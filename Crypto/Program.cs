using System;
using Libs;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("[Ferramenta de Criptografia]");
        Console.WriteLine("1 - Criptografar senha");
        Console.WriteLine("2 - Descriptografar senha");
        Console.Write("Escolha uma opção: ");

        var opcao = Console.ReadLine();

        Console.Write("Digite o texto: ");
        var input = Console.ReadLine();

        switch (opcao)
        {
            case "1":
                var criptografado = Crypto.Encrypt(input);
                Console.WriteLine($"Criptografado: {criptografado}");
                break;
            case "2":
                try
                {
                    var descriptografado = Crypto.Decrypt(input);
                    Console.WriteLine($"Descriptografado: {descriptografado}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao descriptografar: {ex.Message}");
                }
                break;
            default:
                Console.WriteLine("Opção inválida.");
                break;
        }

        Console.WriteLine("Pressione qualquer tecla para sair...");
        Console.ReadKey();
    }
}
