using Lukomor.DI;
using UnityEngine;

namespace Lukomor.Features.Scenes
{
    public class SceneManagerInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _loadingScreenPrefab;
        public override void InstallBindings(IDIContainer container)
        {
            ILoadingScreen loadingScreen = default;

            if (_loadingScreenPrefab != null)
            {
                var loadingScreenGo = Instantiate(_loadingScreenPrefab);
                loadingScreen = loadingScreenGo.GetComponent<ILoadingScreen>();
                
                DontDestroyOnLoad(loadingScreenGo);
            }

            var sceneManagementService = new SceneManagementService(loadingScreen);
            
            container.Bind(sceneManagementService);
            
            sceneManagementService.LoadScene(1);
        }

        private void OnValidate()
        {
            if (_loadingScreenPrefab != null && _loadingScreenPrefab.GetComponent<ILoadingScreen>() == null)
            {
                Debug.LogError($"{gameObject.name} doesn't have any ILoadingScreen component.", gameObject);
            }
        }
    }
}