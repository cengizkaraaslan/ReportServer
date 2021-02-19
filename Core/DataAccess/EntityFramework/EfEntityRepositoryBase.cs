using Core.Entities;
using System.Web;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Concrete;
using Core.Extensions;
using System.Xml.Schema;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
    where TEntity : class, IEntity, new()

    {
        protected readonly DbContext context;
        public EfEntityRepositoryBase(DbContext Context)
        {
            context = Context;
        }
        public async Task<Guid> Add(TEntity entity)
        {


            entity.Id = entity.Id == Guid.Empty ? new Guid() : entity.Id;
            entity.Durum = "E";
            entity.EklemeTarihi = DateTime.Now;
            entity.EkleyenKullanici = Guid.Parse(new HttpContext().GetUserId());
            await context.Set<TEntity>().AddAsync(entity);
            return entity.Id;

        }

        public async Task AddList(IEnumerable<TEntity> entities)
        {
            Parallel.ForEach(entities, entity =>
            {
                entity.Id = entity.Id == Guid.Empty ? new Guid() : entity.Id;
                entity.Durum = "E";
                entity.EklemeTarihi = DateTime.Now;
                entity.EkleyenKullanici = Guid.Parse(new HttpContext().GetUserId());
            });
            await context.Set<TEntity>().AddRangeAsync(entities);
        }

        public async Task<Guid> AddReturnId(TEntity entity)
        {

            entity.Id = entity.Id == Guid.Empty ? new Guid() : entity.Id;
            entity.Durum = "E";
            entity.EklemeTarihi = DateTime.Now;
            entity.EkleyenKullanici = Guid.Parse(new HttpContext().GetUserId());
            await context.Set<TEntity>().AddAsync(entity);
            return entity.Id;
        }


        public async Task Delete(Guid id)
        {
            var entity = await context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);
            context.Set<TEntity>().Remove(entity);
        }

        public async Task DeleteAll(Expression<Func<TEntity, bool>> filter)
        {
            var get = await GetList(filter);
            if (get.Count() > 0)
            {
                context.Set<TEntity>().RemoveRange(get);
            }
            
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            return await context.Set<TEntity>().Where(x => x.Durum != "S").SingleOrDefaultAsync(filter);

        }

        public async Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> filter = null)
        {


            return filter == null
                ? await context.Set<TEntity>().Where(x => x.Durum != "S").ToListAsync()
                : await context.Set<TEntity>().Where(x => x.Durum != "S").Where(filter).ToListAsync();

        }
       
        public async Task Update(TEntity entity)
        {
            entity.Durum = "G";
            entity.EklemeTarihi = DateTime.Now;
            context.Entry(entity).State = EntityState.Modified;
            await Task.FromResult(context.Set<TEntity>().Update(entity));


        }
        public async Task<ListDto<TEntity>> GetPaginatedList(Expression<Func<TEntity, bool>> filter, int skip, int take, string orderby)
        {
            var query = filter == null ? context.Set<TEntity>().Where(x => x.Durum != "S") : context.Set<TEntity>().Where(x => x.Durum != "S").Where(filter);
            return await query.ToPaginatedResult(skip, take, orderby);
        }
        public async Task<List<TEntity>> GetListWithoutTracking(Expression<Func<TEntity, bool>> filter = null)
        {

            return filter == null
            ? await context.Set<TEntity>().AsNoTracking().Where(x => x.Durum != "S").ToListAsync()
              : await context.Set<TEntity>().AsNoTracking().Where(x => x.Durum != "S").Where(filter).ToListAsync();

        }
    }
}
