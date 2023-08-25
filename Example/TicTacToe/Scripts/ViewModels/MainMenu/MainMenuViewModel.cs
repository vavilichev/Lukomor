using Lukomor.Features.Scenes;
using Lukomor.MVVM;

namespace LukomoreArchitecture.Example.TicTacToe.ViewModels.MainMenu
{
    public class MainMenuViewModel : IViewModel
    {
        private readonly SceneManagementService _sceneManagementService;
        
        public MainMenuViewModel(SceneManagementService sceneManagementService)
        {
            _sceneManagementService = sceneManagementService;
        }
        
        public void Start()
        {
            _sceneManagementService.LoadScene(2);
        }

        public void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            return;
#else
            Application.Quit();
#endif
        }
    }
}