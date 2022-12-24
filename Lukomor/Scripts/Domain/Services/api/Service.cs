using System.Threading.Tasks;

namespace Lukomor.Domain.Services
{
	public abstract class Service : IService
	{
		public bool IsReady { get; private set; }

		public async Task InitializeAsync()
		{
			if (!IsReady)
			{
				await InitializeAsyncInternal();

				IsReady = true;
			}
		}
		
		public virtual Task DestroyAsync()
		{
			return Task.CompletedTask;
		}
		
		public virtual void OnApplicationFocus(bool hasFocus) { }
		public virtual void OnApplicationPause(bool pauseStatus) { }
		
		protected abstract Task InitializeAsyncInternal();
	}
}