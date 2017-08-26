using System.Linq;

namespace Korero.Services
{
    public class PaginationInfo
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }

    public static class Pagination
    {
        public static IQueryable<T> Paginate<T>(
            this IQueryable<T> source,
            PaginationInfo pagination)
        {
            return source.Skip((pagination.PageNumber - 1) * pagination.PageSize)
                         .Take(pagination.PageSize);
        }
    }
}