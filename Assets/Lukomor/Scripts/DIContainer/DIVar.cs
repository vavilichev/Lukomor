namespace Lukomor.DIContainer
{
	public sealed class DIVar<T> where T : class
	{
		public T Value {
			get
			{
				if (_value == null)
				{
					_value = DI.Get<T>();
				}

				return _value;
			}
		}

		private T _value;
	}
}