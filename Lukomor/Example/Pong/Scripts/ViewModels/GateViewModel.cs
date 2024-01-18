using Lukomor.MVVM;

namespace Lukomor.Example.Pong
{
    public class GateViewModel : IViewModel
    {
        private readonly GameSessionsService _gameSessionsService;
        private readonly bool _isLeftPlayer;

        public GateViewModel(GameSessionsService gameSessionsService, bool isLeftPlayer)
        {
            _gameSessionsService = gameSessionsService;
            _isLeftPlayer = isLeftPlayer;
        }

        public void RegisterGoal()
        {
            _gameSessionsService.RegisterGoal(_isLeftPlayer);
        }
    }
}