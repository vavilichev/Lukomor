using System.Threading.Tasks;

namespace Lukomor.Application.Features
{
	public abstract class Feature : IFeature
	{
		public bool IsReady { get; private set; }
		public async Task Initialize()
		{
			if (!IsReady)
			{
				await InitializeInternal();

				IsReady = true;
			}
		}

		public virtual void Dispose() { }

		protected virtual Task InitializeInternal() { return Task.CompletedTask; }
	}
}