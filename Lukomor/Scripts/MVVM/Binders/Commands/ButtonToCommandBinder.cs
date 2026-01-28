using UnityEngine;
using UnityEngine.UI;

namespace Lukomor.MVVM.Binders
{
    [RequireComponent(typeof(Button))]
    public class ButtonToCommandBinder : CommandBinder
    {
        [SerializeField] private Button _button;

        private void OnEnable()
        {
            _button.onClick.AddListener(ExecuteCommand);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ExecuteCommand);
        }
    }
}