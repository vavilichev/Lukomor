using System.Threading.Tasks;

namespace Lukomor.Application.Services
{
	public abstract class Service : IService
	{
		public bool IsReady { get; private set; }

		public virtual void Dispose() { }

		public async Task Initialize()
		{
			if (!IsReady)
			{
				await InitializeAsyncInternal();

				IsReady = true;
			}
		}

		protected abstract Task InitializeAsyncInternal();
	}
}