using System;
using System.IO;
using System.Threading.Tasks;
using Lukomor.Domain.Contexts;
using Lukomor.Domain.Scenes;
using Lukomor.Presentation;
using UnityEngine;

namespace Lukomor.Application.Scenes {
	public class SceneManager : MonoBehaviour, ISceneManager {
		public event Action SceneLoading;
		public event Action<bool> SceneLoaded;
		
		public bool IsLoading { get; private set; }

		private ISceneLoader _sceneLoader;
		
		public static ISceneManager CreateInstance(ProjectContext projectContext, UserInterface ui)
		{
			var go = new GameObject("[Scene Manager]");
			var sceneManager = go.AddComponent<SceneManager>();

			sceneManager.Init(ui, projectContext);
			
			DontDestroyOnLoad(go);
			
			return sceneManager;
		}

		public Task LoadScene(int sceneIndex) {
			IsLoading = true;
			
			RaiseSceneLoadingStartedCallback();
			
			return _sceneLoader.LoadScene(sceneIndex, SceneLoadingResultsCallback);
		}

		public Task LoadScene(string sceneName) {
			IsLoading = true;
			
			RaiseSceneLoadingStartedCallback();
			
			return _sceneLoader.LoadScene(sceneName, SceneLoadingResultsCallback);
		}

		public Task ReloadScene() {
			IsLoading = true;
			
			RaiseSceneLoadingStartedCallback();

			return _sceneLoader.ReloadScene(SceneLoadingResultsCallback);
		}

		private void Init(UserInterface ui, ProjectContext projectContext) {
			var sceneNames = CacheSceneNames();
			
			_sceneLoader = new SceneLoader(projectContext, ui, sceneNames);
		}
		
		private string[] CacheSceneNames() {
			var scenesCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
			var sceneNames = new string[scenesCount];

			for (int i = 0; i < scenesCount; i++) {
				sceneNames[i] = Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
			}

			return sceneNames;
		}

		private void RaiseSceneLoadingStartedCallback() {
			SceneLoading?.Invoke();
		}
		
		private void SceneLoadingResultsCallback(SceneLoadingArgs e) {
			IsLoading = false;

			SceneLoaded?.Invoke(e.Success);
		}
	}
}