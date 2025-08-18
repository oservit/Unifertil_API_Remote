namespace Domain.Base
{
    public interface IExternalDataEntity : IEntityBase
    {
        /// <summary>
        /// Código ERP único para o registro.
        /// </summary>
        string? ErpCode { get; set; }
    }
}
