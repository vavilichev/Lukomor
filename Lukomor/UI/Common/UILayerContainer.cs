using UnityEngine;

namespace Lukomor.UI.Common {
	public class UILayerContainer : MonoBehaviour {
		[SerializeField] private UILayer layer;

		public UILayer Layer => layer;
	}
}