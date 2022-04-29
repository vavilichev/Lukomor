using Lukomor.Presentation.Controllers;
using Lukomor.Presentation.Models;

namespace Lukomor.Presentation.Views.Widgets {
	public abstract class Widget<TModel> : View<TModel> where TModel : Model, new()
	{
		protected sealed override void Awake()
		{
			InstallModelAndController();
			Install();
			MarkAsReady();
		}

		protected virtual void OnEnable() {
			Subscribe(Model);
			Controller?.Subscribe(Model);
			
			Refresh(Model);
			Controller?.Refresh(Model);
		}

		protected virtual void OnDisable() {
			Unsubscribe(Model);
			Controller?.Unsubscribe(Model);
		}
		
		public virtual void Hide()
		{
			HideInstantly();
		}

		public void HideInstantly()
		{
			gameObject.SetActive(false);
		}

		public virtual void Show()
		{
			ShowInstantly();
		}

		public void ShowInstantly()
		{
			gameObject.SetActive(true);
		}

		protected virtual void Install() { }
		protected virtual void Refresh(TModel model) { }
		protected virtual void Subscribe(TModel model) { }
		protected virtual void Unsubscribe(TModel model) { }
	}

	public abstract class Widget : Widget<Model>
	{
		protected sealed override Controller<Model> CreateController()
		{
			return null;
		}
	}
}