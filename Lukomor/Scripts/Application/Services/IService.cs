using System.Threading.Tasks;

namespace Lukomor.Application.Services
{
	public interface IService
	{
		bool IsReady { get; }

		Task InitializeAsync();
		Task DestroyAsync();
	}
}