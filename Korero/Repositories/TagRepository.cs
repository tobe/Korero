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
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Returns all tags ordered by their name
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tag> GetTags()
        {
            return this._context.Tag.OrderBy(t => t.Label);
        }

        /// <summary>
        /// Returns a single tag by its id
        /// </summary>
        /// <param name="id">The tag id</param>
        /// <returns></returns>
        public Tag GetTag(int id)
        {
            return this._context.Tag.Where(t => t.ID == id).SingleOrDefault();
        }

        /// <summary>
        /// Adds a new tag
        /// </summary>
        /// <param name="Tag">The tag to add</param>
        /// <returns></returns>
        public bool AddTag(Tag Tag)
        {
            // There is nothing specific to validate, ModelState in the controller checks for everything.
            try
            {
                this._context.Tag.Add(Tag);
                this._context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes a tag and all threads belonging to it.
        /// </summary>
        /// <param name="tag">The tag to delete</param>
        /// <returns></returns>
        public bool DeleteTag(Tag Tag)
        {
            try
            {
                this._context.Tag.Remove(Tag);
                this._context.SaveChanges();
                return true;
            }catch
            {
                return false;
            }
        }
    }
}