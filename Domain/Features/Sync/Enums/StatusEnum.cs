using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Features.Sync.Enums
{
    public enum StatusEnum
    {
        [Description("Sucesso")]
        Success = 1,
        [Description("Erro")]
        Error = 2,
    }
}
