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

		public virtual void OnApplicationFocus(bool hasFocus) { }
		public virtual void OnApplicationPause(bool pauseStatus) { }


		protected virtual Task InitializeInternal() { return Task.CompletedTask; }
	}
}