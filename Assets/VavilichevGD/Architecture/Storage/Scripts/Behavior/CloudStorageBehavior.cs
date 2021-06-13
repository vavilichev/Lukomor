using System;
using UnityEngine;

namespace VavilichevGD.Architecture.StorageSystem {
	public sealed class CloudStorageBehavior : IStorageBehavior {
		public void Save(object saveData) {
			throw new NotImplementedException();
		}

		public void SaveAsync(object saveData, Action callback) {
			throw new NotImplementedException();
		}

		public Coroutine SaveWithRoutine(object saveData, Action callback) {
			throw new NotImplementedException();
		}

		public object Load(object saveDataByDefault) {
			throw new NotImplementedException();
		}

		public void LoadAsync(object saveDataByDefault, Action<object> callback) {
			throw new NotImplementedException();
		}

		public Coroutine LoadWithRoutine(object saveDataByDefault, Action<object> callback) {
			throw new NotImplementedException();
		}

		public void LoadAsync(Action<object> callback, object saveDataByDefault) {
			throw new NotImplementedException();
		}
	}
}