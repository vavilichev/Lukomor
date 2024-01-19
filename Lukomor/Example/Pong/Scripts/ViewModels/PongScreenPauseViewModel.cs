namespace Lukomor.Example.Pong
{
    public class PongScreenPauseViewModel : PongScreenViewModel
    {
        private readonly PongGameSessionService _gameSessionsService;

        public PongScreenPauseViewModel(PongGameSessionService gameSessionsService)
        {
            _gameSessionsService = gameSessionsService;
        }

        public void HandleClick()
        {
            _gameSessionsService.Unpause();
        }
    }
}