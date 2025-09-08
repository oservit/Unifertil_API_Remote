using System.ComponentModel;

namespace Domain.Features.Sync.Enums
{
    public enum NodeTypeEnum
    {
        /// <summary>
        /// Central
        /// </summary>
        [Description("Central")]
        Central = 1,
        /// <summary>
        /// Remote
        /// </summary>
        [Description("Remote")]
        Remote = 2
    }
}
