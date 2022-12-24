using System.Threading.Tasks;

namespace Lukomor.Domain.Services
{
	public interface IService
	{
		bool IsReady { get; }

		Task InitializeAsync();
		Task DestroyAsync();
		
		void OnApplicationFocus(bool hasFocus);
		void OnApplicationPause(bool pauseStatus);
	}
}