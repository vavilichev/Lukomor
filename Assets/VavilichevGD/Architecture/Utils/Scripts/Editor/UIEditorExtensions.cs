using System;
using System.Collections.Generic;

namespace VavilichevGD.Architecture.UserInterface.Extensions {
	public static class UIEditorExtensions {
		
		public static bool IsArrayOrList(this Type type) {
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) || type.IsArray;
		}
	}
	
}