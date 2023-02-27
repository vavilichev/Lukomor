using System;
using System.Collections.Generic;
using System.Linq;
using Lukomor.DI;
using Lukomor.UI.Common;
using UnityEngine;

namespace Lukomor.UI
{
	public sealed class UserInterface : MonoBehaviour
	{
		public event Action<WindowViewModel> WindowOpened;
		public event Action<WindowViewModel> WindowClosed; 
		
		[SerializeField] private UILayerContainer[] containers;

		public WindowViewModel FocusedWindowViewModel { get; private set; }

		private static UserInterface instance;
		private UISceneConfig config;
		private Dictionary<Type, WindowViewModel> createdWindowViewModelsCache;
		private Stack<Type> windowsJournalStack;
		private DiContainer diContainer;

		public static UserInterface CreateInstance(UserInterface prefab, DiContainer diContainer)
		{
			if (instance == null)
			{
				instance = Instantiate(prefab);
				
				instance.createdWindowViewModelsCache = new Dictionary<Type, WindowViewModel>();
				instance.windowsJournalStack = new Stack<Type>();
				instance.diContainer = diContainer;
				
				DontDestroyOnLoad(instance.gameObject);
			}

			return instance;
		}

		public T InstantiateViewModel<T>(T prefab, Transform parent) where T : ViewModel
		{
			var createdViewModel = Instantiate(prefab, parent);

			createdViewModel.Construct(diContainer);

			return createdViewModel;
		}
		
		public void Build(UISceneConfig config)
		{
			this.config = config;
			
			DestroyOldWindows();
			CreateNewWindows();
		}
		
		public T ShowWindow<T>() where T : WindowViewModel
		{
			var windowViewModelType = typeof(T);
			var showingWindow = ShowWindow(windowViewModelType);

			return (T) showingWindow;
		}

		public void Back()
		{
			if (windowsJournalStack.Count <= 1)
			{
				return;
			}

			FocusedWindowViewModel.Window.Hide();

			var windowTypeForOpening = windowsJournalStack.Pop();

			ShowWindow(windowTypeForOpening);
		}
		
		public Transform GetContainer(UILayer layer)
		{
			return containers.FirstOrDefault(container => container.Layer == layer)?.transform;
		}

		private WindowViewModel ShowWindow(Type windowType)
		{
			WindowViewModel windowViewModel;

			if (createdWindowViewModelsCache.TryGetValue(windowType, out windowViewModel))
			{
				ActivateWindowViewModel(windowViewModel);
			}
			else
			{
				if (!config.TryGetPrefab(windowType, out WindowViewModel prefab))
				{
					Debug.Log($"<color=#FF0000>Couldn't open window ({windowType}). It doesn't exist in the config of this scene. </color>");
					return null;
				}

				windowViewModel = CreateWindowViewModel(prefab);
			}

			return windowViewModel;
		}
		
		
		
		
		
		private WindowViewModel CreateWindowViewModel(WindowViewModel prefabWindowViewModel)
		{
			var windowViewModelType = prefabWindowViewModel.GetType();

			if (createdWindowViewModelsCache.TryGetValue(windowViewModelType, out var windowViewModel))
			{
				return windowViewModel;
			}
			
			var transformContainer = GetContainer(prefabWindowViewModel.WindowSettings.TargetLayer);
			var createdWindowViewModel = InstantiateViewModel(prefabWindowViewModel, transformContainer);
			
			createdWindowViewModelsCache[windowViewModelType] = createdWindowViewModel;

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
			
			if (!windowViewModel.Window.IsShown)
			{
				windowViewModel.Subscribe();
				
				var window = windowViewModel.Window;
				
				window.Show();
				window.Hidden += OnWindowHidden;
				window.Destroyed += OnWindowDestroyed;
				
				windowsJournalStack.Push(windowViewModel.GetType());

				FocusedWindowViewModel = windowViewModel;
				
				WindowOpened?.Invoke(FocusedWindowViewModel);
			}
		}
		
		private void DestroyOldWindows()
		{
			foreach (var createdWindowViewModelItem in createdWindowViewModelsCache)
			{
				Destroy(createdWindowViewModelItem.Value.gameObject);
			}
			
			createdWindowViewModelsCache.Clear();
		}

		private void CreateNewWindows()
		{
			FocusedWindowViewModel = null;

			var prefabsForCreating = config.WindowPrefabs;
			
			foreach (var prefab in prefabsForCreating)
			{
				if (prefab.WindowSettings.IsPreCached)
				{
					CreateWindowViewModel(prefab);
				}
			}
		}

		private void OnWindowDestroyed(WindowViewModel windowViewModel)
		{
			var window = windowViewModel.Window;
			
			window.Destroyed -= OnWindowDestroyed;
			window.Hidden -= OnWindowHidden;

			if (!windowViewModel.WindowSettings.IsPreCached)
			{
				var windowViewModelType = windowViewModel.GetType();

				createdWindowViewModelsCache.Remove(windowViewModelType);
			}
		}
		
		private void OnWindowHidden(WindowViewModel windowViewModel)
		{
			windowsJournalStack.RemoveLast(windowViewModel.GetType());
			windowViewModel.Unsubscribe();

			var focusedWindowType = windowsJournalStack.GetLast();
			var focusedWindowViewModel = createdWindowViewModelsCache[focusedWindowType];

			FocusedWindowViewModel = focusedWindowViewModel;

			WindowClosed?.Invoke(windowViewModel);
		}
		
	}
}