using System;

namespace Lukomor.Example.Pong
{
    public class PongScreenMainMenuViewModel : PongScreenViewModel
    {
        private readonly Action<PongGameplayMode> _startGameplay;
        
        public PongScreenMainMenuViewModel(Action<PongGameplayMode> startGameplay)
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