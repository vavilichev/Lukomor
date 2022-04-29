using Lukomor.Presentation.Models;
using VavilichevGD.Utils.Observables;

namespace Lukomor.Example.Presentation.Gameplay
{
	public sealed class GameplayWindowModel : Model
	{
		public ObservableCommand ReloadGrid { get; } = new ObservableCommand();
	}
}