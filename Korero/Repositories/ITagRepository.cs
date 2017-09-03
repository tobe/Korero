using Korero.Models;
using System.Collections.Generic;

namespace Korero.Repositories
{
    public interface ITagRepository
    {
        IEnumerable<Tag> GetTags();
    }
}