using System;

namespace Lukomor.Common.Utils.Observables
{
	public class ObservableEvent
	{
		public event Action Raised;

		public void Raise()
		{
			Raised?.Invoke();
		}
	}

	public class ObservableEvent<T>
	{
		public event Action<T> Raised;

		public void Raise(T parameter)
		{
			Raised?.Invoke(parameter);
		}
	}
	
	public class ObservableEvent<T, TP>
	{
		public event Action<T, TP> Raised;

		public void Raise(T parameterA, TP parameterB)
		{
			Raised?.Invoke(parameterA, parameterB);
		}
	}
	
	public class ObservableEvent<T, TP, TF>
	{
		public event Action<T, TP, TF> Raised;

		public void Raise(T parameterA, TP parameterB, TF parameterC)
		{
			Raised?.Invoke(parameterA, parameterB, parameterC);
		}
	}
}