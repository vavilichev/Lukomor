using System;
using System.Collections.Generic;
using UnityEngine;

namespace VavilichevGD.Architecture.StorageSystem {
	[Serializable]
	public sealed class GameData : ISerializationCallbackReceiver {


		public List<string> keys;
		public List<object> values;

		public Dictionary<string, object>  dataMap = new Dictionary<string, object>();

		
		public void OnBeforeSerialize() {
			keys.Clear();
			values.Clear();

			foreach (var item in dataMap) {
				keys.Add(item.Key);
				values.Add(item.Value);
			}
		}

		public void OnAfterDeserialize()
		{
			dataMap = new Dictionary<string, object>();

			for (int i = 0; i != Math.Min(keys.Count, values.Count); i++)
				dataMap.Add(keys[i], values[i]);
		}
		

		public T Get<T>(string key) {
			this.dataMap.TryGetValue(key, out var foundValue);
			if (foundValue != null)
				return (T) foundValue;
			return default;
		}
		
		public T Get<T>(string key, T valueByDefault) {
			this.dataMap.TryGetValue(key, out var value);
			if (value != null)
				return (T) value;

			this.Set(key, valueByDefault);
			return valueByDefault;
		}

		public void Set<T>(string key, T newValue) {
			this.dataMap[key] = newValue;
		}

		public override string ToString() {
			var line = "";
			foreach (var pair in this.dataMap) 
				line += $"Pair: {pair.Key} - {pair.Value}\n";
			return line;
		}
		
	}
}