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
        Thread GetThread(int ThreadId);
        bool DeleteThread(int ThreadId, IIdentity CurrentUser);
        bool AddThread(Thread thread);
        bool UpdateThreadViews(Thread thread);
        List<Thread> GetThreadsByTag(Tag tag);
        bool DeleteMultipleThreads(List<Thread> ThreadList);
    }
}