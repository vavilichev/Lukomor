using System;

namespace Lukomor.Example.Pong
{
    public class ScreenMainMenuViewModel : ScreenViewModel
    {
        private readonly Action<PongGameplayMode> _startGameplay;
        
        public ScreenMainMenuViewModel(Action<PongGameplayMode> startGameplay)
        {
            _startGameplay = startGameplay;
        }
        
        public void OnePlayerButtonClicked()
        {
            _startGameplay(PongGameplayMode.OnePlayer);
        }

        public void TwoPlayersButtonClicked()
        {
            _startGameplay(PongGameplayMode.TwoPlayer);
        }
 
        public void ExitButtonClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}