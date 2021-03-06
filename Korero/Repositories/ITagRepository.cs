using Korero.Models;
using System.Collections.Generic;

namespace Korero.Repositories
{
    public interface ITagRepository
    {
        IEnumerable<Tag> GetTags();
        Tag GetTag(int id);
        bool AddTag(Tag tag);
        bool DeleteTag(Tag tag);
    }
}