using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lukomor.DIContainer
{
	public static class DI
	{
		private static Dictionary<Type, object> _bindedObjects = new Dictionary<Type, object>();
		
		public static void Bind<T>(T value) where T : class
		{
			var type = typeof(T);

			_bindedObjects.Add(type, value);
		}

		public static void Unbind<T>(T value) where T : class
		{
			var type = typeof(T);

			_bindedObjects.Remove(type);
		}

		public static T Get<T>() where T : class
		{
			var type = typeof(T);

			_bindedObjects.TryGetValue(type, out var foundObject);

			if (foundObject == null)
			{
				Debug.LogError($"DI: Could not find bindable object with key '{type}'.");
				
				return null;
			}

			return (T)foundObject;
		}

		public static T[] GetAll<T>() where T : class
		{
			var type = typeof(T);
			var allFoundObjectsBindable = _bindedObjects.Values.Where(value => type.IsInstanceOfType(value)).ToArray();
			var allFoundObjects = Array.ConvertAll(allFoundObjectsBindable, item => (T)item);

			return allFoundObjects;
		}
	}
}
