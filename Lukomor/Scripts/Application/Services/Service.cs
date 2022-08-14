using System.Threading.Tasks;

namespace Lukomor.Application.Services
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
		
		protected abstract Task InitializeAsyncInternal();
	}
}