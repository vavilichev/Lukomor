namespace Lukomor.Example.Pong
{
    public class ScreenPauseViewModel : ScreenViewModel
    {
        private readonly GameSessionService _gameSessionsService;

        public ScreenPauseViewModel(GameSessionService gameSessionsService)
        {
            _gameSessionsService = gameSessionsService;
        }

        public void HandleClick()
        {
            _gameSessionsService.Unpause();
        }
    }
}