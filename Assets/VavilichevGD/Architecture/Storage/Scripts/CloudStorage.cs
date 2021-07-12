using System;
using UnityEngine;

namespace VavilichevGD.Architecture.StorageSystem {
	public class CloudStorage : Storage{
		protected override void SaveInternal() {
			throw new NotImplementedException();
		}

		protected override void SaveAsyncInternal(Action callback = null) {
			throw new NotImplementedException();
		}

		protected override Coroutine SaveWithRoutineInternal(Action callback = null) {
			throw new NotImplementedException();
		}

		protected override void LoadInternal() {
			throw new NotImplementedException();
		}

		protected override void LoadAsyncInternal(Action<GameData> callback = null) {
			throw new NotImplementedException();
		}

		protected override Coroutine LoadWithRoutineInternal(Action<GameData> callback = null) {
			throw new NotImplementedException();
		}
	}
}