using System;
using System.Linq;
using Korero.Models;
using Korero.Data;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Korero.Services;
using System.Security.Principal;

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
        /// Returns all threads
        /// </summary>
        /// <param name="page">The page the user is at (pagination purposes)</param>
        /// <returns></returns>
        public (IEnumerable<Thread>, int) GetThreads(int? page)
        {
            /* Something like:
             SELECT * FROM `Thread`, `Reply`, `AspNetUsers`, `Tag`
             WHERE `Thread.ID`=`Reply.ThreadID` AND
             WHERE `Tag.ID`=`Thread.TagID` AND
             WHERE `Thread.AuthorID`=`AspNetUsers.Id`
             ORDER BY `Reply.DateCreated` DESC,
                      `Thread.DateCreated` DESC
             */
            IQueryable<Thread> query = this._context.Thread.Include(t => t.Tag)
                .Include(r => r.Replies)
                .Include(t => t.Author)
                .OrderByDescending(t => t.DateCreated);
            // ^ Is sorted by the Thread's creation date. Now sort the replies.

            var paginatedData = query.Paginate(
                new PaginationInfo { PageNumber = page ?? 1, PageSize = 4 }
            ).ToList();

            // tfw u can't .Include(r => r.Replies.OrderBy())... FeelsBadMan
            // https://stackoverflow.com/questions/8447384/how-to-order-child-collections-of-entities-in-ef?rq=1
            foreach (var q in paginatedData)
            {
                q.Replies = q.Replies.OrderBy(x => x.DateCreated).ToList();
            }

            return (paginatedData, query.Count());
        }
    }
}