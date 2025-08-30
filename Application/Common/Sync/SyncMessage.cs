using System.ComponentModel.DataAnnotations;

namespace Application.Common.Sync
{
    /// <summary>
    /// Representa a mensagem de sincronização genérica
    /// </summary>
    public class SyncMessage<TModel>
        where TModel : class, IViewModelBase
    {
        [Required]
        public SyncInfo Info { get; set; }

        /// <summary>
        /// Payload com o modelo específico da feature
        /// </summary>
        [Required]
        public TModel Payload { get; set; } = default!;
    }
}
