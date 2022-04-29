using System.Linq;
using System.Threading.Tasks;
using Lukomor.Application.Features;
using Lukomor.Application.Services;
using Lukomor.Application.Signals;
using Lukomor.DIContainer;
using VavilichevGD.Tools.Async;

namespace Lukomor.Application.Contexts
{
	public abstract class ContextBase : IContext
	{
		public bool IsReady { get; private set; }

		public virtual async Task Initialize() {
			DI.Bind<ISignalTower>(new SignalTower());
			
			InstallServices();
			InstallFeatures();
			
			InitializeServices();
			InitializeFeatures();

			await WaitInitializationComplete();
		}

		protected abstract void InstallServices();

		protected abstract void InstallFeatures();

		private void InitializeServices() {
			var allServices = DI.GetAll<IService>();
			var count = allServices.Length;

			for (int i = 0; i < count; i++) {
				allServices[i].Initialize().RunAsync();
			}
		}

		private void InitializeFeatures()
		{
			var allFeatures = DI.GetAll<IFeature>();
			var count = allFeatures.Length;

			for (int i = 0; i < count; i++) {
				allFeatures[i].Initialize().RunAsync();
			}
		}

		private async Task WaitInitializationComplete() {
			var allServices = DI.GetAll<IService>();
			var allFeatures = DI.GetAll<IFeature>();

			await UnityAwaiters.WaitUntil(() =>
				allFeatures.All(feature => feature.IsReady)
				&& allServices.All(service => service.IsReady));

			IsReady = true;
		}
	}
}