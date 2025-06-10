using Core.Database.Entities;
using Core.Infrastructure.Results;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text;

namespace Core.Database.Repositories
{
    /// <summary>
    /// The Base repository class
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public abstract class BaseRepository<TKey, TEntity, TDbContext> : IBaseRepository<TKey, TEntity>
        where TEntity : class, IEntity<TKey>
        where TDbContext : DbContext, new()
    {
        /// <summary>
        /// default .ctor for BaseRepository
        /// </summary>
        public BaseRepository(TDbContext context)
        {
            Context = context;
            Entities = Context.Set<TEntity>();
        }

        /// <summary>
        /// The database context
        /// </summary>
        protected DbContext Context { get; }

        /// <summary>
        /// The Entities db set
        /// </summary>
        protected DbSet<TEntity> Entities { get;}

        /// <inheritdoc/>
        public IDataResult<TEntity> Get(TKey id)
        {
            if(id == null || id.Equals(Guid.Empty)) throw new ArgumentException("The Id can not be empty guid or null.");

            var result = new DataResult<TEntity>();

            try
            {
                result.Data = Entities.FirstOrDefault(x=> x.Id.Equals(id));
            }
            catch (Exception ex)
            {
                result.AddError(ex);
            }

            return result;
        }

        /// <inheritdoc/>
        public virtual async Task<IDataResult<TEntity>> GetAsync(TKey id, CancellationToken ct = default)
        {
            if (id == null || id.Equals(Guid.Empty)) throw new ArgumentException("The Id can not be empty guid or null.");

            var result = new DataResult<TEntity>();

            try
            {
                result.Data = await Entities.FirstOrDefaultAsync(x=> x.Id.Equals(id), ct);
            }
            catch (Exception ex)
            {
                result.AddError(ex);
            }

            return result;
        }

        /// <inheritdoc/>
        public IDataResult<IEnumerable<TEntity>> GetAll(int offset = 0, int take = 0)
        {
            var result = new DataResult<IEnumerable<TEntity>>();

            try
            {
                var data = GetAllInternal().Skip(offset);

                if(take != 0)
                {
                    data = data.Take(take);
                }
                else
                {
                    data = data.Take(100);
                }

                result.Data = data.ToList();
            }
            catch(Exception ex)
            {
                result.AddError(ex);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<IDataResult<IEnumerable<TEntity>>> GetAllAsync(int? offset = null, int? take = null, CancellationToken ct = default)
        {
            var result = new DataResult<IEnumerable<TEntity>>();
            result.Data = new List<TEntity>();

            try
            {
                if (offset == null)
                {
                    offset = 0;
                }

                if (take == null)
                {
                    take = 100;
                }

                var data = GetAllInternal().Skip(offset.Value).Take(take.Value);

                result.Data = await data.ToListAsync(ct);
            }
            catch (Exception ex)
            {
                result.AddError(ex);
            }

            return result;
        }

        /// <inheritdoc/>
        public Task<IDataResult<TEntity>> GetAllAsQueryableAsync(CancellationToken ct = default)
        {
            var result = new DataResult<TEntity>();

            try
            {
                result.Query = GetAllInternal();
            }
            catch (Exception ex)
            {
                result.AddError(ex);
            }

            return Task.FromResult<IDataResult<TEntity>>(result);
        }

        /// <inheritdoc/>
        public IResult Insert(TEntity entity)
        {
            var result = new Result();

            try
            {
                Entities.Add(entity);
            }
            catch (Exception ex)
            {
                result.AddError(ex);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<IResult> InsertAsync(TEntity entity, CancellationToken ct = default)
        {
            var result = new Result();

            try
            {
                await Entities.AddAsync(entity, ct);
            }
            catch (Exception ex)
            {
                result.AddError(ex);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<IResult> InsertManyAsync(IList<TEntity> entities, CancellationToken ct = default)
        {
            var result = new Result();

            try
            {
                await Entities.AddRangeAsync(entities, ct);
            }
            catch (Exception ex)
            {
                result.AddError(ex);
            }

            return result;
        }

        /// <inheritdoc/>
        public IResult Update(TEntity entity)
        {
            var result = new Result();

            try
            {
                Entities.Attach(entity);
                Context.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                result.AddError(ex);
            }

            return result;
        }

        /// <inheritdoc/>
        public IResult Delete(TEntity entity)
        {
            var result = new Result();

            try
            {
                Entities.Remove(entity);
                Context.Entry(entity).State = EntityState.Deleted;
            }
            catch (Exception ex)
            {
                result.AddError(ex);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<IResult> DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            var result = new Result();

            try
            {
                var entity = await Entities.FirstOrDefaultAsync(predicate, ct);
                if(entity == null)
                {
                    return result;
                }

                Entities.Remove(entity);
                Context.Entry(entity).State = EntityState.Deleted;
            }
            catch (Exception ex)
            {
                result.AddError(ex);
            }

            return result;
        }

        /// <inheritdoc/>
        public IResult Save()
        {
            var result = new Result();

            try
            {
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                result.AddError(ex);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<IResult> SaveAsync(CancellationToken ct = default)
        {
            var result = new Result();

            try
            {
                await Context.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                result.AddError(ex);
            }

            return result;
        }

        /// <summary>
        /// Get all internally
        /// </summary>
        /// <returns></returns>
        private IQueryable<TEntity> GetAllInternal()
        {
            return Entities;
        }
    }
}
