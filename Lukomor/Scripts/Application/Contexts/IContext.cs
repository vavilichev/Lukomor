using System.Threading.Tasks;

namespace Lukomor.Application.Contexts
{
	public interface IContext
	{
		bool IsReady { get; }

		Task InitializeAsync();
		void ForceDestroy();
	}
}