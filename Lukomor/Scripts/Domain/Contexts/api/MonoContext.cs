using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lukomor.Common.DIContainer;
using Lukomor.Common.Utils.Async;
using Lukomor.Domain.Features;
using Lukomor.Domain.Services;
using UnityEngine;

namespace Lukomor.Domain.Contexts
{
    public abstract class MonoContext : MonoBehaviour, IContext
    {
        public bool IsReady { get; private set; }
		
		private Dictionary<Type, IService> _servicesMap;
		private Dictionary<Type, IFeature> _featuresMap;

		private void Awake()
		{
			_servicesMap = new Dictionary<Type, IService>();
			_featuresMap = new Dictionary<Type, IFeature>();
		}
		
		private void OnDestroy()
		{
			Destroy();
		}

		public virtual async Task InitializeAsync()
		{
			InstallServices();
			InstallFeatures();

			InitializeServices();
			InitializeFeatures();

			await WaitInitializationComplete();
		}

		public void Destroy()
		{
			DestroyServices();
			DestroyFeatures();
		}
		
		public T GetService<T>() where T : IService
		{
			var type = typeof(T);

			_servicesMap.TryGetValue(type, out var service);

			return (T)service;
		}
		
		public T GetFeature<T>() where T : IFeature
		{
			var type = typeof(T);

			_featuresMap.TryGetValue(type, out var feature);

			return (T)feature;
		}

		public IService[] GetAllServices()
		{
			return _servicesMap.Values.ToArray();
		}

		public IFeature[] GetAllFeatures()
		{
			return _featuresMap.Values.ToArray();
		}

		protected virtual void InstallServices() { }
		protected virtual void InstallFeatures() { }

		protected void AddService<T>(T service) where T : class, IService
		{
			var type = service.GetType();

			_servicesMap[type] = service;
			
			DI.Bind(service);
		}

		protected void AddFeature<T>(T feature) where T : class, IFeature
		{
			var type = feature.GetType();

			_featuresMap[type] = feature;

			DI.Bind(feature);
		}

		private void InitializeServices()
		{
			var allServices = GetAllServices();
			var count = allServices.Length;

			for (int i = 0; i < count; i++)
			{
				allServices[i].InitializeAsync().RunAsync();
			}
		}

		private void InitializeFeatures()
		{
			var allFeatures = GetAllFeatures();
			var count = allFeatures.Length;

			for (int i = 0; i < count; i++)
			{
				allFeatures[i].InitializeAsync().RunAsync();
			}
		}

		private async Task WaitInitializationComplete()
		{
			var allServices = GetAllServices();
			var allFeatures = GetAllFeatures();

			await UnityAwaiters.WaitUntil(() =>
				allFeatures.All(feature => feature.IsReady)
				&& allServices.All(service => service.IsReady));

			IsReady = true;
		}

		private void DestroyServices()
		{
			var allServices = GetAllServices();
			var count = allServices.Length;

			for (int i = 0; i < count; i++)
			{
				var service = allServices[i];
				
				service.DestroyAsync().RunAsync();
				
				DI.Unbind(service);
			}
		}

		private void DestroyFeatures()
		{
			var allFeatures = GetAllFeatures();
			var count = allFeatures.Length;

			for (int i = 0; i < count; i++)
			{
				var feature = allFeatures[i];
				
				feature.DestroyAsync().RunAsync();

				DI.Unbind(feature);
			}
		}
    }
}