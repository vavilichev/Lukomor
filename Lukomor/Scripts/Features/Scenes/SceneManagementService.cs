using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Lukomor.Features.Scenes
{
    public class SceneManagementService
    {
        public event Action<string> SceneLoadingStarted;
        public event Action<string> SceneChanged;
        public event Action<string> SceneLoaded;
        public event Action<string> SceneStarted;
        public event Action<string> SceneUnloaded;

        private ILoadingScreen _loadingScreen;

        public SceneManagementService(ILoadingScreen loadingScreen = null)
        {
            _loadingScreen = loadingScreen;
            
            SceneManager.sceneLoaded += (scene, _) => SceneLoaded?.Invoke(scene.name);
            SceneManager.sceneUnloaded += scene => SceneUnloaded?.Invoke(scene.name);
        }

        public void LoadScene(string sceneName)
        {
            LoadSceneAsync(sceneName);
        }

        public void LoadScene(int sceneIndex)
        {
            var path = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
            var lastSlash = path.LastIndexOf('/');
            var nameWithExtension = path.Substring(lastSlash + 1);
            var lastDot = nameWithExtension.LastIndexOf('.');
            var sceneName = nameWithExtension.Substring(0, lastDot);
            
            LoadScene(sceneName);
        }

        private async void LoadSceneAsync(string sceneName)
        {
            SceneLoadingStarted?.Invoke(sceneName);

            if (_loadingScreen != null)
            {
                await ShowAnimation(_loadingScreen.Show);
            }

            var async = SceneManager.LoadSceneAsync(sceneName);
            async.allowSceneActivation = false;

            while (async.progress < 0.9f)
            {
                await Task.Yield();
            }

            async.allowSceneActivation = true;
            
            SceneChanged?.Invoke(sceneName);

            await Task.Yield();

            if (_loadingScreen != null)
            {
                await ShowAnimation(_loadingScreen.Hide);
            }

            SceneStarted?.Invoke(sceneName);
        }

        private static async Task ShowAnimation(Action<Action> method)
        {
            var isCompleted = false;

            void OnComplete()
            {
                isCompleted = true;
            }
            
            method(OnComplete);

            while (!isCompleted)
            {
                await Task.Yield();
            }
        }
    }
}