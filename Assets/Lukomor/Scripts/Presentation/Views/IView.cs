namespace Lukomor.Presentation.Views
{
	public interface IView
	{
		bool IsReady { get; }
		
		void AddPayload(string key, object payload);
		T GetPayload<T>(string key);
		T GetAndRemovePayload<T>(string key);
		bool PayloadExists(string key);
		void RemovePayload(string key);
		void RemoveAllPayloads();
	}
}