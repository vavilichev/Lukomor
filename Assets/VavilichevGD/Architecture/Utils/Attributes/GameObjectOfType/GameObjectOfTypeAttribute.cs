using System;
using UnityEngine;

namespace VavilichevGD.Utils.Attributes {
	
	[Serializable]
	[AttributeUsage(AttributeTargets.Field)]
	public class GameObjectOfTypeAttribute : PropertyAttribute {
		public Type type { get; }

		/// <summary>
		/// This attribute uses with GameObject only. You can place game objects with different types of scripts. Even interfaces.
		/// </summary>
		/// <param name="type">Class or interface type</param>
		public GameObjectOfTypeAttribute(Type type) {
			this.type = type;
		}
	}
}