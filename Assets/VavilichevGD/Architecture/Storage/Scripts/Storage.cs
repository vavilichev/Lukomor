using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace VavilichevGD.Architecture.StorageSystem {
	public sealed class Storage {

		#region EVENTS

		public event Action OnStorageLoadedEvent;
		public event Action OnStorageSaveStartedEvent;
		public event Action OnStorageSaveCompleteEvent;

		#endregion


		#region STATIC VARIABLES

		public static BinaryFormatter formatter {
			get {
				if (_formatter == null)
					_formatter = CreateBinaryFormatter();
				return _formatter;
			}
		}
		private static BinaryFormatter _formatter;
		
		public static Storage instance {
			get {
				if (_instance == null)
					_instance = new Storage(StorageType.FileStorage);
				return _instance;
			}
		}
		private static Storage _instance;
		
		public static bool isInitialized => instance.data != null;

		#endregion
		
		
		private GameData data { get; set; }
		
		
		private IStorageBehavior storageBehavior;

		
		private Storage(StorageType type) {
			switch (type) {
				case StorageType.CloudStorage:
					throw new NotSupportedException("Cloud storage doesn't supported yet");
				default:
					this.storageBehavior = new FileStorageBehavior();
					break;
			}
		}
		
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

		
		
		public void Save() {
			this.OnStorageSaveStartedEvent?.Invoke();
			this.storageBehavior.Save(data);
			this.OnStorageSaveCompleteEvent?.Invoke();
		}

		public void SaveAsync(Action callback = null) {
			this.OnStorageSaveStartedEvent?.Invoke();
			this.storageBehavior.SaveAsync(this.data, callback);
			this.OnStorageSaveCompleteEvent?.Invoke();
		}

		public Coroutine SaveWithRoutine(Action callback = null) {
			this.OnStorageSaveStartedEvent?.Invoke();
			return this.storageBehavior.SaveWithRoutine(this.data, () => {
				callback?.Invoke();
				this.OnStorageSaveStartedEvent?.Invoke();
			});
		}
		
		public void Load() {
			this.data = (GameData) storageBehavior.Load(new GameData());
			this.OnStorageLoadedEvent?.Invoke();
		}

		public void LoadAsync(Action callback) {
			this.storageBehavior.LoadAsync(new GameData(), 
				gameData => {
					this.data = (GameData) gameData;
					callback?.Invoke();
					this.OnStorageLoadedEvent?.Invoke();
				});
		}
		
		public Coroutine LoadWithRoutine(Action callback = null) {
			return this.storageBehavior.LoadWithRoutine(new GameData(), loadedData => {
				this.data = (GameData) loadedData;
				callback?.Invoke();
				this.OnStorageLoadedEvent?.Invoke();
			});
		}
		

		public T Get<T>(string key) {
			return this.data.Get<T>(key);
		}
		
		public T Get<T>(string key, T valueByDefault) {
			return this.data.Get(key, valueByDefault);
		}

		public void Set<T>(string key, T value) {
			this.data.Set(key, value);
		}

		public override string ToString() {
			return this.data.ToString();
		}
	}
}