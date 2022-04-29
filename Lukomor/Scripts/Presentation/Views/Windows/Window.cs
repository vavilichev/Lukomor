using System;
using System.Threading.Tasks;
using Lukomor.Presentation.Common;
using Lukomor.Presentation.Controllers;
using Lukomor.Presentation.Models;
using UnityEngine;

namespace Lukomor.Presentation.Views.Windows {
	public abstract class Window<TModel> : View<TModel>, IWindow where TModel : Model, new()
	{
		public event Action<IWindow> Destroyed;
		public event Action<IWindow> Hidden;
		public event Action<bool> BlockInteractingRequested;

		[SerializeField] private UILayer _targetLayer;
		[SerializeField] private Transition _transitionIn = default;
		[SerializeField] private Transition _transitionOut = default;
		[SerializeField] private bool _isPreCached = true;
		[SerializeField] private bool _openedByDefault = true;
		[SerializeField] private bool _closeWhenUnfocused = true;

		public GameObject GameObject => gameObject;
		public bool IsPreCached => _isPreCached;
		public bool OpenedByDefault => _openedByDefault;
		public bool CloseWhenUnfocused => _closeWhenUnfocused;
		public bool IsActive => gameObject.activeInHierarchy;
		public UILayer TargetLayer => _targetLayer;
		public UserInterface UI { get; set; }

		protected sealed override void Awake()
		{
			base.Awake();
		}

		public virtual void Install() { }

		public virtual void Refresh()
		{
			Controller?.Refresh(Model);
		}

		public virtual void Subscribe()
		{
			Controller?.Subscribe(Model);
		}

		public virtual void Unsubscribe()
		{
			Controller?.Unsubscribe(Model);
		}

		public void BlockInteractions() {
			BlockInteractingRequested?.Invoke(true);
		}

		public void UnlockInteractions() {
			BlockInteractingRequested?.Invoke(false);
		}

		public async Task<IWindow> Show() {
			gameObject.SetActive(true);
			transform.SetAsLastSibling();
			
			if (_transitionIn != null) {
				await _transitionIn.Play();
			}

			return this;
		}

		public async Task<IWindow> Hide() {
			if (_transitionOut != null) {
				await _transitionOut.Play();
			}

			HideInstantly();

			return this;
		}

		public IWindow HideInstantly() {
			if (IsPreCached) {
				gameObject.SetActive(false);
			}
			else {
				Destroy(gameObject);
			}

			Hidden?.Invoke(this);
			return this;
		}

		protected virtual void OnDestroy() {
			Destroyed?.Invoke(this);
		}
	}

	public abstract class Window : Window<Model>
	{
		protected sealed override Controller<Model> CreateController()
		{
			return null;
		}
	}
}