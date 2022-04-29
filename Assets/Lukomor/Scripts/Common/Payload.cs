namespace Lukomor.Common
{
	public readonly struct Payload
	{
		public string Key { get; }
		public object Value { get; }

		public Payload(string key, object value)
		{
			Key = key;
			Value = value;
		}
	}
}