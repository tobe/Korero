using Korero.Models;
using System.Collections.Generic;

namespace Korero.Repositories
{
    public interface IReplyRepository
    {
        (IEnumerable<Reply>, int) GetReplies(int threadId, int page);
        bool AddReply(int threadId, Reply reply);
        Reply GetReply(int replyId);
        bool UpdateReply(Reply reply);
    }
}