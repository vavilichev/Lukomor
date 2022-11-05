using System;
using System.Collections.Generic;
using System.Linq;
using Lukomor.Common;
using Lukomor.Common.Scenes;
using Lukomor.Presentation.Common;
using Lukomor.Presentation.Views.Windows;
using Lukomor.Presentation.Views.Windows.Extensions;
using UnityEngine;
using VavilichevGD.Tools.Async;
using VavilichevGD.Tools.Logging;

namespace Lukomor.Presentation
{
	public sealed class UserInterface : MonoBehaviour
	{
		private const string PrefabPath = "[INTERFACE]";

		[SerializeField] private UILayerContainer[] _containers;

		public IWindow FocusedWindow { get; private set; }

		private Dictionary<Type, IWindow> _createdWindowsCache;
		private Dictionary<Type, string> _prefabWindowReferencesCache;
		private WindowsStack _windowStack;

		public static UserInterface CreateInstance()
		{
			var prefab = Resources.Load<UserInterface>(PrefabPath);
			var ui = Instantiate(prefab);

			DontDestroyOnLoad(ui.gameObject);

			return ui;
		}

		private void Awake()
		{
			_createdWindowsCache = new Dictionary<Type, IWindow>();
			_prefabWindowReferencesCache = new Dictionary<Type, string>();
			_windowStack = new WindowsStack();

			DontDestroyOnLoad(gameObject);
		}
 
		public void Build(ISceneConfig config)
		{
			FocusedWindow = null;

			var prefabs = config.WindowPrefabs;
			var createdWindows = _createdWindowsCache.Values.ToArray();

			if (_createdWindowsCache.Count > 0)
			{
				DestroyOldWindows(createdWindows, prefabs);
			}

			CreateNewWindows(createdWindows, prefabs);
		}

		public T ShowWindow<T>() where T : class, IWindow
		{
			var windowType = typeof(T);
			var showedWindow = ShowWindowInternal(windowType);
			
			showedWindow.Subscribe();
			showedWindow.Refresh();

			return (T)showedWindow;
		}

		public T ShowWindow<T>(Payload payload) where T : class, IWindow
		{
			var windowType = typeof(T);
			var showedWindow = ShowWindowInternal(windowType);

			showedWindow.AddPayload(payload.Key, payload.Value);
			
			showedWindow.Subscribe();
			showedWindow.Refresh();

			return (T)showedWindow;
		}

		public T ShowWindow<T>(Payload[] payloads) where T : class, IWindow
		{
			var windowType = typeof(T);
			var showedWindow = ShowWindowInternal(windowType);
			var payloadsAmount = payloads.Length;

			for (int i = 0; i < payloadsAmount; i++)
			{
				var payload = payloads[i];
				
				showedWindow.AddPayload(payload.Key, payload.Value);
			}

			showedWindow.Subscribe();
			showedWindow.Refresh();

			return (T)showedWindow;
		}

		public UserInterface SetBackDestination<T>()
		{
			var windowType = typeof(T);

			_windowStack.Pop();
			_windowStack.Push(windowType);
			_windowStack.Push(FocusedWindow.GetType());

			return this;
		}

		public void Back()
		{
			if (_windowStack.Length > 1)
			{
				var focusedWindowType = FocusedWindow.GetType();
				
				FocusedWindow.Hide().RunAsync();
				_windowStack.RemoveLast(focusedWindowType);
				
				var previousWindowType = _windowStack.Pop();

				_createdWindowsCache.TryGetValue(previousWindowType, out var previousWindow);

				var needToRefresh = previousWindow != null && !previousWindow.IsActive;
				var showedWindow = ShowWindowInternal(previousWindowType);

				if (needToRefresh)
				{
					showedWindow.Subscribe();
					showedWindow.Refresh();
				}
			}
		}
		
