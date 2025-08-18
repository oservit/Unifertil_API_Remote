using System.Text;

namespace Libs
{
    public class FileManager
    {
        /// <summary>
        /// Cria uma nova Instância da Classe.
        /// </summary>
        public FileManager()
        {

        }

        /// <summary>
        /// Retorna um valor booleano indicando se o arquivo existe no caminho especificado. Informar o caminho completo (Diretório + Nome + Extensão).
        /// </summary>
        /// <param name="caminhoCompleto"></param>
        /// <returns></returns>
        public bool Exists(string caminhoCompleto)
        {
            try
            {
                return File.Exists(caminhoCompleto);
            }

            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Salva o arquivo em formato de texto no caminho específicado.
        /// </summary>
        /// <param name="caminho"></param>
        /// <param name="conteudo"></param>
        public void Save(string caminho,string conteudo)
        {
            try
            {
                File.WriteAllText(@caminho, conteudo);
            }

            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Retorna o conteúdo do arquivo especificado em formado de stream (Texto).
        /// </summary>
        /// <param name="caminhoCompleto"></param>
        /// <returns></returns>
        public string Read(string caminhoCompleto)
        {
            try
            {
                string conteudo;
                using (StreamReader streamReader = new StreamReader(caminhoCompleto, Encoding.UTF8))
                {
                    conteudo = streamReader.ReadToEnd();
                }

                return conteudo;
            }

            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
