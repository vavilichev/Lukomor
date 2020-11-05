using System;
using UnityEngine;

namespace VavilichevGD.Architecture.StorageSystem {
	public sealed class StorageBehaviorPlayerPrefs : StorageBehaviorBase{

		#region DELEGATES

		public override event StorageHandler OnStorageLoadedEvent;
		
		#endregion


		public override bool HasObject(string key) {
			return PlayerPrefs.HasKey(key) || this.repoDataMap.ContainsKey(key);
		}

		public override void ClearKey(string key) {
			if (this.repoDataMap.ContainsKey(key))
				this.repoDataMap.Remove(key);
			PlayerPrefs.DeleteKey(key);
			
			Debug.Log($"STORAGE PREFS: Key deleted: {key}");
		}

		public override void ClearAll() {
			this.repoDataMap.Clear();
			PlayerPrefs.DeleteAll();
			
			Debug.Log("STORAGE PREFS: All prefs deleted");
		}
		

		
		#region SET

		public override void SetFloat(string key, float value) {
			PlayerPrefs.SetFloat(key, value);
		}

		public override void SetInteger(string key, int value) {
			PlayerPrefs.SetInt(key, value);
		}

		public override void SetBool(string key, bool value) {
			PlayerPrefs.SetInt(key, this.BoolToInteger(value));
		}

		public override void SetString(string key, string value) {
			PlayerPrefs.SetString(key, value);
		}

		public override void SetEnum(string key, Enum value) {
			PlayerPrefs.SetString(key, value.ToString());
		}

		public override void SetCustom<T>(string key, T value) {
			var json = JsonUtility.ToJson(value);
			var jsonEncrypted = this.Encrypt(json);
			//Debug.Log($"Saved Key: {key}, value: {json}");
			PlayerPrefs.SetString(key, jsonEncrypted);
		}

		public override void SetRepoData(string key, RepoData value) {
			this.repoDataMap[key] = value;
			this.SetCustom(key, value);
		}

		#endregion


		#region GET

		public override float GetFloat(string key, float defaultValue = 0f) {
			return PlayerPrefs.GetFloat(key, defaultValue);
		}

		public override int GetInteger(string key, int defaultValue = 0) {
			return PlayerPrefs.GetInt(key, defaultValue);
		}

		public override bool GetBool(string key, bool defaultValue = false) {
			return this.IntToBool(PlayerPrefs.GetInt(key, this.BoolToInteger(defaultValue)));
		}

		public override string GetString(string key, string defaultValue = "") {
			return PlayerPrefs.GetString(key, defaultValue);
		}

		public override T GetEnum<T>(string key, T defaultValue) {
			return (T) Enum.Parse(typeof(T), PlayerPrefs.GetString(key));
		}

		public override T GetCustom<T>(string key, T defaultValue = default) {
			if (!PlayerPrefs.HasKey(key))
				return defaultValue;

			var jsonEncrypted = PlayerPrefs.GetString(key);
			var json = this.Decrypt(jsonEncrypted);
			return JsonUtility.FromJson<T>(json);
		}

		public override RepoData GetRepoData(string key, RepoData defaultValue) {
			if (this.repoDataMap.ContainsKey(key))
				return this.repoDataMap[key];

			return defaultValue;
		}

		#endregion
		
	}
}