namespace VavilichevGD.Utils.Observables {
	public sealed class ObservableVariable<T> {
		public delegate void ChangeVariableHandler(T newValue);
		public event ChangeVariableHandler Changed;
		
		public T Value {
			get => value;
			set {
				if (!Equals(this.value, value)) {
					this.value = value;
					
					Changed?.Invoke(value);
				}
			}
		}

		private T value;

		public ObservableVariable() {
			value = default;
		}

		public ObservableVariable(T valueByDefault) {
			Value = valueByDefault;
		}

		public void ForceChangedEventInvoke() {
			Changed?.Invoke(Value);
		}
	}
}