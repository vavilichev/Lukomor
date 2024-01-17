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

            container.RegisterSingleton(_ => new GameSessionsService(gameState, scoreLimit));
            
            var screensRegistrations = new PongScreensRegistrations();
            screensRegistrations.Register(container);

            container.Register(_ => new PongUIRootViewModel(
                () => container.Resolve<PongScreenPauseViewModel>(),
                () => container.Resolve<PongScreenResultViewModel>(),
                () => container.Resolve<PongScreenGameplayViewModel>(),
                container.Resolve<GameSessionsService>()
            ));


            var uiRootVM = container.Resolve<PongUIRootViewModel>();
            _rootUIView.Bind(uiRootVM);
            uiRootVM.OpenMainMenuScreen();
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