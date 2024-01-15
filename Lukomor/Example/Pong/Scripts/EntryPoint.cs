using Lukomor.DI;
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

            var container = new DIContainer();
            PongScreensRegistrations.Register(container);

            var uiRootViewModel = new UIRootViewModel(
                () => container.Resolve<ScreenMainMenuViewModel>(),
                () => container.Resolve<ScreenPauseViewModel>(),
                () => container.Resolve<ScreenResultViewModel>(),
                () => container.Resolve<ScreenGameplayViewModel>()
            );
            
            _rootUIView.Bind(uiRootViewModel);
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