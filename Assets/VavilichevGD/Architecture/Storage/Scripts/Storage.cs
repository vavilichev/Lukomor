using System;
using UnityEngine;
using UnityEngine.Events;

namespace VavilichevGD.Architecture.StorageSystem {
	public delegate void StorageHandler();
	
	public static class Storage {

		#region DELEGATES

		public static event StorageHandler OnStorageLoadedEvent;

		#endregion

		public static bool isInitialized => behavior != null && behavior.isInitialized;

		private static IStorageBehavior behavior;

		
		public static void Load(Scene scene) {
			if (behavior == null)
				behavior = new StorageBehaviorPlayerPrefs();

			void OnStorageLoaded() {
				behavior.OnStorageLoadedEvent -= OnStorageLoaded;
				OnStorageLoadedEvent?.Invoke();
			}

			behavior.OnStorageLoadedEvent += OnStorageLoaded; 
			behavior.Load(scene);
		}

		public static Coroutine LoadAsync(Scene scene) {
			if (behavior == null)
				behavior = new StorageBehaviorPlayerPrefs();

			void OnStorageLoaded() {
				behavior.OnStorageLoadedEvent -= OnStorageLoaded;
				OnStorageLoadedEvent?.Invoke();
			}

			behavior.OnStorageLoadedEvent += OnStorageLoaded; 
			return behavior.LoadAsync(scene);
		}
		
		
		
        public static bool HasObject(string key) {
	        return behavior.HasObject(key);
        }

        public static void ClearKey(string key) {
	        behavior.ClearKey(key);
        }

        public static void ClearAll() {
	        behavior.ClearAll();
        }

        public static void SaveAllRepositories(Scene scene) {
	        behavior.SaveAllRepositories(scene);
        }

        public static Coroutine SaveAllRepositoriesAsync(Scene scene, UnityAction callback) {
	        return behavior.SaveAllRepositoriesAsync(scene, callback);
        }


        #region SET

        public static void SetFloat(string key, float value) {
	        behavior.SetFloat(key, value);
        }

        public static void SetInteger(string key, int value) {
	        behavior.SetInteger(key, value);
        }

        public static void SetBool(string key, bool value) {
	        behavior.SetBool(key, value);
        }

        public static void SetString(string key, string value) {
	        behavior.SetString(key, value);
        }

        public static void SetEnum(string key, Enum value) {
	        behavior.SetEnum(key, value);
        }

        public static void SetCustom<T>(string key, T value) {
	        behavior.SetCustom(key, value);
        }

        public static void SetRepoData(string key, RepoData value) {
	        behavior.SetRepoData(key, value);
        }

        #endregion

        
        #region GET

        public static float GetFloat(string key, float defaultValue = 0) {
	        return behavior.GetFloat(key, defaultValue);
        }

        public static int GetInteger(string key, int defaultValue = 0) {
	        return behavior.GetInteger(key, defaultValue);
        }

        public static bool GetBool(string key, bool defaultValue = false) {
	        return behavior.GetBool(key, defaultValue);
        }

        public static string GetString(string key, string defaultValue = "") {
	        return behavior.GetString(key, defaultValue);
        }

        public static T GetEnum<T>(string key, T defaultValue) where T : Enum {
	        return behavior.GetEnum(key, defaultValue);
        }

        public static T GetCustom<T>(string key, T defaultValue = default) {
	        return behavior.GetCustom(key, defaultValue);
        }

        public static RepoData GetRepoData(string key, RepoData defaultValue) {
	        return behavior.GetRepoData(key, defaultValue);
        }

        #endregion

		
	}
}