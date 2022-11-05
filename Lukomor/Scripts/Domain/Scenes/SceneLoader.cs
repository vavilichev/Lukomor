using System;
using System.Collections;
using System.Threading.Tasks;
using Lukomor.Application.Contexts;
using Lukomor.Common.Scenes;
using Lukomor.Presentation;
using UnityEngine;
using VavilichevGD.Tools.Coroutines;
using VavilichevGD.Tools.Logging;

namespace Lukomor.Domain.Scenes
{
	public class SceneLoader : ISceneLoader
	{
		private const string ScenesLibraryAssetPath = "SceneConfigs/ScenesLibrary";
		private const float Progress90 = 0.9f;

		public bool IsLoading { get; private set; }

		private bool isLoadingUnityScene { get; set; }
		
		private readonly UserInterface _ui;
		private readonly string[] _sceneNames;
		private readonly IContext _projectContext;
		private readonly IContext _currentSceneContext;

		public SceneLoader(UserInterface ui, string[] sceneNames, IContext projectContext)
		{
			_ui = ui;
			_sceneNames = sceneNames;
			_projectContext = projectContext;
		}

		public async Task LoadScene(int sceneIndex, Action<SceneLoadingArgs> callback = null)
		{
			if (_sceneNames == null || _sceneNames.Length < sceneIndex + 1)
			{
				Log.PrintError($"SceneLoader: cannot load scene with index {sceneIndex}. Index out of range");

				var args = new SceneLoadingArgs
				{
					SceneIndex = sceneIndex,
					Success = false
				};

				callback?.Invoke(args);
			}
			else
			{
				var sceneName = _sceneNames[sceneIndex];

				await LoadSceneAsync(sceneName, callback);
			}
		}

		public async Task LoadScene(string sceneName, Action<SceneLoadingArgs> callback = null)
		{
			await LoadSceneAsync(sceneName, callback);
		}

		public async Task ReloadScene(Action<SceneLoadingArgs> callback = null)
		{
			var sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

			await LoadSceneAsync(sceneName, callback);
		}

		private async Task LoadSceneAsync(string sceneName, Action<SceneLoadingArgs> callback = null)
		{
			var args = new SceneLoadingArgs
			{
				SceneName = sceneName,
				SceneIndex = Array.IndexOf(_sceneNames, sceneName),
				Success = false
			};

			if (IsLoading)
			{
				Log.PrintError($"SceneLoader: cannot load scene {sceneName}. Another scene is loading now");

				callback?.Invoke(args);
			}
			else
			{
				IsLoading = true;
				
				UnloadCurrentContext();

				Coroutines.StartRoutine(LoadSceneRouting(sceneName));

				while (isLoadingUnityScene)
				{
					await Task.Yield();
				}

				await Task.Yield(); 			// It's required to switching scenes before new scene context will start loading.
				
				await LoadContext(sceneName);

				var scenesLibrary = Resources.Load<ScenesLibrary>(ScenesLibraryAssetPath);
				var sceneConfig = scenesLibrary.GetConfigOfScene(sceneName);

				if (sceneConfig != null)
				{
					_ui.Build(sceneConfig);
				}

				IsLoading = false;
				args.Success = true;

				callback?.Invoke(args);
			}
		}

		private void UnloadCurrentContext()
		{
			_currentSceneContext?.Destroy();
		}

		private async Task LoadContext(string sceneName)
		{
			var sceneContext = _projectContext.GetSceneContext(sceneName);

			if (sceneContext != null)
			{
				_currentSceneContext = sceneContext;
				
				await _currentSceneContext.InitializeAsync();
			}
		}

		private IEnumerator LoadSceneRouting(string sceneName)
		{
			isLoadingUnityScene = true;

			var asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
			asyncOperation.allowSceneActivation = false;

			while (asyncOperation.progress < Progress90)
			{
				yield return null;
			}

			asyncOperation.allowSceneActivation = true;

			isLoadingUnityScene = false;
		}
	}
}