		private IWindow ShowWindowInternal(Type windowType)
		{
			var previousWindow = FocusedWindow;

			if (previousWindow?.GetType() == windowType)
			{
				ActivateWindow(previousWindow);
				
				return previousWindow;
			}

			_createdWindowsCache.TryGetValue(windowType, out var windowForShowing);

			if (windowForShowing == null)
			{
				_prefabWindowReferencesCache.TryGetValue(windowType, out var prefabPath);

				if (!string.IsNullOrEmpty(prefabPath))
				{
					var prefabGO = Resources.Load<GameObject>(prefabPath);
					var prefabWindow = prefabGO.GetComponent<IWindow>();

					windowForShowing = CreateWindow(prefabWindow);
				}
				else
				{
					Log.PrintError($"Couldn't find window of type {windowType.Name} for opening");
				}
			}

			ActivateWindow(windowForShowing);

			return windowForShowing;
		}

		private void ActivateWindow(IWindow window)
		{
			if (window != null)
			{
				if (!window.IsActive)
				{
					window.Show().RunAsync();
				}
				
				if (window is IHomeWindow)
				{
					_windowStack.Clear();
				}
				
				FocusedWindow = window;
				_windowStack.Push(window.GetType());
			}
		}

		private void DestroyOldWindows(IWindow[] alreadyCreatedWindows, IWindow[] prefabsForCreating)
		{
			foreach (var createdWindow in alreadyCreatedWindows)
			{
				if (!prefabsForCreating.ContainsWindow(createdWindow))
				{
					var type = createdWindow.GetType();

					Destroy(createdWindow.GameObject);
					_createdWindowsCache.Remove(type);
				}
			}
		}

		private void CreateNewWindows(IWindow[] alreadyCreatedWindows, IWindow[] prefabsForCreating)
		{
			foreach (var prefab in prefabsForCreating)
			{
				if (prefab.IsPreCached)
				{
					if (!alreadyCreatedWindows.ContainsWindow(prefab))
					{
						CreateWindow(prefab);
					}
				}
				else
				{
					CachePrefabPath(prefab);
				}
			}
		}

		private IWindow CreateWindow(IWindow prefab)
		{
			prefab.GameObject.SetActive(false);
			
			var container = GetContainer(prefab.TargetLayer);
			var createdWindowGO = Instantiate(prefab.GameObject, container);
			var createdWindow = createdWindowGO.GetComponent<IWindow>();
			var createdWindowType = createdWindow.GetType();

			createdWindow.UI = this;
			
			createdWindow.Install();

			if (createdWindow.IsPreCached && createdWindow.OpenedByDefault)
			{
				createdWindowGO.SetActive(true);
				
				_windowStack.Push(createdWindowType);
				FocusedWindow = createdWindow;
				
				createdWindow.Subscribe();
				createdWindow.Refresh();
			}

			_createdWindowsCache[createdWindowType] = createdWindow;

			createdWindow.Destroyed += OnWindowDestroyed;
			createdWindow.Hidden += OnWindowHidden;
			
			prefab.GameObject.SetActive(true);
			
			return createdWindow;
		}

		private void CachePrefabPath(IWindow prefab)
		{
			var prefabName = prefab.GameObject.name;
			var prefabPath = $"Windows/{prefabName}";
			var windowType = prefab.GetType();

			_prefabWindowReferencesCache[windowType] = prefabPath;
		}

		private Transform GetContainer(UILayer layer)
		{
			return _containers.FirstOrDefault(container => container.layer == layer)?.transform;
		}

		private void OnWindowDestroyed(IWindow window)
		{
			window.Destroyed -= OnWindowDestroyed;
			window.Hidden -= OnWindowHidden;

			if (!window.IsPreCached)
			{
				var windowType = window.GetType();

				_createdWindowsCache.Remove(windowType);
			}
		}
		
		private void OnWindowHidden(IWindow window)
		{
			_windowStack.RemoveLast(window.GetType());
			window.Unsubscribe();
		}
	}
}