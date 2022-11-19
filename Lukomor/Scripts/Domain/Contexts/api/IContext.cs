using System.Threading.Tasks;

namespace Lukomor.Domain.Contexts
{
	public interface IContext
	{
		bool IsReady { get; }

		Task InitializeAsync();
		void Destroy();
	}
}