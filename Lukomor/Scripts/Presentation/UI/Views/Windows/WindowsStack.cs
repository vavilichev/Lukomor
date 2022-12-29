using System;
using System.Collections.Generic;
using System.Linq;

namespace Lukomor.Presentation.Views.Windows {
	public class WindowsStack {
		public int Length => _windowsQueue.Count;

		private List<Type> _windowsQueue;

		public WindowsStack() {
			_windowsQueue = new List<Type>();
		}

		public void Push(Type windowType) {
			_windowsQueue.Add(windowType);
		}

		public Type Pop() {
			Type result = null;
			
			if (_windowsQueue.Any()) {
				var lastIndex = _windowsQueue.Count - 1;
				
				result =  _windowsQueue[lastIndex];

				if (!typeof(IHomeWindow).IsAssignableFrom(result)) {
					_windowsQueue.RemoveAt(lastIndex);
				}
			}

			return result;
		}

		public void Clear() {
			_windowsQueue.Clear();
		}

		public void RemoveLast(Type type)
		{
			if (_windowsQueue.Contains(type))
			{
				var lastIndexOf = _windowsQueue.LastIndexOf(type);
				
				_windowsQueue.RemoveAt(lastIndexOf);
			}
		}

		public Type GetLast()
		{
			return _windowsQueue.Last();
		}
	}
}