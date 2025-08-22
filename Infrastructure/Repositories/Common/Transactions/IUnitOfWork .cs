using Infrastructure.Data.Oracle;

namespace Infrastructure.Repositories.Base.Transactions
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Inicia uma nova transação.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Comita a transação atual.
        /// </summary>
        void Commit();

        /// <summary>
        /// Reverte a transação atual.
        /// </summary>
        void Rollback();

        /// <summary>
        /// Salva as alterações no contexto.
        /// </summary>
        int SaveChanges();

        /// <summary>
        /// Obtém o contexto do banco de dados.
        /// </summary>
        OracleDbContext Context { get; }
    }

}
