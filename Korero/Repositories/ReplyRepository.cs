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
    public class ReplyRepository : IReplyRepository
    {
        private readonly ApplicationDbContext _context;

        public ReplyRepository(ApplicationDbContext context)
        {
            this._context = context;
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
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a single reply by replyId
        /// </summary>
        /// <param name="replyId">The ID of the reply</param>
        /// <returns></returns>
        public Reply GetReply(int replyId)
        {
            return this._context.Reply.Where(r => r.ID == replyId)
                .Include(r => r.Author)
                .Include(r => r.Thread)
                .SingleOrDefault();
        }

        /// <summary>
        /// Updates a reply [reply].
        /// This is kind of "magical" so to speak of ASP.NET.
        /// Minimizes the code by automatically grabbing the needed things from the 
        /// Reply model itself. Neat.
        /// </summary>
        /// <param name="reply">The reply model to update</param>
        /// <returns></returns>
        public bool UpdateReply(Reply reply)
        {
            try
            {
                this._context.Reply.Update(reply);
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