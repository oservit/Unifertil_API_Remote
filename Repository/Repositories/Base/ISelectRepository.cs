using Libs;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Base
{
    public interface ISelectRepository<T> where T : class
    {
        /// <summary>
        /// Cria uma query dinamica.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> CreateQuery();

        /// <summary>
        /// Aplica filtros à consulta com base no <paramref name="filterRequest"/>.
        /// </summary>
        /// <param name="query">A consulta original à qual o filtro será aplicado.</param>
        /// <param name="filterRequest">O objeto que contém os critérios de filtro.</param>
        /// <returns>A consulta filtrada, ou a consulta original se <paramref name="filterRequest"/> for nulo.</returns>
        IQueryable<T> ApplyFilter(IQueryable<T> query, FilterRequest filterRequest);

        /// <summary>
        /// Aplica a ordenação à consulta com base no <paramref name="orderByRequest"/>.
        /// </summary>
        /// <param name="query">A consulta original à qual a ordenação será aplicada.</param>
        /// <param name="orderByRequest">O objeto que contém as informações de ordenação.</param>
        /// <returns>A consulta ordenada, ou a consulta original se <paramref name="orderByRequest"/> for nulo.</returns>
        IQueryable<T> ApplyOrdering(IQueryable<T> query, OrderByRequest orderByRequest);

        /// <summary>
        /// Busca um registro específico pelo Id informado de forma assíncrona.
        /// </summary>
        /// <param name="id">O ID do registro a ser buscado.</param>
        Task<T?> Get(long id);

        /// <summary>
        /// Busca um registro específico pelo Id informado de forma assíncrona.
        /// </summary>
        /// <param name="id">O ID do registro a ser buscado.</param>
        Task<T?> Get(string id);

        /// <summary>
        /// Busca um registro específico de acordo com a função de pesquisa de forma assíncrona.
        /// </summary>
        /// <param name="predicate">A expressão para filtrar o registro.</param>
        Task<T?> Get(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Retorna o valor de campos no select customizavel.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        Task<TResult?> GetField<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector);

        /// <summary>
        /// Obtém uma lista de entidades, aplicando um filtro opcional se fornecido de forma assíncrona.
        /// </summary>
        /// <returns>Uma lista de entidades, potencialmente filtrada.</returns>
        Task<IEnumerable<T>> GetList(FilterRequest? filterRequest = null);

        /// <summary>
        /// Retorna uma lista de registros de acordo com a função de pesquisa de forma assíncrona.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetList(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Retorna uma lista paginada de registros de acordo com a função de pesquisa de forma assíncrona.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetList(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize);

        /// <summary>
        /// Retorna uma lista paginada de registros de forma assíncrona.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetList(int pageIndex, int pageSize);

        /// <summary>
        /// Retorna os resultados paginados de forma assíncrona.
        /// </summary>
        Task<IEnumerable<T>> GetPagedResults(FilterRequest filter, OrderByRequest order, int pageIndex, int pageSize);

        /// <summary>
        /// Retorna o número total de registros de forma assíncrona.
        /// </summary>
        /// <returns></returns>
        Task<long> Count();

        /// <summary>
        /// Retorna o número total de registros de acordo com a função de pesquisa de forma assíncrona.
        /// </summary>
        /// <returns></returns>
        Task<long> Count(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Remove o objeto da memória.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Verifica se o registro existe no banco de dados.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Exists(long id);

        /// <summary>
        /// Verifica a existencia de um registro de acordo com um predicado.
        /// </summary>
        /// <param name="predicate">A expressão para filtrar o registro.</param>
        Task<bool> Exists(Expression<Func<T, bool>> predicate);

    }
}