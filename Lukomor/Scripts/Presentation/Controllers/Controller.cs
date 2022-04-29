using Lukomor.Presentation.Models;

namespace Lukomor.Presentation.Controllers {
	public abstract class Controller<T> where T : Model {
		public T Model { get; private set; }

		public void SetModel(T model)
		{
			Model = model;
		}
		
		public abstract void Refresh(T model);
		public virtual void Subscribe(T model) { }
		public virtual void Unsubscribe(T model) { }
	}

	public abstract class Controller : Controller<Model> { }
}