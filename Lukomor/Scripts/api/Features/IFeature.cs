using System.Threading.Tasks;

namespace Lukomor.Features
{
	public interface IFeature
	{
		bool IsReady { get; }
		
		Task InitializeAsync();
		Task DestroyAsync();

		void OnApplicationFocus(bool hasFocus);
		void OnApplicationPause(bool pauseStatus);
	}
}