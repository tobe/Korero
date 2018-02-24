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
            /* Ok, so, the issue is following: Return all the threads and their respective info
            // such as Tag, Author and sort them by DateCreated. However, also do return a SINGLE,
            // first reply of said thread, ordered by its DateCreated. To accomplish this **Explicit** loading
            // must be used...
            // @see https://stackoverflow.com/questions/34001556/how-to-get-only-the-first-element-with-include
            // "Even with lazy loading disabled it is still possible to lazily load related entities,
            // but it must be done with an explicit call.
            // To do so you use the Load method on the related entity’s entry"

            var query = this._context.Thread.Include(t => t.Tag)
                .Include(t => t.Author)
                //.Include(r => r.Replies)
                .OrderByDescending(t => t.DateCreated);

            foreach(var q in query)
            {
                // Foreach fetched thread additionally include a single reply ordered by the date (first)
                this._context.Entry(q)
                .Collection(r => r.Replies)
                .Query()
                .OrderBy(r => r.DateCreated).Take(1)
                .Load();
            }*/

            var query = this._context.Thread.Include(t => t.Tag)
                .Include(t => t.Author)
                .Include(r => r.Replies)
                .OrderByDescending(t => t.DateCreated);

            var paginatedData = query.Paginate(
                new PaginationInfo { PageNumber = page ?? 1, PageSize = 4 }
            ).ToList();

            // tfw u can't .Include(r => r.Replies.OrderBy())... FeelsBadMan
            // https://stackoverflow.com/questions/8447384/how-to-order-child-collections-of-entities-in-ef?rq=1
            foreach (var q in paginatedData)
            {
                q.Replies = q.Replies.OrderBy(x => x.DateCreated).ToList();
            }
            // Note to self: idk why I was returning all replies when I only need the first one... sigh

            return (paginatedData, query.Count());
        }

        /// <summary>
        /// Returns a single thread
        /// </summary>
        /// <param name="ThreadId">The ID of the thread to return</param>
        /// <returns></returns>
        public Thread GetThread(int ThreadId)
        {
            return this._context.Thread.Where(t => t.ID == ThreadId)
                .Include(t => t.Tag)
                .Include(t => t.Author)
                .SingleOrDefault();
        }

        /// <summary>
        /// Adds a new thread 
        /// </summary>
        /// <param name="thread">The thread to add</param>
        /// <returns></returns>
        public bool AddThread(Thread thread)
        {
            // Just add it. All the verification is done beforehand
            try
            {
                this._context.Thread.Add(thread);
                this._context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Removes a thread specified by its id
        /// </summary>
        /// <param name="id">Thread id</param>
        /// <param name="currentUser">Currently logged in user</param>
        /// <returns>true on success, false on failure</returns>
        public bool DeleteThread(int ThreadId, IIdentity CurrentUser)
        {
            Thread thread = this.GetThread(ThreadId);
            if (thread == null)
                return false;

            // Check if the user trying to delete it is the author of the thread
            if (thread.Author.UserName != CurrentUser.Name)
                return false;

            this._context.Thread.Remove(thread);
            this._context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Updates the thread's view count
        /// </summary>
        /// <param name="thread">The thread view count to update</param>
        public bool UpdateThreadViews(Thread thread)
        {
            // The thread has been passed on, bump the view count and let the magic happen...
            thread.Views++;

            try
            {
                this._context.Thread.Update(thread);
                this._context.SaveChanges();

                return true;
            }catch
            {
                return false;
            }
        }
    }
}