using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lukomor.Example.Pong.Scripts
{
    public class PongGameEntryPoint
    {
        private const string SCENE_GAMEPLAY = "PongGameplay";
        private const string SCENE_MAIN_MENU = "PongMainMenu";
        
        private static PongGameEntryPoint _instance;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void StartGame()
        {
            _instance = new PongGameEntryPoint();
            _instance.Init();
        }

        private void Init()
        {
            var sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == SCENE_GAMEPLAY)
            {
                StartGameplay(GameplayMode.OnePlayer);
                return;
            }

            if (sceneName == SCENE_MAIN_MENU)
            {
                StartMainMenu();
                return;
            }
            
            LoadMainMenu();
            
            // TODO: also create container where we can put a scene manager that can load different scenes.
            // In the child scenes we can create different viewModels that can run methods from this scene manager.
            // We just put methods into these viewModels.
        }

        private void StartGameplay(GameplayMode mode)
        {
            var entryPoint = Object.FindObjectOfType<PongGameplayEntryPoint>();
            entryPoint.Process(mode);
        }

        private void StartMainMenu()
        {
            var entryPoint = Object.FindObjectOfType<PongMainMenuEntryPoint>();
            entryPoint.Process();
        }
        
        private void LoadMainMenu()
        {
            SceneManager.sceneLoaded += OnMainMenuSceneLoaded;
            SceneManager.LoadScene(SCENE_MAIN_MENU);
        }
        
        private void OnMainMenuSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == SCENE_MAIN_MENU)
            {
                SceneManager.sceneLoaded -= OnMainMenuSceneLoaded;
                StartMainMenu();
            }
        }
    }
}