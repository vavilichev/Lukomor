using System.Threading.Tasks;

namespace Lukomor.Features.Data
{
    public interface IRepository
    {
        Task Save();
        Task Load();
    }
}