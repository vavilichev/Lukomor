using System;

namespace VavilichevGD.Utils.Observables
{
	public class ObservableEvent
	{
		public event Action Requested;

		public void Raise()
		{
			Requested?.Invoke();
		}
	}

	public class ObservableEvent<T>
	{
		public event Action<T> Requested;

		public void Raise(T parameter)
		{
			Requested?.Invoke(parameter);
		}
	}
	
	public class ObservableEvent<T, TP>
	{
		public event Action<T, TP> Requested;

		public void Raise(T parameterA, TP parameterB)
		{
			Requested?.Invoke(parameterA, parameterB);
		}
	}
	
	public class ObservableEvent<T, TP, TF>
	{
		public event Action<T, TP, TF> Requested;

		public void Raise(T parameterA, TP parameterB, TF parameterC)
		{
			Requested?.Invoke(parameterA, parameterB, parameterC);
		}
	}
}