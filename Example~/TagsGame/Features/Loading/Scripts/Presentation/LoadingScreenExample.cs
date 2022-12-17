using Lukomor.Application;
using Lukomor.Common.DIContainer;
using Lukomor.Domain.Scenes;
using UnityEngine;

namespace Lukomor.TagsGame.Loading.Presentation
{
    public class LoadingScreenExample : MonoBehaviour
    {
        [SerializeField] private GameObject _goContent;

        private static LoadingScreenExample _instance;

        #region Unity lifecycle

        private void Start()
        {
            if (TryCreateSingleton())
            {
                if (!Game.IsMainObjectsBound)
                {
                    Game.ProjectContextPreInitialized += OnGameProjectContextPreInitialized;
                }
                else
                {
                    Init();
                }
            }
        }
        
        private void OnDestroy()
        {
            if (Game.IsMainObjectsBound)
            {
                var sceneManager = DI.Get<ISceneManager>();

                sceneManager.SceneLoading -= OnSceneLoadingStarted;
                sceneManager.SceneLoaded -= OnSceneLoaded;
            }
        }

        #endregion
        
        private bool TryCreateSingleton()
        {
            var singletonCreated = false;

            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                singletonCreated = true;
                _instance = this;

                DontDestroyOnLoad(gameObject);
            }

            return singletonCreated;
        }

        private void Init()
        {
            var sceneManager = DI.Get<ISceneManager>();
                
            sceneManager.SceneLoading += OnSceneLoadingStarted;
            sceneManager.SceneLoaded += OnSceneLoaded;

            if (sceneManager.IsLoading)
            {
                OnSceneLoadingStarted();
            }
        }

        private void OnGameProjectContextPreInitialized()
        {
            Game.ProjectContextPreInitialized -= OnGameProjectContextPreInitialized;
            
            Init();
        }

        private void OnSceneLoadingStarted()
        {
            _goContent.SetActive(true);
        }

        private void OnSceneLoaded(bool success)
        {
            _goContent.SetActive(false);
        }
    }
}