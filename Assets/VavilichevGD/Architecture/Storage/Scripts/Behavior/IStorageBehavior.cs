using System;
using UnityEngine;

namespace VavilichevGD.Architecture.StorageSystem {
	public interface IStorageBehavior {
		void Save(object saveData);
		void SaveAsync(object saveData, Action callback);
		Coroutine SaveWithRoutine(object saveData, Action callback);
		object Load(object saveDataByDefault);
		void LoadAsync(object saveDataByDefault, Action<object> callback);
		Coroutine LoadWithRoutine(object saveDataByDefault, Action<object> callback);
	}
}