using Domain.Base;

namespace Infrastructure.Repositories.Base
{
    public interface IRepositoryBase<T> : ISelectRepository<T> where T : class
    {
        /// <summary>
        /// Salva uma nova entidade.
        /// </summary>
        /// <param name="entity">A entidade a ser adicionada.</param>
        Task<int> Save(T entity);

        /// <summary>
        /// Salva uma nova entidade ou atualiza uma existente no contexto.
        /// </summary>
        /// <param name="entity">A entidade a ser adicionada ou atualizada.</param>
        Task<int> SaveOrUpdate(T entity);

        /// <summary>
        /// Salva uma lista de entidades.
        /// </summary>
        /// <param name="entities">As entidades a serem adicionadas.</param>
        Task<int> SaveList(List<T> entities);

        /// <summary>
        /// Marca a entidade como modificada no contexto.
        /// </summary>
        /// <param name="entity">A entidade a ser atualizada.</param>
        Task<int> Update(T entity);

        /// <summary>
        /// Atualiza apenas os campos da entidade especificada que foram alterados, 
        /// com base nas propriedades da entidade atualizada.
        /// </summary>
        Task<int> UpdateSomeFields(long id, object updatedFields);

        /// <summary>
        /// Remove uma entidade do contexto.
        /// </summary>
        /// <param name="entity">A entidade a ser removida.</param>
        Task<int> Delete(long id);

        /// <summary>
        /// Remove uma entidade de forma lógica.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> SoftDelete(IAuditableEntity entity);
    }
}