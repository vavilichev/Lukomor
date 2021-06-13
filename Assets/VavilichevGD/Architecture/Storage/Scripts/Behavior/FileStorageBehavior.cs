using System;
using System.Collections;
using System.IO;
using System.Threading;
using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture.StorageSystem {
	public sealed class FileStorageBehavior : IStorageBehavior {

		#region CONSTANTS

		private readonly string savesDirectory = Application.persistentDataPath + "/saves";
		private const string SAVE_FILE_NAME = "GameSave";

		#endregion

		private string filePath { get; }

		public FileStorageBehavior() {
			if (!Directory.Exists(savesDirectory)) 
				Directory.CreateDirectory(savesDirectory);
			this.filePath =  $"{savesDirectory}/{SAVE_FILE_NAME}";
			Debug.Log($"FilePath: {filePath}");
		}

		#region SAVE

		public void Save(object saveData) {
			var file = File.Create(filePath);
			Storage.formatter.Serialize(file, saveData);
			file.Close();
		}

		public void SaveAsync(object saveData, Action callback) {
			var thread = new Thread(() => this.SaveDataTaskThreaded(saveData, callback));
			thread.Start();
		}
		
		public Coroutine SaveWithRoutine(object saveData, Action callback = null) {
			return Coroutines.StartRoutine(this.SaveRoutine(saveData, callback));
		}

		private IEnumerator SaveRoutine(object saveData, Action callback) {
			var threadEnded = false;
			
			this.SaveAsync(saveData, () => {
				threadEnded = true;
			});
			
			while (!threadEnded)
				yield return null;
			
			callback?.Invoke();
		}

		private void SaveDataTaskThreaded(object saveData, Action callback) {
			this.Save(saveData);
			callback?.Invoke();
		}

		#endregion



		#region LOAD

		public object Load(object saveDataByDefault) {
			if (!File.Exists(filePath)) {
				if (saveDataByDefault != null)
					this.Save(saveDataByDefault);
				return saveDataByDefault;
			}

			var file = File.Open(filePath, FileMode.Open);
			var saveData = Storage.formatter.Deserialize(file);
			file.Close();
			return saveData;
		}
		
		public void LoadAsync(object saveDataByDefault, Action<object> callback) {
			var thread = new Thread(() => LoadDataTaskThreaded(saveDataByDefault, callback));
			thread.Start();
		}
		
		public Coroutine LoadWithRoutine(object saveDataByDefault, Action<object> callback = null) {
			return Coroutines.StartRoutine(this.LoadRoutine(saveDataByDefault, callback));
		}

		private IEnumerator LoadRoutine(object saveDataByDefault, Action<object> callback) {
			var threadEnded = false;
			var gameData = default(object);
			
			this.LoadAsync(saveDataByDefault, (loadedData) => {
				threadEnded = true;
				gameData = loadedData;
			});
			
			while (!threadEnded)
				yield return null;
			
			callback?.Invoke(gameData);
		}
		
		private void LoadDataTaskThreaded(object saveDataByDefault, Action<object> callback) {
			var saveData = this.Load(saveDataByDefault);
			callback?.Invoke(saveData);
		}

		#endregion

	}
}