using System;
using Korero.Models;
using System.Linq;
using System.Collections.Generic;
using System.Security.Principal;

namespace Korero.Repositories
{
    public interface IThreadRepository
    {
        (IEnumerable<Thread>, int) GetThreads(int? page);
    }
}