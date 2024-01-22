namespace Lukomor.Example.Pong
{
    public class ScreenPauseViewModel : ScreenViewModel
    {
        private readonly GameSessionService _gameSessionsService;
        private readonly ScenesService _scenesService;

        public ScreenPauseViewModel(GameSessionService gameSessionsService, ScenesService scenesService)
        {
            _gameSessionsService = gameSessionsService;
            _scenesService = scenesService;
        }

        public void HandleResumeButtonClick()
        {
            _gameSessionsService.Unpause();
        }

        public void HandleMainMenuButtonClick()
        {
            _scenesService.LoadMainMenuScene();
        }
    }
}