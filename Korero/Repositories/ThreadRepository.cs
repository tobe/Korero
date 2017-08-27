using System;
using System.Linq;
using Korero.Models;
using Korero.Data;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Korero.Services;

namespace Korero.Repositories
{
    public class ThreadRepository : IThreadRepository
    {
        private readonly ApplicationDbContext _context;

        public ThreadRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public (IEnumerable<Thread>, int) GetThreads(int? page)
        {
            IQueryable<Thread> query = this._context.Thread.Include(t => t.Tag).Include(t => t.Replies);

            var paginatedData = query.Paginate(
                new PaginationInfo { PageNumber = page ?? 1, PageSize = 1 }
            ).ToList();

            return (paginatedData, query.Count());
        }
    }
}