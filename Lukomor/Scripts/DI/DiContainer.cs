using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lukomor.DI
{
	public sealed class DiContainer
	{
		private readonly Dictionary<Type, object> bindObjects = new Dictionary<Type, object>();
		
		public void Bind<T>(T value) where T : class
		{
			var type = typeof(T);

			if (bindObjects.ContainsKey(type))
			{
				Debug.LogWarning($"Adding duplicate of object of type {type}. Old object was overwritten.");
			}

			bindObjects[type] = value;
		}

		public void Unbind<T>() where T : class
		{
			var type = typeof(T);

			bindObjects.Remove(type);
		}
		
		public T Get<T>() where T : class
		{
			var type = typeof(T);

			bindObjects.TryGetValue(type, out var foundObject);

			if (foundObject == null)
			{
				Debug.LogError($"DI: Could not find bindable object with key '{type}'.");
				
				return null;
			}

			return (T)foundObject;
		}

		public bool TryGet<T>(out T instance)
		{
			instance = default;
			var type = typeof(T);

			bool result = bindObjects.TryGetValue(type, out var foundObject);

			if (result)
			{
				instance = (T) foundObject;
			}

			return result;
		}

		public bool Has<T>()
		{
			var type = typeof(T);

			return bindObjects.ContainsKey(type);
		}

		public T[] GetAll<T>() where T : class
		{
			var type = typeof(T);
			var allFoundObjectsBindable = bindObjects.Values.Where(value => type.IsInstanceOfType(value)).ToArray();
			var allFoundObjects = Array.ConvertAll(allFoundObjectsBindable, item => (T)item);

			return allFoundObjects;
		}
	}
}
