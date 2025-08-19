using System.Xml.Serialization;
using System.Text.Json;
using Libs.Common;

namespace Libs
{
    /// <summary>
    /// Disponibiliza os Métodos Para Serialização e Deserialização de Objetos.
    /// </summary>
    public static class Serialization
    {

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Serializa o Objeto Em Formato XML e Retorna como Stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public static byte[] Serialize<T>(T objeto)
        {
            if (objeto != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    new XmlSerializer(objeto.GetType()).Serialize(ms, objeto);
                    return ms.ToArray();
                }
            }
            else
                throw new Exception("Objeto Não Pode Ser Nulo");
        }

        /// <summary>
        /// Retorna um Objeto do Tipo Especificado a Partir da Stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public static object Deserialize(byte[] stream, Type tipo)
        {
            if (stream != null)
            {
                using (MemoryStream ms = new MemoryStream(stream))
                {
                    return new XmlSerializer(tipo).Deserialize(ms);
                }
            }
            return null;
        }



        /// <summary>
        /// Serializa o Objeto Em Formato JSON e Retorna como String.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Objeto"></param>
        /// <returns></returns>
        public static string SerializarJson<T>(T Objeto)
        {
            if (Objeto != null)
            {
                return JsonSerializer.Serialize<T>(Objeto);
            }
            else
                throw new Exception("Objeto Não Pode Ser Nulo");
        }



        /// <summary>
        /// Retorna um Objeto do Tipo Especificado a Partir do Json em Formado de String.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DeserializeJson(string json, Type type)
        {
            if (!string.IsNullOrEmpty(json))
            {
                return JsonSerializer.Deserialize(json, type);
            }

            return null;
        }

        public static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, Options);
        }

        public static T DeserializeResult<T>(DataResult result)
        {
            return JsonSerializer.Deserialize<T>(result.Data.ToString(), Options);
        }
    }
}
