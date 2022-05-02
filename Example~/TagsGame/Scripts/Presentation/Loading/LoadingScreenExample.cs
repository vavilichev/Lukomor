using Lukomor.Example.Domain;
using UnityEngine;

namespace Lukomor.Example.Presentation.Loading
{
    public class LoadingScreenExample : MonoBehaviour
    {
        [SerializeField] private GameObject _goContent;

        private static LoadingScreenExample _instance;

        private void Start()
        {
            if (CreateSingleton())
            {
                TagsGame.SceneManager.SceneLoading += OnSceneLoadingStarted;
                TagsGame.SceneManager.SceneLoaded += OnSceneLoaded;

                if (TagsGame.SceneManager.IsLoading)
                {
                    OnSceneLoadingStarted();
                }
            }
        }

        private bool CreateSingleton()
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

        private void OnDestroy()
        {
            TagsGame.SceneManager.SceneLoading -= OnSceneLoadingStarted;
            TagsGame.SceneManager.SceneLoaded -= OnSceneLoaded;
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