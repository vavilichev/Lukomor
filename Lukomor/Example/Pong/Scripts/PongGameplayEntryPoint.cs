using Lukomor.DI;
using Lukomor.MVVM;
using UnityEngine;

namespace Lukomor.Example.Pong
{
    public class PongGameplayEntryPoint : MonoBehaviour
    {
        private const int GAME_SCORE_LIMIT = 3;
        
        [SerializeField] private PongBlockView _leftBlock;
        [SerializeField] private PongBlockView _rightBlock;
        [SerializeField] private PongBallView _ball;
        [SerializeField] private PongGateView _gateLeft;
        [SerializeField] private PongGateView _gateRight;
        [SerializeField] private View _rootUIView;

        public void Process(DIContainer container, PongGameplayMode gameplayMode)
        {
            SetupPlayers(gameplayMode);

            container.RegisterSingleton("left_player_gate",
                c => new GateViewModel(c.Resolve<PongGameSessionService>(), true));
            container.RegisterSingleton("right_player_gate",
                c => new GateViewModel(c.Resolve<PongGameSessionService>(), false));

            container.RegisterSingleton("left_player_block", c => new BlockViewModel(c.Resolve<PongGameSessionService>()));
            container.RegisterSingleton("right_player_block", c => new BlockViewModel(c.Resolve<PongGameSessionService>()));
            
            var screensRegistrations = new PongScreensRegistrations();
            screensRegistrations.Register(container);

            container.RegisterSingleton(c => new PongUIRootViewModel(
                () => container.Resolve<PongScreenPauseViewModel>(),
                () => container.Resolve<PongScreenResultViewModel>(),
                () => container.Resolve<PongScreenGameplayViewModel>(),
                () => container.Resolve<PongScreenGoalViewModel>(),
                c.Resolve<PongGameSessionService>()
            ));

            container.RegisterSingleton(c => new PongGameSessionService(GAME_SCORE_LIMIT));
            container.RegisterSingleton(c => new BallViewModel(c.Resolve<PongGameSessionService>()));
            
            _ball.GetComponent<View>().Bind(container.Resolve<BallViewModel>());
            _gateLeft.GetComponent<View>().Bind(container.Resolve<GateViewModel>("left_player_gate"));
            _gateRight.GetComponent<View>().Bind(container.Resolve<GateViewModel>("right_player_gate"));
            _leftBlock.GetComponent<View>().Bind(container.Resolve<BlockViewModel>("left_player_block"));
            _rightBlock.GetComponent<View>().Bind(container.Resolve<BlockViewModel>("right_player_block"));

            var uiRootVM = container.Resolve<PongUIRootViewModel>();
            _rootUIView.Bind(container.Resolve<PongUIRootViewModel>());
            uiRootVM.OpenGameplayScreen();
        }

        private void SetupPlayers(PongGameplayMode mode)
        {
            SetupPlayer<PongFirstPlayerInputController>(_leftBlock);

            if (mode == PongGameplayMode.OnePlayer)
            {
                SetupAI(_rightBlock, _ball);
            }
            else
            {
                SetupPlayer<PongSecondPlayerInputController>(_rightBlock);
            }
        }
        
        private static void SetupPlayer<T>(PongBlockView block) where T : PongInputController
        {
            var inputController = block.GetComponent<PongInputController>();

            if (!inputController)
            {
                inputController = block.gameObject.AddComponent<T>();
            }
            
            inputController.Bind(block);
        }

        private static void SetupAI(PongBlockView block, PongBallView ball)
        {
            var inputController = block.GetComponent<PongAIInputController>();

            if (!inputController)
            {
                inputController = block.gameObject.AddComponent<PongAIInputController>();
            }
            
            inputController.Bind(block, ball);
        }
    }
}