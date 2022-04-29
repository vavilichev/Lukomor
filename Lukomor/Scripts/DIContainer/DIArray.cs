namespace Lukomor.DIContainer
{
	public sealed class DIArray<T> where T : class
	{
		public T[] Values
		{
			get
			{
				if (_values == null)
				{
					_values = DI.GetAll<T>();
				}

				return _values;
			}
		}

		private T[] _values;
	}
}