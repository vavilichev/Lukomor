using System;
using UnityEngine;

namespace VavilichevGD.Utils.Attributes.ObjectsOfType {
	public class ObjectOfTypeAttributeBase : PropertyAttribute {
		public Type type { get; }
		public bool allowSceneObjects { get; }
 
		public ObjectOfTypeAttributeBase(Type type, bool allowSceneObjects = true)
		{
			this.type = type;
			this.allowSceneObjects = allowSceneObjects;
		}
	}
}