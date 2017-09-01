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
        /// Removes a thread specified by its id
        /// </summary>
        /// <param name="id">Thread id</param>
        /// <param name="currentUser">Currently logged in user</param>
        /// <returns>true on success, false on failure</returns>
        public bool DeleteThread(int id, IIdentity currentUser)
        {
            Thread thread = this._context.Thread.Include(t => t.Author)
                .SingleOrDefault(t => t.ID == id);
            if (thread == null)
                return false;

            // Check if the user trying to delete it is the author of the thread
            if (thread.Author.UserName != currentUser.Name)
                return false;

            this._context.Thread.Remove(thread);
            this._context.SaveChanges();
            return true;
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
                .SingleOrDefault();
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

        /// <summary>
        /// Adds a reply [reply] to thread specified by threadId
        /// </summary>
        /// <param name="threadId">Thread id</param>
        /// <param name="reply">The reply itself</param>
        /// <returns>true on success, false on failure</returns>
        public bool AddReply(int threadId, Reply reply)
        {
            try
            {
                this._context.Reply.Add(reply);
                this._context.SaveChanges();
                return true;
            }catch
            {
                return false;
            }
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
             * `Reply.ThreadID` = `Thread.ID` AND
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