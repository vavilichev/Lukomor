using System;

namespace VavilichevGD.Utils.Attributes.ObjectsOfType {
	public class ScriptableObjectOfTypeAttribute : ObjectOfTypeAttributeBase {
		public ScriptableObjectOfTypeAttribute(Type type, bool allowSceneObjects = true) : base(type, allowSceneObjects) {
		}
	}
}