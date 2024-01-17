using System;

namespace Lukomor.Example.Pong
{
    public class PongScreenMainMenuViewModel : PongScreenViewModel
    {
        private readonly Action<GameplayMode> _startGameplay;
        
        public PongScreenMainMenuViewModel(Action<GameplayMode> startGameplay)
        {
            _startGameplay = startGameplay;
        }
        
        public void OnePlayerButtonClicked()
        {
            _startGameplay(GameplayMode.OnePlayer);
        }

        public void TwoPlayersButtonClicked()
        {
            _startGameplay(GameplayMode.TwoPlayer);
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