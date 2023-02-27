using System.Collections.Generic;
using Lukomor.Common.Utils.Async;
using UnityEngine;
using UnityEngine.UI;

namespace Lukomor.UI
{
	public abstract class DialogWindow<TWindowViewModel> : Window<TWindowViewModel> where TWindowViewModel : WindowViewModel
	{
		[Space] 
		[SerializeField] protected List<Button> closeButtons;

		private void OnEnable()
		{
			foreach (Button closeButton in closeButtons)
			{
				closeButton.onClick.AddListener(OnCloseButtonClick);
			}
			
			OnEnableInternal();
		}

		private void OnDisable()
		{
			foreach (Button closeButton in closeButtons)
			{
				closeButton.onClick.RemoveListener(OnCloseButtonClick);
			}
			
			OnDisableInternal();
		}
		
		protected virtual void OnEnableInternal() { }
		protected virtual void OnDisableInternal() { }

		protected virtual void OnCloseButtonClick()
		{
			Hide().RunAsync();
		}
	}
}