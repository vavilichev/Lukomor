using System.Threading.Tasks;

namespace Lukomor.Data
{
    public interface IRepository
    {
        Task Save();
        Task Load();
    }
}