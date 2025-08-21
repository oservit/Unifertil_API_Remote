namespace Application.Common.Sync
{
    /// <summary>
    /// Representa a mensagem de sincronização genérica
    /// </summary>
    public class SyncMessage<TModel>
        where TModel : class, IViewModelBase
    {
        /// <summary>
        /// Quem enviou (ex.: Central, Remote1, etc.)
        /// </summary>
        public int SenderId { get; set; } = default!;

        /// <summary>
        /// Quem deve receber
        /// </summary>
        public int ReceiverId { get; set; } = default!;

        /// <summary>
        /// Payload com o modelo específico da feature
        /// </summary>
        public TModel Payload { get; set; } = default!;
    }
}
