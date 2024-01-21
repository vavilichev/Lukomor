using Lukomor.MVVM;

namespace Lukomor.Example.Pong
{
    public class GateViewModel : IViewModel
    {
        private readonly GameSessionService _gameSessionsService;
        private readonly PongPlayer _ownerPlayer;

        public GateViewModel(GameSessionService gameSessionsService, PongPlayer ownerPlayer)
        {
            _gameSessionsService = gameSessionsService;
            _ownerPlayer = ownerPlayer;
        }

        public void HandleBallCatch()
        {
            var opponentPlayer = _ownerPlayer == PongPlayer.One ? PongPlayer.Two : PongPlayer.One;
            
            _gameSessionsService.RegisterGoal(opponentPlayer);
        }
    }
}