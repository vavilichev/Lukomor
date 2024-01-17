using UnityEngine.SceneManagement;

namespace Lukomor.Example.Pong
{
    public class ScenesService
    {
        public const string SCENE_GAMEPLAY = "PongGameplay";
        public const string SCENE_MAIN_MENU = "PongMainMenu";

        public GameplayMode CachedGameplayMode { get; private set; }

        public string GetActiveSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }
        
        public void LoadGameplayScene(GameplayMode mode)
        {
            CachedGameplayMode = mode;
            SceneManager.LoadScene(SCENE_GAMEPLAY);
        }

        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene(SCENE_MAIN_MENU);
        }
    }
}