using System.Collections.Generic;
using Lukomor.Presentation.Controllers;
using Lukomor.Presentation.Models;
using UnityEngine;

namespace Lukomor.Presentation.Views {
	public abstract class View<TModel> : MonoBehaviour, IView where TModel : Model, new()
	{
		public TModel Model { get; protected set; }
		public Controller<TModel> Controller { get; private set; }
		public bool IsReady { get; private set; }
		
		private readonly Dictionary<string, object> _payloadsMap = new Dictionary<string, object>();

		public void AddPayload(string key, object payloadEntry) {
			_payloadsMap[key] = payloadEntry;
			
			OnPayloadAdded(key);
		}

		public bool PayloadExists(string key)
		{
			return _payloadsMap.ContainsKey(key);
		}

		public TValue GetPayload<TValue>(string key) {
			TValue payloadEntry = default;
			
			_payloadsMap.TryGetValue(key, out object value);

			if (value != null) {
				payloadEntry = (TValue)value;
			}

			return payloadEntry;
		}

		public TValue GetAndRemovePayload<TValue>(string key)
		{
			var payload = GetPayload<TValue>(key);

			RemovePayload(key);

			return payload;
		}

		public void RemovePayload(string key) {
			if (_payloadsMap.ContainsKey(key)) {
				_payloadsMap.Remove(key);
			}
		}

		public void RemoveAllPayloads() {
			_payloadsMap.Clear();
		}
		
		protected virtual void Awake()
		{
			InstallModelAndController();
			MarkAsReady();
		}

		protected void InstallModelAndController()
		{
			Model = new TModel();
			Controller = CreateController();
			
			Controller?.SetModel(Model);
		}
		
		protected void MarkAsReady()
		{
			IsReady = true;
		}

		protected abstract Controller<TModel> CreateController();

		protected virtual void OnPayloadAdded(string payloadKey) { }
	}
}