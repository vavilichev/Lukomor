using System;
using System.Threading.Tasks;
using Lukomor.Common.Utils.Async;
using Lukomor.Domain.Contexts;
using Lukomor.Domain.Scenes;
using Lukomor.Presentation;
using UnityEngine;

namespace Lukomor.Application.Scenes
{
	public class SceneLoader : ISceneLoader
	{
		private const float Progress90 = 0.9f;

		public bool IsLoading { get; private set; }

		private bool isLoadingUnityScene { get; set; }
		
		private readonly UserInterface _ui;
		private readonly string[] _sceneNames;
		private readonly ProjectContext _projectContext;
		private IContext _currentSceneContext;

		public SceneLoader(ProjectContext projectContext, UserInterface ui, string[] sceneNames)
		{
			_projectContext = projectContext;
			_ui = ui;
			_sceneNames = sceneNames;
		}

		public async Task LoadScene(int sceneIndex, Action<SceneLoadingArgs> callback = null)
		{
			if (_sceneNames == null || _sceneNames.Length < sceneIndex + 1)
			{
				Debug.LogError($"SceneLoader: cannot load scene with index {sceneIndex}. Index out of range");

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
				Debug.LogError($"SceneLoader: cannot load scene {sceneName}. Another scene is loading now");

				callback?.Invoke(args);
			}
			else
			{
				IsLoading = true;
				
				UnloadCurrentSceneContext();

				await LoadUnitySceneAsync(sceneName);

				SceneContext loadedSceneContext = await LoadContext(sceneName);

				_ui.Build(loadedSceneContext.UISceneConfig);

				IsLoading = false;
				args.Success = true;

				callback?.Invoke(args);
			}
		}

		private void UnloadCurrentSceneContext()
		{
			_currentSceneContext?.Destroy();
		}

		private async Task<SceneContext> LoadContext(string sceneName)
		{
			SceneContext sceneContext = _projectContext.GetSceneContext(sceneName);

			if (sceneContext != null)
			{
				_currentSceneContext = sceneContext;
				
				await sceneContext.InitializeAsync();
			}

			return sceneContext;
		}

		private async Task LoadUnitySceneAsync(string sceneName)
		{
			isLoadingUnityScene = true;

			var asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
			asyncOperation.allowSceneActivation = false;

			while (asyncOperation.progress < Progress90)
			{
				await Task.Yield();
			}

			asyncOperation.allowSceneActivation = true;

			isLoadingUnityScene = false;

			await UnityAwaiters.WaitNextFrame();
		}
	}
}