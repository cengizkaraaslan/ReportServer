using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;


namespace Core.Extensions
{
   public static class DataTableExtensions
    {
        public async static Task<ListDto> ToPaginatedResult(this IQueryable<object> query, int skip, int take, string orderby)
        {
            var size = query.Count();
            if (orderby != null && !string.IsNullOrEmpty(orderby) && orderby != null)
            {

                query = query.OrderBy($"{orderby}");
            }
            var items = query
                .Skip(skip)
                .Take(take)
                .ToList();


            return await Task.FromResult(new ListDto
            {
                ModelList = items,
                Toplam = size
            });
        }
        public async static Task<ListDto<T>> ToPaginatedResult<T>(this IQueryable<T> query, int skip, int take, string orderby)
        {
            var size = query.Count();
           

            if (orderby != null && !string.IsNullOrEmpty(orderby) && orderby != null)
            {

                query = query.OrderBy($"{orderby}");
            }
           
            var items = query
                   .Skip(skip)
                   .Take(take)
                   .ToList();

            return await Task.FromResult(new ListDto<T>
            {
                ModelList = items,
                Toplam = size
            });

        }
    }
}
