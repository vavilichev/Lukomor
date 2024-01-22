using Lukomor.DI;
using Lukomor.MVVM;
using UnityEngine;

namespace Lukomor.Example.Pong
{
    public class PongGameplayEntryPoint : MonoBehaviour
    {
        private const int GAME_SCORE_LIMIT = 3;
        
        [SerializeField] private BlockView _leftBlock;
        [SerializeField] private BlockView _rightBlock;
        [SerializeField] private View _ball;
        [SerializeField] private View _gateLeft;
        [SerializeField] private View _gateRight;
        [SerializeField] private View _rootUIView;

        public void Process(DIContainer container, PongGameplayMode gameplayMode)
        {
            SetupPlayers(gameplayMode);
            RegisterViewModels(container);
            BindViewModels(container);
            OpenDefaultScreen(container);
        }

        private void SetupPlayers(PongGameplayMode mode)
        {
            SetupPlayer<FirstPlayerInputController>(_leftBlock);

            if (mode == PongGameplayMode.OnePlayer)
            {
                SetupAI(_rightBlock, _ball.transform);
            }
            else
            {
                SetupPlayer<SecondPlayerInputController>(_rightBlock);
            }
        }
        
        private static void SetupPlayer<T>(BlockView block) where T : InputController
        {
            var inputController = block.GetComponent<InputController>();

            if (inputController)
            {
                Destroy(inputController);
            }
            
            inputController = block.gameObject.AddComponent<T>();
            
            inputController.Bind(block);
        }

        private static void SetupAI(BlockView block, Transform targetTransform)
        {
            var inputController = block.GetComponent<AIInputController>();

            if (!inputController)
            {
                inputController = block.gameObject.AddComponent<AIInputController>();
            }
            
            inputController.Bind(block, targetTransform);
        }

        private void RegisterViewModels(DIContainer container)
        {
            container.RegisterSingleton(c => new GameSessionService(GAME_SCORE_LIMIT));
            container.RegisterSingleton(c => new BallViewModel(c.Resolve<GameSessionService>()));
            
            container.RegisterSingleton(PongPlayer.One.ToString(),
                c => new GateViewModel(c.Resolve<GameSessionService>(), PongPlayer.One));
            container.RegisterSingleton(PongPlayer.Two.ToString(),
                c => new GateViewModel(c.Resolve<GameSessionService>(), PongPlayer.Two));
            
            container.RegisterSingleton(PongPlayer.One.ToString(),
                c => new BlockViewModel(c.Resolve<GameSessionService>()));
            container.RegisterSingleton(PongPlayer.Two.ToString(),
                c => new BlockViewModel(c.Resolve<GameSessionService>()));

            container.RegisterSingleton(
                c => new ScreenGameplayViewModel(c.Resolve<GameSessionService>()));
            container.RegisterSingleton(
                c => new ScreenPauseViewModel(c.Resolve<GameSessionService>(), c.Resolve<ScenesService>()));
            container.RegisterSingleton(
                c => new ScreenGameOverViewModel(c.Resolve<GameSessionService>(),c.Resolve<ScenesService>()));
            container.RegisterSingleton(c => new ScreenRoundOverViewModel(
                c.Resolve<GameSessionService>()));
            
            container.RegisterSingleton(c => new UIRootGameplayViewModel(
                () => container.Resolve<ScreenPauseViewModel>(),
                () => container.Resolve<ScreenGameOverViewModel>(),
                () => container.Resolve<ScreenGameplayViewModel>(),
                () => container.Resolve<ScreenRoundOverViewModel>(),
                c.Resolve<GameSessionService>()
            ));
        }

        private void BindViewModels(DIContainer container)
        {
            _ball.GetComponent<View>().Bind(container.Resolve<BallViewModel>());
            _gateLeft.Bind(container.Resolve<GateViewModel>(PongPlayer.One.ToString()));
            _gateRight.Bind(container.Resolve<GateViewModel>(PongPlayer.Two.ToString()));
            _leftBlock.GetComponent<View>().Bind(container.Resolve<BlockViewModel>(PongPlayer.One.ToString()));
            _rightBlock.GetComponent<View>().Bind(container.Resolve<BlockViewModel>(PongPlayer.Two.ToString()));
            _rootUIView.Bind(container.Resolve<UIRootGameplayViewModel>());
        }

        private void OpenDefaultScreen(DIContainer container)
        {
            var uiRoot = container.Resolve<UIRootGameplayViewModel>();
            
            uiRoot.OpenGameplayScreen();
        }
    }
}