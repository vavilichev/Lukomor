using System;
using System.Collections.Generic;
using System.Linq;
using Lukomor.Presentation.Common;
using Lukomor.Presentation.Views.Windows;
using UnityEngine;

namespace Lukomor.Presentation
{
	public sealed class UserInterface : MonoBehaviour
	{
		private static class Keys
		{
			public const string UserInterfacePrefabPath = "[INTERFACE]";
			public const string WindowViewModelPrefabsFolder = "Prefabs/Windows";
			public const string WindowViewModelPrefabsConfigFolder = "Configs/UI";
		}

		public event Action<WindowViewModel> WindowOpened;
		public event Action<WindowViewModel> WindowClosed; 
		
		[SerializeField] private UILayerContainer[] _containers;
		[SerializeField] private SceneConfigUI[] _sceneConfigsUI;

		public WindowViewModel FocusedWindowViewModel { get; private set; }

		private static UserInterface _instance;
		private Dictionary<Type, WindowViewModel> _createdWindowViewModelsCache;
		private Dictionary<Type, string> _prefabWindowViewModelReferencesCache;
		private WindowsStack _windowStack;

		public static UserInterface GetOrCreateInstance()
		{
			if (_instance == null)
			{
				var prefab = Resources.Load<UserInterface>(Keys.UserInterfacePrefabPath);
				var ui = Instantiate(prefab);

				DontDestroyOnLoad(ui.gameObject);

				_instance = ui;
			}

			return _instance;
		}

		private void Awake()
		{
			if (_instance != null)
			{
				Destroy(gameObject);
				return;
			}
			
			_createdWindowViewModelsCache = new Dictionary<Type, WindowViewModel>();
			_prefabWindowViewModelReferencesCache = new Dictionary<Type, string>();
			_windowStack = new WindowsStack();
		}

		public void Build(string sceneName)
		{
			DestroyOldWindows();
			CreateNewWindows(sceneName);
		}
		
		private WindowViewModel CreateWindowViewModel(WindowViewModel prefabWindowViewModel)
		{
			var windowViewModelType = prefabWindowViewModel.GetType();

			if (_createdWindowViewModelsCache.TryGetValue(windowViewModelType, out var windowViewModel))
			{
				return windowViewModel;
			}
			
			var container = GetContainer(prefabWindowViewModel.WindowSettings.TargetLayer);
			var createdWindowViewModel = Instantiate(prefabWindowViewModel, container);
			
			_createdWindowViewModelsCache[windowViewModelType] = createdWindowViewModel;

			if (createdWindowViewModel.WindowSettings.OpenWhenCreated)
			{
				ActivateWindowViewModel(createdWindowViewModel);
			}
			else
			{
				createdWindowViewModel.Window.HideInstantly();
			}

			return createdWindowViewModel;
		}

		private void ActivateWindowViewModel(WindowViewModel windowViewModel)
		{
			windowViewModel.Refresh();
			
			if (!windowViewModel.IsActive)
			{
				windowViewModel.Subscribe();
				
				var window = windowViewModel.Window;
				
				window.Show();
				window.Hidden += OnWindowHidden;
				window.Destroyed += OnWindowDestroyed;
				
				_windowStack.Push(windowViewModel.GetType());

				FocusedWindowViewModel = windowViewModel;
				
				WindowOpened?.Invoke(FocusedWindowViewModel);
			}
		}
		
		private void CachePrefabPath(WindowViewModel windowViewModelPrefab)
		{
			var prefabName = windowViewModelPrefab.name;
			var prefabPath = $"{Keys.WindowViewModelPrefabsFolder}/{prefabName}";
			var windowViewModelType = windowViewModelPrefab.GetType();

			_prefabWindowViewModelReferencesCache[windowViewModelType] = prefabPath;
		}
		
		public IWindowOpenHandler ShowWindow<T>() where T : class, IWindow
		{
			var windowViewModelType = typeof(T);
			WindowViewModel windowViewModel;

			if (_createdWindowViewModelsCache.TryGetValue(windowViewModelType, out windowViewModel))
			{
				ActivateWindowViewModel(windowViewModel);
			}
			else
			{
				var prefabPath = _prefabWindowViewModelReferencesCache[windowViewModelType];
				var prefab = Resources.Load<WindowViewModel>(prefabPath);

				windowViewModel = CreateWindowViewModel(prefab);
			}

			var handler = new WindowOpenHandler(windowViewModel, this);

			return handler;
		}
		
		public void SetBackDestination<TWindowViewModel>() where TWindowViewModel : WindowViewModel
		{
			var windowViewModelType = typeof(TWindowViewModel);
			
			_windowStack.Pop();
			_windowStack.Push(windowViewModelType);
			_windowStack.Push(FocusedWindowViewModel.GetType());
		}
		
		public void Back()
		{
			if (FocusedWindowViewModel.Window is IHomeWindow)
			{
				return;
			}

			_windowStack.Pop();
			
			var windowTypeForRefreshing = _windowStack.Pop();
			var viewModelForRefreshing = _createdWindowViewModelsCache[windowTypeForRefreshing];
			
			ActivateWindowViewModel(viewModelForRefreshing);
		}
		
		private void DestroyOldWindows()
		{
			foreach (var createdWindowViewModelItem in _createdWindowViewModelsCache)
			{
				Destroy(createdWindowViewModelItem.Value);
			}
			
			_createdWindowViewModelsCache.Clear();
			_prefabWindowViewModelReferencesCache.Clear();
		}

		private void CreateNewWindows(string sceneName)
		{
			FocusedWindowViewModel = null;

			var sceneConfigUI = _sceneConfigsUI.First(c => c.SceneName == sceneName);
			var sceneViewModelsConfigPath = $"{Keys.WindowViewModelPrefabsConfigFolder}/{sceneConfigUI.SceneViewModelsFileName}";
			var sceneViewModels = Resources.Load<SceneViewModelsConfig>(sceneViewModelsConfigPath);
			var prefabsForCreating = sceneViewModels.ViewModelPrefabs;
			
			foreach (var prefab in prefabsForCreating)
			{
				if (prefab.WindowSettings.IsPreCached)
				{
					CreateWindowViewModel(prefab);
				}
				else
				{
					CachePrefabPath(prefab);
				}
			}
		}
		
		private Transform GetContainer(UILayer layer)
		{
			return _containers.FirstOrDefault(container => container.layer == layer)?.transform;
		}
		
		private void OnWindowDestroyed(WindowViewModel windowViewModel)
		{
			var window = windowViewModel.Window;
			
			window.Destroyed -= OnWindowDestroyed;
			window.Hidden -= OnWindowHidden;

			if (!windowViewModel.WindowSettings.IsPreCached)
			{
				var windowViewModelType = windowViewModel.GetType();

				_createdWindowViewModelsCache.Remove(windowViewModelType);
			}
		}
		
		private void OnWindowHidden(WindowViewModel windowViewModel)
		{
			_windowStack.RemoveLast(windowViewModel.GetType());
			windowViewModel.Unsubscribe();

			var focusedWindowType = _windowStack.GetLast();
			var focusedWindowViewModel = _createdWindowViewModelsCache[focusedWindowType];

			FocusedWindowViewModel = focusedWindowViewModel;

			WindowClosed?.Invoke(windowViewModel);
		}
		
	}
}