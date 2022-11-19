using System;
using System.Threading.Tasks;
using Lukomor.Common.DIContainer;
using Lukomor.Presentation.Common;
using UnityEngine;

namespace Lukomor.Presentation.Views.Windows
{
	public abstract class Window<TWindowViewModel> : View<TWindowViewModel>, IWindow<TWindowViewModel>
		where TWindowViewModel : WindowViewModel
	{
		public event Action<WindowViewModel> Hidden;
		public event Action<WindowViewModel> Destroyed;
		public event Action<bool> BlockInteractingRequested;

		[SerializeField] private Transition _transitionIn = default;
		[SerializeField] private Transition _transitionOut = default;
		
		public bool IsShown { get; private set; }

		protected UserInterface UI => _userInterface.Value;
		
		private DIVar<UserInterface> _userInterface = new DIVar<UserInterface>();

		public void BlockInteractions()
		{
			BlockInteractingRequested?.Invoke(true);
		}

		public void UnlockInteractions()
		{
			BlockInteractingRequested?.Invoke(false);
		}
		
		public async Task<IWindow> Show()
		{
			IsShown = true;
			
			gameObject.SetActive(true);
			transform.SetAsLastSibling();

			if (_transitionIn != null)
			{
				await _transitionIn.Play();
			}

			return this;
		}

		public async Task<IWindow> Hide()
		{
			if (_transitionOut != null)
			{
				await _transitionOut.Play();
			}

			HideInstantly();

			IsShown = false;
			return this;
		}

		public IWindow HideInstantly()
		{
			if (ViewModel.WindowSettings.IsPreCached)
			{
				gameObject.SetActive(false);
			}
			else
			{
				Destroy(gameObject);
			}

			Hidden?.Invoke(ViewModel);
			return this;
		}

		protected virtual void OnDestroy()
		{
			Destroyed?.Invoke(ViewModel);
		}
	}
}