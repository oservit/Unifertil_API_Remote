using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Sync
{
    public class SyncInfo
    {
        /// <summary>
        /// Quem enviou (ex.: Central, Remote1, etc.)
        /// </summary>
        public int SenderId { get; set; }

        /// <summary>
        /// Quem deve receber
        /// </summary>
        public int ReceiverId { get; set; }

        /// <summary>
        /// Código da Operação
        /// </summary>
        public int OperationId { get; set; }

        /// <summary>
        /// Código da Entidade/Tabela
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Hash do Registro
        /// </summary>
        public string Hash { get; set; } = string.Empty;

        /// <summary>
        /// Origem da chamada.
        /// </summary>
        public SyncCaller? Caller { get; set; } = SyncCaller.Trigger;
    }
}
