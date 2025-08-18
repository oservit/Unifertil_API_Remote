using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Libs.Exceptions
{
    [Serializable]
    public class ArgumentException : SystemException
    {
        private readonly string? _paramName;

        // Construtor padrão
        public ArgumentException()
            : base("Value does not fall within the expected range.")
        {
            HResult = -2147024809; // Código de erro padrão (COR_E_ARGUMENT)
        }

        // Construtor que aceita uma mensagem de erro
        public ArgumentException(string? message)
            : base(message)
        {
            HResult = -2147024809; // Código de erro padrão
        }

        // Construtor que aceita uma mensagem de erro e uma exceção interna
        public ArgumentException(string? message, Exception? innerException)
            : base(message, innerException)
        {
            HResult = -2147024809; // Código de erro padrão
        }

        // Construtor que aceita uma mensagem de erro e o nome do parâmetro que causou a exceção
        public ArgumentException(string? message, string? paramName)
            : base(message)
        {
            _paramName = paramName;
            HResult = -2147024809; // Código de erro padrão
        }

        // Construtor que aceita uma mensagem de erro, o nome do parâmetro que causou a exceção, e uma exceção interna
        public ArgumentException(string? message, string? paramName, Exception? innerException)
            : base(message, innerException)
        {
            _paramName = paramName;
            HResult = -2147024809; // Código de erro padrão
        }

        // Construtor para desserialização
        protected ArgumentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _paramName = info.GetString("ParamName");
        }

        // Propriedade para o nome do parâmetro que causou a exceção
        public virtual string? ParamName => _paramName;

        // Sobrescrita da propriedade Message para incluir o nome do parâmetro, se disponível
        public override string Message
        {
            get
            {
                string s = base.Message;
                if (!string.IsNullOrEmpty(_paramName))
                {
                    s += $" Parameter name: {_paramName}";
                }
                return s;
            }
        }

        // Método para obter os dados de serialização
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ParamName", _paramName, typeof(string));
        }

        // Métodos estáticos para lançar exceções em condições específicas
        public static void ThrowIfNullOrEmpty(string? argument, string? paramName = null)
        {
            if (string.IsNullOrEmpty(argument))
            {
                ArgumentNullException.ThrowIfNull(argument, paramName);
                throw new ArgumentException("Value cannot be null or empty.", paramName);
            }
        }

        public static void ThrowIfNullOrWhiteSpace(string? argument, string? paramName = null)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                ArgumentNullException.ThrowIfNull(argument, paramName);
                throw new ArgumentException("Value cannot be null, empty, or whitespace.", paramName);
            }
        }
    }
}
