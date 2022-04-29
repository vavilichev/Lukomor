using System;
using System.Collections;
using System.Threading.Tasks;
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

		private bool IsLoadingUnityScene { get; set; }
		private UserInterface UI { get; }
		private string[] SceneNames { get; }

		public SceneLoader(UserInterface ui, string[] sceneNames)
		{
			UI = ui;
			SceneNames = sceneNames;
		}

		public async Task LoadScene(int sceneIndex, Action<SceneLoadingArgs> callback = null)
		{
			if (SceneNames == null || SceneNames.Length < sceneIndex + 1)
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
				var sceneName = SceneNames[sceneIndex];

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
				SceneIndex = Array.IndexOf(SceneNames, sceneName),
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

				Coroutines.StartRoutine(LoadSceneRouting(sceneName));

				while (IsLoadingUnityScene)
				{
					await Task.Yield();
				}

				var scenesLibrary = Resources.Load<ScenesLibrary>(ScenesLibraryAssetPath);
				var sceneConfig = scenesLibrary.GetConfigOfScene(sceneName);

				if (sceneConfig != null)
				{
					UI.Build(sceneConfig);
				}

				IsLoading = false;
				args.Success = true;

				callback?.Invoke(args);
			}
		}

		private IEnumerator LoadSceneRouting(string sceneName)
		{
			IsLoadingUnityScene = true;

			var asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
			asyncOperation.allowSceneActivation = false;

			while (asyncOperation.progress < Progress90)
			{
				yield return null;
			}

			asyncOperation.allowSceneActivation = true;

			IsLoadingUnityScene = false;
		}
	}
}