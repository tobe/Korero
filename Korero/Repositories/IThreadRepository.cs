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
        Thread GetThread(int id);
        (IEnumerable<Reply>, int) GetReplies(int threadId, int page);
        bool DeleteThread(int id, IIdentity currentUser);
    }
}