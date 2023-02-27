using System;
using System.Threading.Tasks;
using Lukomor.Common.Utils.Async;
using Lukomor.Contexts;
using Lukomor.DI;
using Lukomor.Presentation;
using Lukomor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Lukomor.Scenes
{
	public class SceneLoader : ISceneLoader
	{
		private readonly DiContainer diContainer;
		private readonly string[] sceneNames;
		private bool IsLoadingScene { get; set; }
		private bool IsLoadingUnityScene { get; set; }
		private SceneContext CurrentSceneContext { get; set; }

		public SceneLoader(DiContainer diContainer, string[] sceneNames)
		{
			this.diContainer = diContainer;
			this.sceneNames = sceneNames;

			UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnUnitySceneLoaded;
		}

		public async Task LoadScene(int sceneIndex, Action<SceneLoadingArgs> callback = null)
		{
			if (sceneNames == null || sceneIndex >= sceneNames.Length)
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
				var sceneName = sceneNames[sceneIndex];

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
				SceneIndex = Array.IndexOf(sceneNames, sceneName),
				Success = false
			};

			if (!IsLoadingScene)
			{
				IsLoadingScene = true;

				UnloadCurrentSceneContext();

				await LoadUnitySceneAsync(sceneName);
				await LoadCurrentContext();

				BuildSceneUI();

				IsLoadingScene = false;
				args.Success = true;
			}
			else
			{
				Debug.LogError($"SceneLoader: cannot load scene {sceneName}. Another scene is loading now");
			}

			callback?.Invoke(args);
		}

		private void UnloadCurrentSceneContext()
		{
			if (CurrentSceneContext != null)
			{
				CurrentSceneContext.Destroy();
			}
		}

		private async Task LoadCurrentContext()
		{
			SceneContext sceneContext = Object.FindObjectOfType<SceneContext>();

			if (sceneContext != null)
			{
				CurrentSceneContext = sceneContext;

				await sceneContext.InitializeAsync(diContainer);
			}
		}

		private async Task LoadUnitySceneAsync(string sceneName)
		{
			IsLoadingUnityScene = true;

			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

			await UnityAwaiters.WaitWhile(() => IsLoadingUnityScene);
		}

		private void BuildSceneUI()
		{
			var ui = diContainer.Get<UserInterface>();

			ui.Build(CurrentSceneContext.UISceneConfig);
		}

		private void OnUnitySceneLoaded(Scene scene, LoadSceneMode mode)
		{
			IsLoadingUnityScene = false;
		}
	}
}