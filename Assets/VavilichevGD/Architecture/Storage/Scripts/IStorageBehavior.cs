using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VavilichevGD.Architecture.StorageSystem {
	
	
	public interface IStorageBehavior {

		#region DELEGATES

		event StorageHandler OnStorageLoadedEvent;

		#endregion

		Dictionary<string, RepoData> repoDataMap { get; }
		bool isInitialized { get; }
		bool isOnProcess { get; }
		
		void Load(Scene scene);
		Coroutine LoadAsync(Scene scene);
		
		
		bool HasObject(string key);
		void ClearKey(string key);
		void ClearAll();

		void SaveAllRepositories(Scene scene);
		Coroutine SaveAllRepositoriesAsync(Scene scene, UnityAction callback = null);

		
		#region SET

		void SetFloat(string key, float value);
		void SetInteger(string key, int value);
		void SetBool(string key, bool value);
		void SetString(string key, string value);
		void SetEnum(string key, Enum value);
		void SetCustom<T>(string key, T value);
		void SetRepoData(string key, RepoData value);

		#endregion


		#region GET

		float GetFloat(string key, float defaultValue = 0f);
		int GetInteger(string key, int defaultValue = 0);
		bool GetBool(string key, bool defaultValue = false);
		string GetString(string key, string defaultValue = "");
		T GetEnum<T>(string key, T defaultValue) where T : Enum;
		T GetCustom<T>(string key, T defaultValue);
		RepoData GetRepoData(string key, RepoData defaultValue);

		#endregion

	}
}