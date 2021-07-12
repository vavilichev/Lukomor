using System;
using System.Collections;
using System.IO;
using System.Threading;
using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture.StorageSystem {
	public sealed class FileStorage : Storage{

		public string filePath { get; }
		
		public FileStorage(string fileName) {
			var folder = "Saves";
			var folderPath = $"{Application.persistentDataPath}/{folder}";
			if (!Directory.Exists(folderPath)) 
				Directory.CreateDirectory(folderPath);

			filePath = $"{folderPath}/{fileName}";
		}


		#region SAVE

		protected override void SaveInternal() {
			var file = File.Create(filePath);
			formatter.Serialize(file, data);
			file.Close();
		}

		protected override void SaveAsyncInternal(Action callback = null) {
			var thread = new Thread(() => SaveDataTaskThreaded(callback));
			thread.Start();
		}
		
		private void SaveDataTaskThreaded(Action callback) {
			Save();
			callback?.Invoke();
		}
		
		protected override Coroutine SaveWithRoutineInternal(Action callback = null) {
			return Coroutines.StartRoutine(SaveRoutine(callback));

		}
		
		private IEnumerator SaveRoutine(Action callback) {
			var threadEnded = false;
			
			SaveAsync(() => {
				threadEnded = true;
			});
			
			while (!threadEnded)
				yield return null;
			
			callback?.Invoke();
		}

		#endregion



		#region LOAD
		
		protected override void LoadInternal() {
			if (!File.Exists(filePath)) {
				var gameDataByDefault = new GameData();
				data = gameDataByDefault;
				Save();
			}

			var file = File.Open(filePath, FileMode.Open);
			data = (GameData) formatter.Deserialize(file);
			file.Close();
		}

		
		protected override void LoadAsyncInternal(Action<GameData> callback = null) {
			var thread = new Thread(() => LoadDataTaskThreaded(callback));
			thread.Start();
		}
		
		private void LoadDataTaskThreaded(Action<GameData> callback) {
			Load();
			callback?.Invoke(data);
		}
		

		
		protected override Coroutine LoadWithRoutineInternal(Action<GameData> callback = null) {
			return Coroutines.StartRoutine(LoadRoutine(callback));

		}
		
		private IEnumerator LoadRoutine(Action<GameData> callback) {
			var threadEnded = false;
			var gameData = new GameData();
			
			LoadAsync((loadedData) => {
				threadEnded = true;
			});
			
			while (!threadEnded)
				yield return null;
			
			callback?.Invoke(gameData);
		}

		#endregion

		
	}
}