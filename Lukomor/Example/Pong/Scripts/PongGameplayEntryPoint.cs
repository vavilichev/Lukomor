using Lukomor.DI;
using Lukomor.MVVM;
using UnityEngine;

namespace Lukomor.Example.Pong
{
    public class PongGameplayEntryPoint : MonoBehaviour
    {
        [SerializeField] private Block _leftBlock;
        [SerializeField] private Block _rightBlock;
        [SerializeField] private Ball _ball;
        [SerializeField] private Gate _gateLeft;
        [SerializeField] private Gate _gateRight;
        [SerializeField] private View _rootUIView;

        public void Process(DIContainer container, GameplayMode gameplayMode)
        {
            Debug.Log("GameplayEntryPoint: " + gameplayMode);
            
            // TODO: Load state;
            // TODO: Load Control
            // TODO: Load Services
            // TODO: Load UI
            
            var gameState = new PongGameState();
            const int scoreLimit = 3;

            SetupPlayers(gameplayMode);

            container.RegisterSingleton("left_player_gate",
                c => new GateViewModel(c.Resolve<GameSessionsService>(), true));
            container.RegisterSingleton("right_player_gate",
                c => new GateViewModel(c.Resolve<GameSessionsService>(), false));

            container.RegisterSingleton("left_player_block", c => new BlockViewModel(c.Resolve<GameSessionsService>()));
            container.RegisterSingleton("right_player_block", c => new BlockViewModel(c.Resolve<GameSessionsService>()));
            
            var screensRegistrations = new PongScreensRegistrations();
            screensRegistrations.Register(container);

            container.RegisterSingleton(_ => new PongUIRootViewModel(
                () => container.Resolve<PongScreenPauseViewModel>(),
                () => container.Resolve<PongScreenResultViewModel>(),
                () => container.Resolve<PongScreenGameplayViewModel>(),
                () => container.Resolve<PongScreenGoalViewModel>()
            ));

            container.RegisterSingleton(c => new GameSessionsService(gameState, scoreLimit,
                c.Resolve<PongUIRootViewModel>().OpenGoalScreen));
            container.RegisterSingleton(c => new BallViewModel(c.Resolve<GameSessionsService>()));
            
            
           

            _ball.GetComponent<View>().Bind(container.Resolve<BallViewModel>());
            _gateLeft.GetComponent<View>().Bind(container.Resolve<GateViewModel>("left_player_gate"));
            _gateRight.GetComponent<View>().Bind(container.Resolve<GateViewModel>("right_player_gate"));
            _leftBlock.GetComponent<View>().Bind(container.Resolve<BlockViewModel>("left_player_block"));
            _rightBlock.GetComponent<View>().Bind(container.Resolve<BlockViewModel>("right_player_block"));

            var uiRootVM = container.Resolve<PongUIRootViewModel>();
            _rootUIView.Bind(uiRootVM);
            uiRootVM.OpenGameplayScreen();
        }

        private void SetupPlayers(GameplayMode mode)
        {
            SetupPlayer<FirstPlayerInputController>(_leftBlock);

            if (mode == GameplayMode.OnePlayer)
            {
                SetupAI(_rightBlock, _ball);
            }
            else
            {
                SetupPlayer<SecondPlayerInputController>(_rightBlock);
            }
        }
        
        private static void SetupPlayer<T>(Block block) where T : InputController
        {
            var inputController = block.GetComponent<InputController>();

            if (!inputController)
            {
                inputController = block.gameObject.AddComponent<T>();
            }
            
            inputController.Bind(block);
        }

        private static void SetupAI(Block block, Ball ball)
        {
            var inputController = block.GetComponent<AIInputController>();

            if (!inputController)
            {
                inputController = block.gameObject.AddComponent<AIInputController>();
            }
            
            inputController.Bind(block, ball);
        }
    }
}