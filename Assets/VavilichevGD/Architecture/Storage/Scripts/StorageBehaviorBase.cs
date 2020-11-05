using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture.StorageSystem {
	public abstract class StorageBehaviorBase : IStorageBehavior {
        
		#region Keys
    
        private static readonly string password = $"{SystemInfo.deviceModel}_VavilichevGD_PSWD";
        private static readonly string salt = $"THISIS{SystemInfo.deviceModel}_VavilichevGD_SAULT";
        private static readonly string VIKey = "@1B2ccD4e562g7A8";
    
        #endregion

        #region DELEGATES

        public virtual event StorageHandler OnStorageLoadedEvent;

        #endregion

        public Dictionary<string, RepoData> repoDataMap { get; }
        public bool isInitialized { get; protected set; }
        public bool isOnProcess { get; protected set; }


        public StorageBehaviorBase() {
            this.repoDataMap = new Dictionary<string, RepoData>();
        }

        
        #region Convertion

        protected int BoolToInteger(bool value) {
            return value ? 1 : 0;
        }

        protected bool IntToBool(int value) {
            return value != 0;
        }

        #endregion
    
    
        #region Encryption

        protected string Encrypt(string plainText) {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes =
                new Rfc2898DeriveBytes(password, Encoding.ASCII.GetBytes(salt)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged()
                {Mode = CipherMode.CBC, Padding = PaddingMode.Zeros};
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream()) {
                using (var cryptoStream =
                    new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)) {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }

                memoryStream.Close();
            }

            return Convert.ToBase64String(cipherTextBytes);
        }

        protected string Decrypt(string encryptedText) {
            try {
                byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);

                byte[] keyBytes =
                    new Rfc2898DeriveBytes(password, Encoding.ASCII.GetBytes(salt)).GetBytes(
                        256 / 8);
                var symmetricKey = new RijndaelManaged()
                    {Mode = CipherMode.CBC, Padding = PaddingMode.None};

                var decryptor =
                    symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
                var memoryStream = new MemoryStream(cipherTextBytes);
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                int decryptedByteCount =
                    cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount)
                    .TrimEnd("\0".ToCharArray());
            }
            catch {
                return encryptedText;
            }
        }

        #endregion


        #region Methods


        public virtual void Load(Scene scene) {
            this.isOnProcess = true;
            this.isInitialized = false;
            this.repoDataMap.Clear();

            var repositories = scene.GetRepositories<IRepository>();
            foreach (var repository in repositories) {
                var key = repository.id;
                var repoData = this.GetCustom(key, repository.GetRepoDataDefault());
                this.repoDataMap[key] = repoData;
                
#if DEBUG
                Debug.Log($"STORAGE: Loaded key {repoData.id} with value {repoData.json}");
#endif
            }

            this.isInitialized = true;
            this.isOnProcess = false;
            this.OnStorageLoadedEvent?.Invoke();
        }

        public Coroutine LoadAsync(Scene scene) {
            return Coroutines.StartRoutine(this.LoadAsyncRoutine(scene));
        }

        protected virtual IEnumerator LoadAsyncRoutine(Scene scene) {
            if (this.isOnProcess) {
                Debug.LogError("You cannot load scene while another process is running");
                yield break;
            }

            this.isOnProcess = true;
            this.isInitialized = false;
            this.repoDataMap.Clear();

            var repositories = scene.GetRepositories<IRepository>();
            foreach (var repository in repositories) {
                var key = repository.id;
                var repoData = this.GetCustom(key, repository.GetRepoDataDefault());
                this.repoDataMap[key] = repoData;
                
#if DEBUG
                Debug.Log($"STORAGE: Loaded key {repoData.id} with value {repoData.json}");
#endif
                yield return null;
            }

            this.isInitialized = true;
            this.isOnProcess = false;
            this.OnStorageLoadedEvent?.Invoke();
        }

        public abstract bool HasObject(string key);

        public abstract void ClearKey(string key);

        public abstract void ClearAll();

        public virtual void SaveAllRepositories(Scene scene) {
            var repositories = scene.GetRepositories<IRepository>();
            foreach (var repository in repositories) {
                var repoData = repository.GetRepoData();
                this.SetRepoData(repoData.id, repoData);
#if DEBUG
                Debug.Log($"STORAGE PREFS: Save Key: {repoData.id}, value: {repoData.json}");
#endif
            }
        }

        public virtual Coroutine SaveAllRepositoriesAsync(Scene scene, UnityAction callback = null) {
            return Coroutines.StartRoutine(this.SaveAllRepositoriesAsyncRoutine(scene, callback));
        }

        protected IEnumerator SaveAllRepositoriesAsyncRoutine(Scene scene, UnityAction callback) {
            if (this.isOnProcess) {
                Debug.LogError("STORAGE: You cannot save anything while another process is running");
                yield break;
            }
            
            var repositories = scene.GetRepositories<IRepository>();
            foreach (var repository in repositories) {
                var repoData = repository.GetRepoData();
                this.SetRepoData(repoData.id, repoData);
#if DEBUG
                Debug.Log($"STORAGE PREFS: Save Key: {repoData.id}, value: {repoData.json}");
#endif
                yield return null;
            }
            
            callback?.Invoke();
        }


        #region SET

        public abstract void SetFloat(string key, float value);

        public abstract void SetInteger(string key, int value);

        public abstract void SetBool(string key, bool value);

        public abstract void SetString(string key, string value);

        public abstract void SetEnum(string key, Enum value);

        public abstract void SetCustom<T>(string key, T value);

        public abstract void SetRepoData(string key, RepoData value);

        #endregion

        
        #region GET

        public abstract float GetFloat(string key, float defaultValue = 0);

        public abstract int GetInteger(string key, int defaultValue = 0);

        public abstract bool GetBool(string key, bool defaultValue = false);

        public abstract string GetString(string key, string defaultValue = "");

        public abstract T GetEnum<T>(string key, T defaultValue) where T : Enum;

        public abstract T GetCustom<T>(string key, T defaultValue = default);

        public abstract RepoData GetRepoData(string key, RepoData defaultValue);

        #endregion


        #endregion
    }
}