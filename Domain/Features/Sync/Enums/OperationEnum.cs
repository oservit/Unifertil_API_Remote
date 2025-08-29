using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Features.Sync.Enums
{
    public enum OperationEnum
    {
        [Description("Inclusão")]
        Insert = 1,
        [Description("Alteração")]
        Update = 2,
        [Description("Remoção")]
        Delete = 3
    }
}
