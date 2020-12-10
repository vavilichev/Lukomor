using UnityEngine.UI;

namespace VavilichevGD.Architecture.UI {
	public interface IUIPopup : IUIElementOnLayer {
		
		bool isPreCached { get; }
		Button buttonClose { get; }
		Button buttonCloseAlt { get; }
		
	}
}