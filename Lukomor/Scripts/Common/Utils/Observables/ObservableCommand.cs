using System;

namespace Lukomor.Common.Utils.Observables {
	public class ObservableCommand {
		public event Action Requested;

		public void Execute()
		{
			Requested?.Invoke();
		}
	}
	
	public class ObservableCommand<T>
	{
		public event Action<T> Requested;

		public void Execute(T value)
		{
			Requested?.Invoke(value);
		}
	}
}