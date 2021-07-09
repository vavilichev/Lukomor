using System;
using UnityEngine;

namespace VavilichevGD.Utils.Attributes {
	[Serializable]
	public class ClassReferenceAttribute : PropertyAttribute {
		public Type type;

		public ClassReferenceAttribute(Type type) {
			this.type = type;
		}
	}
}