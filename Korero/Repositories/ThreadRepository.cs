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

        /// <summary>
        /// Returns a single thread
        /// </summary>
        /// <returns></returns>
        public Thread GetThread(int id)
        {
            return this._context.Thread.Where(t => t.ID == id)
                .Include(t => t.Tag)
                .Include(t => t.Author)
                .FirstOrDefault();
        }

        /// <summary>
        /// Returns all threads
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public (IEnumerable<Thread>, int) GetThreads(int? page)
        {
            IQueryable<Thread> query = this._context.Thread.Include(t => t.Tag)
                .Include(t => t.Replies)
                .Include(t => t.Author);

            var paginatedData = query.Paginate(
                new PaginationInfo { PageNumber = page ?? 1, PageSize = 4 }
            ).ToList();

            return (paginatedData, query.Count());
        }
    }
}