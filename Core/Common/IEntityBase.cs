
namespace Domain.Common
{
    public interface IEntityBase
    {
        /// <summary>
        /// Identificador do registro.
        /// </summary>
        public long? Id { get; set; }
        public object Clone();
    }
}
