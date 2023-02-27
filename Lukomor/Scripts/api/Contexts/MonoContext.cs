using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lukomor.Common.Utils.Async;
using Lukomor.DI;
using Lukomor.Features;
using UnityEngine;

namespace Lukomor.Contexts
{
    public abstract class MonoContext : MonoBehaviour, IContext
    {
        public bool IsReady { get; private set; }

        [SerializeField] private FeatureInstaller[] serviceFeaturesInstallers;
        [SerializeField] private FeatureInstaller[] gameplayFeatureInstallers;
        
		private List<IFeature> cachedServiceFeatures;
		private List<IFeature> cachedGameplayFeatures;

		#region Unity Lifecycle

		private void Awake()
		{
			cachedServiceFeatures = new List<IFeature>();
			cachedGameplayFeatures = new List<IFeature>();
		}
		
		private void OnDestroy()
		{
			Destroy();
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			foreach (IFeature serviceFeature in cachedServiceFeatures)
			{
				serviceFeature.OnApplicationFocus(hasFocus);
			}

			foreach (IFeature gameplayFeature in cachedGameplayFeatures)
			{
				gameplayFeature.OnApplicationFocus(hasFocus);
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			foreach (IFeature serviceFeature in cachedServiceFeatures)
			{
				serviceFeature.OnApplicationPause(pauseStatus);
			}

			foreach (IFeature gameplayFeature in cachedGameplayFeatures)
			{
				gameplayFeature.OnApplicationPause(pauseStatus);
			}
		}

		#endregion

		#region Lifecycle
		
		public virtual async Task InitializeAsync(DiContainer container)
		{
			InstallServiceFeatures(container);
			InstallGameplayFeatures(container);

			InitializeServiceFeatures();
			InitializeGameplayFeatures();

			await WaitInitializationComplete();
		}

		public void Destroy()
		{
			DestroyServiceFeatures();
			DestroyGameplayFeatures();
		}

		#endregion
		
		private void InstallServiceFeatures(DiContainer container)
		{
			foreach (var serviceFeatureInstaller in serviceFeaturesInstallers)
			{
				var createdFeature = serviceFeatureInstaller.Create(container);

				cachedServiceFeatures.Add(createdFeature);
			}
		}

		private void InstallGameplayFeatures(DiContainer container)
		{
			foreach (var gameplayFeatureInstaller in gameplayFeatureInstallers)
			{
				var createdFeature = gameplayFeatureInstaller.Create(container);

				cachedGameplayFeatures.Add(createdFeature);
			}
		}

		private void InitializeServiceFeatures()
		{
			foreach (var serviceFeature in cachedServiceFeatures)
			{
				serviceFeature.InitializeAsync().RunAsync();
			}
		}

		private void InitializeGameplayFeatures()
		{
			foreach (var gameplayFeature in cachedGameplayFeatures)
			{
				gameplayFeature.InitializeAsync().RunAsync();
			}
		}

		private async Task WaitInitializationComplete()
		{
			await UnityAwaiters.WaitUntil(() =>
				cachedGameplayFeatures.All(feature => feature.IsReady)
				&& cachedServiceFeatures.All(service => service.IsReady));

			IsReady = true;
		}

		private void DestroyServiceFeatures()
		{
			foreach (var serviceFeature in cachedServiceFeatures)
			{
				serviceFeature.DestroyAsync().RunAsync();
			}
			
			cachedServiceFeatures.Clear();
			
			foreach (var serviceFeaturesInstaller in serviceFeaturesInstallers)
			{
				serviceFeaturesInstaller.Dispose();
			}
		}

		private void DestroyGameplayFeatures()
		{
			foreach (var gameplayFeature in cachedGameplayFeatures)
			{
				gameplayFeature.DestroyAsync().RunAsync();
			}
			
			cachedGameplayFeatures.Clear();

			foreach (var featureInstaller in gameplayFeatureInstallers)
			{
				featureInstaller.Dispose();
			}
		}
    }
}