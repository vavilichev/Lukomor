using System;
using UnityEngine;
using UnityEngine.Serialization;
using VavilichevGD.Architecture.StorageSystem;

namespace VavilichevGD.Architecture.Example {
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