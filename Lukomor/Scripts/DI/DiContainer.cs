using System;
using System.Collections.Generic;

namespace Lukomor.DI
{
	public sealed class DIContainer : IDIContainer
	{
		private readonly Dictionary<Type, object> _instances = new();
		private readonly IDIContainer _parentContainer;
		public bool IsRoot => _parentContainer == null;

		public DIContainer() { }

		public DIContainer(IDIContainer parentContainer)
		{
			_parentContainer = parentContainer;
		}

		public void Bind<T>(T instance) where T : class
		{
			var type = typeof(T);

			if (_instances.ContainsKey(type))
			{
				throw new Exception($"You cannot add the same type of instance to container twice. Type: {type}");
			}

			_instances[type] = instance;
		}

		public T Resolve<T>() where T : class
		{
			var type = typeof(T);

			if (_instances.TryGetValue(type, out var foundInstance))
			{
				return (T) foundInstance;
			}
			
			if (!IsRoot)
			{
				return _parentContainer.Resolve<T>();
			}

			throw new Exception($"There is no instance of type registered. Type: {type}");
		}

		public void Dispose()
		{
			_instances.Clear();
		}
	}
}
