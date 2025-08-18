
namespace Domain.Base
{
    public interface IEntityBase
    {
        /// <summary>
        /// Identificador do registro.
        /// </summary>
        long? Id { get; set; }
        object Clone();
    }
}
