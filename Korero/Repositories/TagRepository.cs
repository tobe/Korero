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

        public IEnumerable<Tag> GetTags()
        {
            return this._context.Tag;
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
    }
}