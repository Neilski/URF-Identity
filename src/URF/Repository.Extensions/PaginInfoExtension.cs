using Abl.Data;
using System.Linq;


namespace Repository.Extensions
{
    public static class PaginInfoExtension
    {
        public static IQueryable<T> GetPage<T>(
            this IQueryable<T> source,
            PagingInfo pagingInfo)
        {
            pagingInfo.TotalItems = source.Count();
            if (pagingInfo.PageNo > pagingInfo.TotalPages)
            {
                pagingInfo.PageNo = pagingInfo.TotalPages;
            }

            var offset = (pagingInfo.PageNo - 1) * pagingInfo.PageSize;
            var sortExpressions = pagingInfo.GetSortExpressions();

            if (sortExpressions.Count <= 0)
            {
                return source.Skip(offset).Take(pagingInfo.PageSize);
            }

            var sortedSource = source.Sort(pagingInfo.GetSortExpressions(), null);
            return sortedSource.Skip(offset).Take(pagingInfo.PageSize);
        }
    }
}