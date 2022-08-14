using System.Threading.Tasks;

namespace Lukomor.Application.Features
{
	public interface IFeature
	{
		bool IsReady { get; }

		Task InitializeAsync();
		Task DestroyAsync();
	}
}