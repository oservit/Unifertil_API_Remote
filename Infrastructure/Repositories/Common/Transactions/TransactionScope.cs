using System;

namespace Infrastructure.Repositories.Base.Transactions
{
    /// <summary>
    /// Classe para gerenciar o escopo de uma transação utilizando UnitOfWork.
    /// </summary>
     public class TransactionScope : IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;
        private bool _disposed;
        private bool _committed;

        public TransactionScope(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork), "UnitOfWork não pode ser nulo.");
            _unitOfWork.BeginTransaction(); // Inicia a transação
        }

        public void Commit()
        {
            _unitOfWork.SaveChanges();
            if (_committed) throw new InvalidOperationException("A transação já foi confirmada.");
            _unitOfWork.Commit();
            _committed = true; // Marca como confirmada
        }

        public void Rollback()
        {
            if (_committed) throw new InvalidOperationException("A transação já foi confirmada e não pode ser revertida.");
            _unitOfWork.Rollback();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (!_committed) // Se não foi confirmado, faz rollback
                    _unitOfWork.Rollback();
                _disposed = true;
            }
        }
    }
}
