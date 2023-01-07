using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lukomor.Common.DIContainer
{
	public static class DI
	{
		private static Dictionary<Type, object> _bindedObjects = new Dictionary<Type, object>();
		
		public static void Bind<T>(T value) where T : class
		{
			var type = typeof(T);

			if (_bindedObjects.ContainsKey(type))
			{
				Debug.LogWarning($"Adding duplicate of object of type {type}. Old object was rewritten.");
			}

			_bindedObjects[type] = value;
		}

		public static void Unbind<T>() where T : class
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

		public static bool TryGet<T>(out T instance)
		{
			instance = default;
			var type = typeof(T);

			bool result = _bindedObjects.TryGetValue(type, out var foundObject);

			if (result)
			{
				instance = (T) foundObject;
			}

			return result;
		}

		public static bool Has<T>()
		{
			var type = typeof(T);

			return _bindedObjects.ContainsKey(type);
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
