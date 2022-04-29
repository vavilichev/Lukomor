using System;
using System.Collections.Generic;
using System.Linq;
using VavilichevGD.Tools.Logging;

namespace Lukomor.Common
{
	public abstract class ServiceLocator<T>
	{
		private readonly Dictionary<Type, T> _servicesMap;

		public ServiceLocator()
		{
			_servicesMap = new Dictionary<Type, T>();
		}

		public T Register(T entity)
		{
			var type = entity.GetType();

			if (_servicesMap.ContainsKey(type))
			{
				Log.PrintError($"ServiceLocator ({typeof(T).Name}): Already contain key of type {type.Name}");
				return default;
			}

			_servicesMap[type] = entity;
			
			return entity;
		}

		public T[] Register(T[] entities)
		{
			var count = entities.Length;

			for (int i = 0; i < count; i++)
			{
				Register(entities[i]);
			}

			return entities;
		}

		public void Unregister<TP>(TP entity) where TP : T
		{
			var type = typeof(TP);

			if (_servicesMap.ContainsKey(type))
			{
				Log.PrintWarning($"ServiceLocator ({typeof(T).Name}): Doesn't contain key of type {type.Name}");
				return;
			}

			_servicesMap.Remove(type);
		}

		public TP Get<TP>() where TP : T
		{
			var type = typeof(TP);
			TP result = default;

			_servicesMap.TryGetValue(type, out var foundValue);

			if (foundValue != null)
			{
				result = (TP)foundValue;
			}

			return result;
		}

		public T[] GetAll()
		{
			return _servicesMap.Values.ToArray();
		}
	}
}