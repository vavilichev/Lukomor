using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VavilichevGD.Utils.Attributes.ObjectsOfType {
	[CustomPropertyDrawer(typeof(GameObjectOfTypeAttribute))]
	public class GameObjectOfTypeDrawer : ObjectOfTypeDrawerBase {
		protected override Type GetCurrentObjectType() {
			Type resultType = null;
			
			if (HasObjectType<GameObject>()) {
				resultType = typeof(GameObject);
			}
			
			return resultType;
		}

		protected override Type GetRequiredObjectType() {
			return typeof(GameObject);
		}

		protected override bool IsValidObject(Object o, Type requiredType) {
			bool result = false;
			
			var go = o as GameObject;

			if (go != null) {
				result = go.GetComponent(requiredType) != null;
			}

			return result;
		}
	}
}