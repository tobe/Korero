using System.Threading.Tasks;

namespace Korero.Data
{
    public interface IDbInitialize
    {
        Task Initialize();
    }
}
