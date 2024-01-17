using System;
using UnityEngine;

namespace Lukomor.Example.Pong
{
    public class PongScreenMainMenuViewModel : PongScreenViewModel
    {
        private readonly Action _showPauseScreen;
        public PongScreenMainMenuViewModel(Action showPauseScreen)
        {
            _showPauseScreen = showPauseScreen;
        }
        
        public void OnePlayerButtonClicked()
        {
            // TODO: Setup one player game
            Debug.Log("One player clicked");
            _showPauseScreen?.Invoke();
        }

        public void TwoPlayersButtonClicked()
        {
            // TODO: Setup two players game
            Debug.Log("Two players clicked");
            _showPauseScreen?.Invoke();
        }
 
        public void ExitButtonClicked()
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