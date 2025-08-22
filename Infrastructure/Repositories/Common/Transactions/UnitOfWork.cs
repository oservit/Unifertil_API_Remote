using Microsoft.EntityFrameworkCore.Storage;
using Infrastructure.Data.Oracle;

namespace Infrastructure.Repositories.Base.Transactions
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OracleDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(OracleDbContext context)
        {
            _context = context;
        }
        public OracleDbContext Context => _context;

        public void BeginTransaction()
        {
            if (_transaction != null)
                throw new InvalidOperationException("Uma transação já está em andamento.");

            _transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Nenhuma transação em andamento.");

            _context.SaveChanges();

            _transaction.Commit();
            _transaction = null;
        }

        public void Rollback()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Nenhuma transação em andamento.");

            _transaction.Rollback();
            _transaction = null;
        }

        public int SaveChanges()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Nenhuma transação em andamento.");

            return _context.SaveChanges();
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
            }
        }
    }
}
