using UnityEngine.UI;

namespace VavilichevGD.Architecture.UserInterface {
	public interface IUIPopup : IUIElementOnLayer {

		bool isPreCached { get; }
		Button[] buttonsClose { get; }

	}
}