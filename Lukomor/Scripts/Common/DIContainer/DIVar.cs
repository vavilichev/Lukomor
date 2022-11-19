namespace Lukomor.Common.DIContainer
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

		public bool HasValue => DI.Has<T>();

		private T _value;
	}
}