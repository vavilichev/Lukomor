using System.Threading.Tasks;
using Lukomor.DI;

namespace Lukomor.Contexts
{
	public interface IContext
	{
		bool IsReady { get; }

		Task InitializeAsync(DIContainer container);
		void Destroy();
	}
}