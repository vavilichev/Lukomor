using Lukomor.Presentation.Views.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Lukomor.TagsGame.Preview.Presentation.UI
{
    public class WindowPreview : Window<WindowPreviewViewModel>
    {
        [SerializeField] private Button _btnContinue;

        public override void Subscribe()
        {
            base.Subscribe();

            _btnContinue.onClick.AddListener(OnContinueButtonClick);
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            _btnContinue.onClick.RemoveListener(OnContinueButtonClick);
        }

        private void OnContinueButtonClick()
        {
            ViewModel.RequestContinue();
        }
    }
}
