using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VavilichevGD.Utils.Attributes.ObjectsOfType {
	[CustomPropertyDrawer(typeof(ScriptableObjectOfTypeAttribute))]
	public class ScriptableObjectOfTypeDrawer : ObjectOfTypeDrawerBase {
		protected override Type GetCurrentObjectType() {
			Type resultType = null;
			
			if (HasObjectType<ScriptableObject>()) {
				resultType = typeof(ScriptableObject);
			}

			return resultType;
		}

		protected override Type GetRequiredObjectType() {
			return typeof(ScriptableObject);
		}

		protected override bool IsValidObject(Object o, Type requiredType) {
			bool result = false;
			
			var scrOb = o as ScriptableObject;

			if (scrOb != null) {
				var currentType = scrOb.GetType();
				
				result = requiredType.IsAssignableFrom(currentType);
			}

			return result;
		}
	}
}