using Libs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Libs.Exceptions
{
    [Serializable]
    public class NotFoundException : BusinessException
    {
        public DataResult Result { get; }

        public NotFoundException(int code, string message)
            : this(code, message, null)
        {
        }

        public NotFoundException(int code, string message, Exception? inner)
            : base(message, inner)
        {
            base.Code = code;
            Result = new DataResult
            {
                Data = this,
                Success = false,
                Message = message
            };
        }

        public NotFoundException()
            : this(404, "Registro não encontrado.", null)
        {
        }

        public NotFoundException(string message)
            : this(404, message, null)
        {
        }
    }
}
