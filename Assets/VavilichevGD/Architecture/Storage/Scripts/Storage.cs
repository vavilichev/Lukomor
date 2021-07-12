using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace VavilichevGD.Architecture.StorageSystem {
	public abstract class Storage {

		#region EVENTS

		public event Action OnStorageSaveStartedEvent;
		public event Action OnStorageSaveCompleteEvent;
		public event Action<GameData> OnStorageLoadedEvent;

		#endregion
		
		
		public static BinaryFormatter formatter {
			get {
				if (_formatter == null)
					_formatter = CreateBinaryFormatter();
				return _formatter;
			}
		}
		private static BinaryFormatter _formatter;

		public GameData data { get; protected set; }
		
		
		
		private static BinaryFormatter CreateBinaryFormatter() {
			var createdFormatter = new BinaryFormatter();
			var selector = new SurrogateSelector();
			
			var vector3Surrogate = new Vector3SerializationSurrogate();
			var vector2Surrogate = new Vector2SerializationSurrogate();
			var quaternionSurrogate = new QuaternionSerializationSurrogate();
			
			selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
			selector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), vector2Surrogate);
			selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSurrogate);
			
			createdFormatter.SurrogateSelector = selector;

			return createdFormatter;
		}


		#region SAVE

		public void Save() {
			OnStorageSaveStartedEvent?.Invoke();
			SaveInternal();
			OnStorageSaveCompleteEvent?.Invoke();
		}

		protected abstract void SaveInternal();

		public void SaveAsync(Action callback = null) {
			OnStorageSaveStartedEvent?.Invoke();
			SaveAsyncInternal(callback);
			OnStorageSaveCompleteEvent?.Invoke();
		}

		protected abstract void SaveAsyncInternal(Action callback = null);

		public Coroutine SaveWithRoutine(Action callback = null) {
			OnStorageSaveStartedEvent?.Invoke();
			return SaveWithRoutineInternal(() => {
				callback?.Invoke();
				OnStorageSaveCompleteEvent?.Invoke();
			});
		}

		protected abstract Coroutine SaveWithRoutineInternal(Action callback = null);

		#endregion



		#region LOAD

		public void Load() {
			LoadInternal();
			OnStorageLoadedEvent?.Invoke(data);
		}

		protected abstract void LoadInternal();

		public void LoadAsync(Action<GameData> callback = null) {
			LoadAsyncInternal(loadedData => {
				callback?.Invoke(data);
				OnStorageLoadedEvent?.Invoke(data);
			});
		}

		protected abstract void LoadAsyncInternal(Action<GameData> callback = null);
		
		public Coroutine LoadWithRoutine(Action<GameData> callback = null) {
			return LoadWithRoutineInternal(loadedData => {
				callback?.Invoke(data);
				OnStorageLoadedEvent?.Invoke(data);
			});
		}

		protected abstract Coroutine LoadWithRoutineInternal(Action<GameData> callback = null);

		#endregion
		
		
		

		public T Get<T>(string key) {
			return data.Get<T>(key);
		}
		
		public T Get<T>(string key, T valueByDefault) {
			return data.Get(key, valueByDefault);
		}

		public void Set<T>(string key, T value) {
			this.data.Set(key, value);
		}

		public override string ToString() {
			return this.data.ToString();
		}
		
	}
}