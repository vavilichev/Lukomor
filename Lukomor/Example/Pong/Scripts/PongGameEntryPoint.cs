using Lukomor.DI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lukomor.Example.Pong
{
    public class PongGameEntryPoint
    {
        private static PongGameEntryPoint _instance;

        private DIContainer _rootContainer;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void StartGame()
        {
            _instance = new PongGameEntryPoint();
            _instance.Init();
        }

        private void Init()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            _rootContainer = new DIContainer();
            var scenesService = _rootContainer.RegisterSingleton(_ => new PongScenesService()).CreateInstance();
            var sceneName = scenesService.GetActiveSceneName();

            if (sceneName == PongScenesService.SCENE_GAMEPLAY)
            {
                StartGameplay(PongGameplayMode.OnePlayer);
                return;
            }

            if (sceneName == PongScenesService.SCENE_MAIN_MENU)
            {
                StartMainMenu();
                return;
            }
            
            scenesService.LoadMainMenuScene();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == PongScenesService.SCENE_MAIN_MENU)
            {
                StartMainMenu();
                return;
            }

            if (scene.name == PongScenesService.SCENE_GAMEPLAY)
            {
                var gameplayMode = _rootContainer.Resolve<PongScenesService>().CachedGameplayMode;
                StartGameplay(gameplayMode);
            }
        }

        public void StartMainMenu()
        {
            var entryPoint = Object.FindObjectOfType<PongMainMenuEntryPoint>();
            var mainMenuContainer = new DIContainer(_rootContainer);
            
            entryPoint.Process(mainMenuContainer);
        }
        
        public void StartGameplay(PongGameplayMode mode)
        {
            var entryPoint = Object.FindObjectOfType<PongGameplayEntryPoint>();
            var gameplayContainer = new DIContainer(_rootContainer);
            
            entryPoint.Process(gameplayContainer, mode);
        }

    }
}