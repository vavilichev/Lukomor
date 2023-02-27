using System;
using System.Threading.Tasks;
using Lukomor.UI.Common;
using UnityEngine;

namespace Lukomor.UI
{
	public abstract class Window<TWindowViewModel> : View<TWindowViewModel>, IWindow<TWindowViewModel>
		where TWindowViewModel : WindowViewModel
	{
		public event Action<WindowViewModel> Hidden;
		public event Action<WindowViewModel> Destroyed;
		public event Action<bool> BlockInteractingRequested;

		[SerializeField] private Transition transitionIn = default;
		[SerializeField] private Transition transitionOut = default;
		
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
			gameObject.SetActive(true);
			transform.SetAsLastSibling();

			if (transitionIn != null)
			{
				await transitionIn.Play();
			}

			return this;
		}

		public async Task<IWindow> Hide()
		{
			if (transitionIn != null && transitionIn.IsPlaying)
			{
				return this;
			}
			
			if (transitionOut != null)
			{
				if (transitionOut.IsPlaying)
				{
					return this;
				}
				
				await transitionOut.Play();
			}

			HideInstantly();

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