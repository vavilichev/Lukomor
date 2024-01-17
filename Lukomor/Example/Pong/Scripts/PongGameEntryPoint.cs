using Lukomor.DI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lukomor.Example.Pong.Scripts
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
            var scenesService = _rootContainer.RegisterSingleton(_ => new ScenesService()).CreateInstance();
            var sceneName = scenesService.GetActiveSceneName();

            if (sceneName == ScenesService.SCENE_GAMEPLAY)
            {
                StartGameplay(GameplayMode.OnePlayer);
                return;
            }

            if (sceneName == ScenesService.SCENE_MAIN_MENU)
            {
                StartMainMenu();
                return;
            }
            
            scenesService.LoadMainMenuScene();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == ScenesService.SCENE_MAIN_MENU)
            {
                StartMainMenu();
                return;
            }

            if (scene.name == ScenesService.SCENE_GAMEPLAY)
            {
                var gameplayMode = _rootContainer.Resolve<ScenesService>().CachedGameplayMode;
                StartGameplay(gameplayMode);
            }
        }

        public void StartMainMenu()
        {
            var entryPoint = Object.FindObjectOfType<PongMainMenuEntryPoint>();
            var mainMenuContainer = new DIContainer(_rootContainer);
            
            entryPoint.Process(mainMenuContainer);
        }
        
        public void StartGameplay(GameplayMode mode)
        {
            var entryPoint = Object.FindObjectOfType<PongGameplayEntryPoint>();
            var gameplayContainer = new DIContainer(_rootContainer);
            
            entryPoint.Process(gameplayContainer, mode);
        }

    }
}