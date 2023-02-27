using System;
using System.IO;
using System.Threading.Tasks;
using Lukomor.DI;

namespace Lukomor.Scenes
{
	public class SceneManager : ISceneManager
	{
		public event Action SceneLoadingStarted;
		public event Action<bool> SceneLoaded;

		public bool IsLoading { get; private set; }

		private ISceneLoader SceneLoader { get; }

		public SceneManager(DiContainer diContainer)
		{
			var sceneNames = CacheSceneNames();

			SceneLoader = new SceneLoader(diContainer, sceneNames);
		}
		
		public Task LoadScene(int sceneIndex)
		{
			IsLoading = true;

			RaiseSceneLoadingStartedCallback();

			return SceneLoader.LoadScene(sceneIndex, OnSceneLoaded);
		}

		public Task LoadScene(string sceneName)
		{
			IsLoading = true;

			RaiseSceneLoadingStartedCallback();

			return SceneLoader.LoadScene(sceneName, OnSceneLoaded);
		}

		public Task ReloadScene()
		{
			IsLoading = true;

			RaiseSceneLoadingStartedCallback();

			return SceneLoader.ReloadScene(OnSceneLoaded);
		}

		private string[] CacheSceneNames()
		{
			var scenesCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
			var sceneNames = new string[scenesCount];

			for (int i = 0; i < scenesCount; i++)
			{
				sceneNames[i] =
					Path.GetFileNameWithoutExtension(
						UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
			}

			return sceneNames;
		}

		private void RaiseSceneLoadingStartedCallback()
		{
			SceneLoadingStarted?.Invoke();
		}

		private void OnSceneLoaded(SceneLoadingArgs e)
		{
			IsLoading = false;

			SceneLoaded?.Invoke(e.Success);
		}
	}
}