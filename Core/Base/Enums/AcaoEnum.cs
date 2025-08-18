using System.ComponentModel;

namespace Domain.Base.Enums
{
    public enum AcaoEnum
    {
        [Description("Inclusão")]
        Inclusao = 1,
        [Description("Alteração")]
        Alteracao = 2,
        [Description("Remoção")]
        Remocao = 3
    }
}
