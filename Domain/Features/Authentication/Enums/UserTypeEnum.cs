using System.ComponentModel;

namespace Domain.Features.Authentication.Enums
{
    public enum UserTypeEnum
    {
        /// <summary>
        /// Administrador da Central
        /// </summary>
        [Description("SystemAdmin")]
        SystemAdmin = 1,

        /// <summary>
        /// Comunicação Central <-> Remotes
        /// </summary>
        [Description("SyncAPI")]
        SyncAPI = 2,

        /// <summary>
        /// Integrações de terceiros
        /// </summary>
        [Description("ExternalAPI")]
        ExternalAPI = 3,

        /// <summary>
        /// Usuários humanos
        /// </summary>
        [Description("HumanUser")]
        HumanUser = 4
    }
}
