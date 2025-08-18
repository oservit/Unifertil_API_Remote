namespace Libs
{
    /// <summary>
    /// Disponibiliza Métodos Para Manipular Diretórios no Sistema Operacional.
    /// </summary>
    public class Folder
    {

        /// <summary>
        /// Cria o Diretório (Pasta) no Caminho Especificado Caso Ele Não Exista.
        /// </summary>
        /// <param name="caminho"></param>
        /// <returns></returns>
        public static bool Create(string caminho)
        {
            if (!Directory.Exists(caminho))
            {
                try
                {
                    Directory.CreateDirectory(caminho);
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao Criar Diretório", ex);
                }
            }
            else
                return true;
        }
    }
}
