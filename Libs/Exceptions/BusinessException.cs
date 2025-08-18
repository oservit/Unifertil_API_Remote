using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.Exceptions
{
    [Serializable]
    public class BusinessException : Exception
    {
        public int Code { get; set; }

        public BusinessException(int code, string message)
            : base(message)
        {
            Code = code;
        }

        public BusinessException(int code, string message, Exception inner)
            : base(message, inner)
        {
            Code = code;
        }

        public BusinessException(string message)
            : this(300, message)
        {
        }

        public BusinessException(string message, Exception inner)
            : this(300, message, inner)
        {
        }

        public BusinessException()
        {
            Code = 300; // Código de erro padrão
        }
    }
}
