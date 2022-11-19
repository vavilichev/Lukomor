using System.Threading.Tasks;

namespace Lukomor.Domain.Features
{
	public abstract class Feature : IFeature
	{
		public bool IsReady { get; private set; }
		public async Task InitializeAsync()
		{
			if (!IsReady)
			{
				await InitializeInternal();

				IsReady = true;
			}
		}

		public virtual Task DestroyAsync()
		{
			return Task.CompletedTask;
		}

		protected virtual Task InitializeInternal() { return Task.CompletedTask; }
	}
}