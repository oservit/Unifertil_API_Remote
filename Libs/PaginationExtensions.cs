using System.Linq;

namespace Libs
{
    public static class PaginationExtensions
    {
        /// <summary>
        /// Aplica a paginação a uma consulta IQueryable.
        /// </summary>
        /// <typeparam name="T">Tipo da entidade.</typeparam>
        /// <param name="query">Consulta a ser paginada.</param>
        /// <param name="pageIndex">Número da página. Se <c>null</c>, a paginação não é aplicada.</param>
        /// <param name="pageSize">Número de itens por página. Se <c>null</c>, a paginação não é aplicada.</param>
        /// <returns>Consulta IQueryable com paginação aplicada.</returns>
        public static IQueryable<T> ApplyPagination<T>(
            this IQueryable<T> query,
            int? pageIndex = null,
            int? pageSize = null)
        {
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                var validPageIndex = pageIndex.Value < 1 ? 1 : pageIndex.Value;
                var validPageSize = pageSize.Value < 1 ? 20 : pageSize.Value;
                query = query.Skip((validPageIndex - 1) * validPageSize).Take(validPageSize);
            }

            return query;
        }
    }
}
