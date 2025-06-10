using Core.Database.Entities;
using Core.Infrastructure.Results;
using System.Linq.Expressions;

namespace Core.Database.Repositories
{
    /// <summary>
    /// The Base repository interface
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseRepository<TKey, TEntity>
       where TEntity : IEntity<TKey>
    {
        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <param name="offset">Skip number of records</param>
        /// <param name="take">Take specific amount of records</param>
        /// <returns></returns>
        IDataResult<IEnumerable<TEntity>> GetAll(int offset = 0, int take = 0);

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <param name="offset">Skip number of records</param>
        /// <param name="take">Take specific amount of records</param>
        /// <returns></returns>
        Task<IDataResult<IEnumerable<TEntity>>> GetAllAsync(int? offset = null, int? take = null, CancellationToken ct = default);

        /// <summary>
        /// Gets all entities query
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IDataResult<TEntity>> GetAllAsQueryableAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets the entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IDataResult<TEntity> Get(TKey id);

        /// <summary>
        /// Gets the entity by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<IDataResult<TEntity>> GetAsync(TKey id, CancellationToken ct = default);

        /// <summary>
        /// Inserts the entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        IResult Insert(TEntity entity);

        /// <summary>
        /// Inserts the entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<IResult> InsertAsync(TEntity entity, CancellationToken ct = default);

        /// <summary>
        /// Inserts colelction of entities
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IResult> InsertManyAsync(IList<TEntity> entities, CancellationToken ct = default);

        /// <summary>
        /// Updates the entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        IResult Update(TEntity entity);

        /// <summary>
        /// Deletes the entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        IResult Delete(TEntity entity);

        /// <summary>
        /// Deletes the entity
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IResult> DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);

        /// <summary>
        /// Save changes to database
        /// </summary>
        /// <returns></returns>
        IResult Save();

        /// <summary>
        /// Save changes to database
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IResult> SaveAsync(CancellationToken ct = default);
    }
}
