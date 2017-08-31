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
        /// <param name="page">The page the user is at (pagination purposes)</param>
        /// <returns></returns>
        public (IEnumerable<Thread>, int) GetThreads(int? page)
        {
            IQueryable<Thread> query = this._context.Thread.Include(t => t.Tag)
                .Include(t => t.Replies)
                .Include(t => t.Author)
                .OrderByDescending(t => t.DateCreated);

            var paginatedData = query.Paginate(
                new PaginationInfo { PageNumber = page ?? 1, PageSize = 4 }
            ).ToList();

            return (paginatedData, query.Count());
        }

        /// <summary>
        /// Returns all replies from a thread specified by the threadId
        /// </summary>
        /// <param name="threadId">The ID of the thread itself</param>
        /// <param name="page">The page the user is at (pagination purposes)</param>
        /// <returns></returns>
        public (IEnumerable<Reply>, int) GetReplies(int threadId, int page)
        {
            /* Something like...
             * SELECT * FROM `Reply`, `AspNetUsers`, `Thread`
             * WHERE `Thread.ID` = threadId AND
             * `Reply.ThreadID` = `Thread.ThreadID` AND
             * `Reply.AuthorID` = `AspNetUsers.Id`
             * ORDER BY `Thread.dateCreated` DESC
             * :)
             */
            /*IQueryable<Reply> query = this._context.Thread.Where(t => t.ID == threadId)
                .SelectMany(
                    r => r.Replies
                    .OrderByDescending(f => f.DateCreated)
                );*/
            IQueryable<Reply> query = this._context.Reply.Where(r => r.Thread.ID == threadId)
                .OrderBy(f => f.DateCreated)
                .Include(r => r.Author);

            var paginatedData = query.Paginate(
                new PaginationInfo { PageNumber = page, PageSize = 5 }
            ).ToList();

            return (paginatedData, query.Count());
        }
    }
}