using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Common
{
    public abstract class EntityBase : IEntityBase, IDisposable, ICloneable
    {
        [JsonIgnore]
        private protected bool disposed;

        [Key]
        [Column("ID")]
        public virtual long? Id { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is EntityBase outraEntidade)
                return Id != 0 && Id == outraEntidade.Id;
            return false;
        }
        public override int GetHashCode() => Id.GetHashCode();
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
