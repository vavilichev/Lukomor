using Lukomor.Presentation.Controllers;
using Lukomor.Presentation.Views.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Lukomor.Example.Presentation.Gameplay
{
	public class GameplayWindow : Window<GameplayWindowModel>
	{
		[SerializeField] private Button _buttonRestart;
		
		protected override Controller<GameplayWindowModel> CreateController()
		{
			return new GameplayWindowController();
		}

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
			Model.ReloadGrid.Execute();
		}
	}
}