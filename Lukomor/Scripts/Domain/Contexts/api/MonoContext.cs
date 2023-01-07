using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lukomor.Common.Utils.Async;
using Lukomor.Domain.Features;
using UnityEngine;

namespace Lukomor.Domain.Contexts
{
    public abstract class MonoContext : MonoBehaviour, IContext
    {
        public bool IsReady { get; private set; }

        [SerializeField] private FeatureInstaller[] _serviceFeaturesInstallers;
        [SerializeField] private FeatureInstaller[] _gameplayFeatureInstallers;
        
		private List<IFeature> _cachedServiceFeatures;
		private List<IFeature> _cachedGameplayFeatures;

		#region Unity Lifecycle

		private void Awake()
		{
			_cachedServiceFeatures = new List<IFeature>();
			_cachedGameplayFeatures = new List<IFeature>();
		}
		
		private void OnDestroy()
		{
			Destroy();
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			foreach (IFeature serviceFeature in _cachedServiceFeatures)
			{
				serviceFeature.OnApplicationFocus(hasFocus);
			}

			foreach (IFeature gameplayFeature in _cachedGameplayFeatures)
			{
				gameplayFeature.OnApplicationFocus(hasFocus);
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			foreach (IFeature serviceFeature in _cachedServiceFeatures)
			{
				serviceFeature.OnApplicationPause(pauseStatus);
			}

			foreach (IFeature gameplayFeature in _cachedGameplayFeatures)
			{
				gameplayFeature.OnApplicationPause(pauseStatus);
			}
		}

		#endregion

		#region Lifecycle
		
		public virtual async Task InitializeAsync()
		{
			InstallServiceFeatures();
			InstallGameplayFeatures();

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
		
		private void InstallServiceFeatures()
		{
			foreach (var serviceFeatureInstaller in _serviceFeaturesInstallers)
			{
				var createdFeature = serviceFeatureInstaller.Create();

				_cachedServiceFeatures.Add(createdFeature);
			}
		}

		private void InstallGameplayFeatures()
		{
			foreach (var gameplayFeatureInstaller in _gameplayFeatureInstallers)
			{
				var createdFeature = gameplayFeatureInstaller.Create();

				_cachedGameplayFeatures.Add(createdFeature);
			}
		}

		private void InitializeServiceFeatures()
		{
			foreach (var serviceFeature in _cachedServiceFeatures)
			{
				serviceFeature.InitializeAsync().RunAsync();
			}
		}

		private void InitializeGameplayFeatures()
		{
			foreach (var gameplayFeature in _cachedGameplayFeatures)
			{
				gameplayFeature.InitializeAsync().RunAsync();
			}
		}

		private async Task WaitInitializationComplete()
		{
			await UnityAwaiters.WaitUntil(() =>
				_cachedGameplayFeatures.All(feature => feature.IsReady)
				&& _cachedServiceFeatures.All(service => service.IsReady));

			IsReady = true;
		}

		private void DestroyServiceFeatures()
		{
			foreach (var serviceFeature in _cachedServiceFeatures)
			{
				serviceFeature.DestroyAsync().RunAsync();
			}
			
			_cachedServiceFeatures.Clear();
			
			foreach (var serviceFeaturesInstaller in _serviceFeaturesInstallers)
			{
				serviceFeaturesInstaller.Dispose();
			}
		}

		private void DestroyGameplayFeatures()
		{
			foreach (var gameplayFeature in _cachedGameplayFeatures)
			{
				gameplayFeature.DestroyAsync().RunAsync();
			}
			
			_cachedGameplayFeatures.Clear();

			foreach (var featureInstaller in _gameplayFeatureInstallers)
			{
				featureInstaller.Dispose();
			}
		}
    }
}