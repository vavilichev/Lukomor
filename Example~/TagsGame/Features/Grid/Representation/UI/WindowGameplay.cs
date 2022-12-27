using Lukomor.Presentation.Views.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Lukomor.TagsGame.UI
{
	public class WindowGameplay : Window<WindowGameplayViewModel>
	{
		[SerializeField] private Button _buttonRestart;
		
		public override void Subscribe()
		{
			base.Subscribe();
			
			_buttonRestart.onClick.AddListener(OnRestartButtonClick);
		}
		
		public override void Unsubscribe()
		{
			base.Unsubscribe();
			
			_buttonRestart.onClick.RemoveListener(OnRestartButtonClick);
		}

		private void OnRestartButtonClick()
		{
			ViewModel.RequestReload();
		}
	}
}