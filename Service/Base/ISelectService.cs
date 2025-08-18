using Libs;
using System.Linq.Expressions;

namespace Service.Base
{
    public interface ISelectService<T> where T : class
    {
        /// <summary>
        /// Executa uma consulta dinâmica e retorna os resultados projetados para o tipo especificado.
        /// </summary>
        Task<IEnumerable<dynamic>> DbQuery(
            Func<IQueryable<T>, IQueryable<dynamic>> projection,
            bool useDistinct = false,
            FilterRequest? filter = null,
            OrderByRequest? orderBy = null,
            int? pageIndex = null,
            int? pageSize = null,
            params Func<IQueryable<T>, IQueryable<T>>[] includes);

        /// <summary>
        /// Obtém uma entidade projetada de acordo com a projeção fornecida.
        /// </summary>
        Task<dynamic?> Get(
            Func<IQueryable<T>, IQueryable<dynamic>> projection,
            Expression<Func<T, bool>>? filter = null);

        /// <summary>
        /// Obtém uma lista de entidades projetadas de acordo com a projeção fornecida.
        /// </summary>
        Task<IEnumerable<dynamic>> GetList(
            Func<IQueryable<T>, IQueryable<dynamic>> projection,
            Expression<Func<T, bool>>? filter = null);

        /// <summary>
        /// Busca um registro específico pelo Id informado.
        /// </summary>
        Task<T?> Get(string id);

        /// <summary>
        /// Busca um registro específico pelo Id informado.
        /// </summary>
        Task<T?> Get(long id);

        /// <summary>
        /// Busca um registro específico de acordo com a função de pesquisa.
        /// </summary>
        Task<T?> Get(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Obtém uma lista de entidades, aplicando um filtro opcional se fornecido.
        /// </summary>
        Task<IEnumerable<T>> GetList(FilterRequest? filterRequest = null);

        /// <summary>
        /// Retorna uma lista de registros de acordo com a função de pesquisa.
        /// </summary>
        Task<IEnumerable<T>> GetList(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Retorna uma lista paginada de registros.
        /// </summary>
        Task<IEnumerable<T>> GetList(int pageIndex, int pageSize);

        /// <summary>
        /// Retorna uma lista paginada de registros de acordo com a função de pesquisa.
        /// </summary>
        Task<IEnumerable<T>> GetList(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize);

        /// <summary>
        /// Retorna o número total de registros.
        /// </summary>
        Task<long> Count();

        /// <summary>
        /// Retorna o número total de registros de acordo com a função de pesquisa.
        /// </summary>
        Task<long> Count(Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> GetPagedResults(FilterRequest filter, OrderByRequest order, int pageIndex, int pageSize);

        /// <summary>
        /// Verifica se o registro existe no banco de dados.
        /// </summary>
        bool Exists(long id);

        /// <summary>
        /// Remove o objeto da memória.
        /// </summary>
        void Dispose();
    }
}