using Lukomor.DI;
using Lukomor.Example.Pong.Scripts;
using Lukomor.Example.Pong.Scripts.Services;
using Lukomor.MVVM;
using UnityEngine;

namespace Lukomor.Example.Pong
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private Block _leftBlock;
        [SerializeField] private Block _rightBlock;
        [SerializeField] private Ball _ball;
        [SerializeField] private Gate _gateLeft;
        [SerializeField] private Gate _gateRight;
        [SerializeField] private View _rootUIView;

        private void Start()
        {
            SetupPlayer<FirstPlayerInputController>(_leftBlock);
            SetupAI(_rightBlock, _ball);

            var gameState = new PongGameState();
            var scoreLimit = 3;

            var container = new DIContainer();
            container.RegisterSingleton(_ => new GameSessionsService(gameState, scoreLimit));
            
            var screensRegistrations = new PongScreensRegistrations();
            screensRegistrations.Register(container);

            container.Register(_ => new PongUIRootViewModel(
                () => container.Resolve<PongScreenMainMenuViewModel>(),
                () => container.Resolve<PongScreenPauseViewModel>(),
                () => container.Resolve<PongScreenResultViewModel>(),
                () => container.Resolve<PongScreenGameplayViewModel>(),
                container.Resolve<GameSessionsService>()
            ));


            var uiRootVM = container.Resolve<PongUIRootViewModel>();
            _rootUIView.Bind(uiRootVM);
            uiRootVM.OpenMainMenuScreen();
        }
        
        private void SetupPlayer<T>(Block block) where T : InputController
        {
            var inputController = block.GetComponent<InputController>();

            if (!inputController)
            {
                inputController = block.gameObject.AddComponent<T>();
            }
            
            inputController.Bind(block);
        }

        private void SetupAI(Block block, Ball ball)
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