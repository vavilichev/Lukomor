using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VavilichevGD.Architecture.StorageSystem.Example {
	[Serializable]
	public sealed class DummyRepoEntity : IRepoEntity {

		[FormerlySerializedAs("someText")] public string exampleString;

		public DummyRepoEntity() {
			exampleString = "Default string";
		}
		
		public string ToJson() {
			return JsonUtility.ToJson(this);
		}
	}
}