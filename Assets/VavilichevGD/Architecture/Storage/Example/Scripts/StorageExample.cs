using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace VavilichevGD.Architecture.StorageSystem.Example {
	public class StorageExample : MonoBehaviour {

		#region CONSTANTS

		private const string KEY_INT = "INTEGER";
		private const string KEY_SPEED = "SPEED";
		private const string KEY_FLOAT = "FLOAT";
		private const string KEY_VECTOR3 = "VECTOR3";
		private const string KEY_VECTOR2 = "VECTOR2";
		private const string KEY_VERSION = "VERSION";

		#endregion
		
		[SerializeField] private Button buttonSaveAsync;
		[SerializeField] private Button buttonLoadInstantly;
		[SerializeField] private Button buttonLoadWithRoutine;
		[SerializeField] private Text textLog;

		private bool savingComplete;


		#region LIFECYCLE

		private void Start() {
			Storage.instance.Load();
			this.PrintData("loaded instantly at start", false);
			
			var loadedVersion = Storage.instance.Get<int>(KEY_VERSION);
			if (loadedVersion < 2) {
				Storage.instance.Set(KEY_VERSION, 2);
				Storage.instance.Set(KEY_SPEED, 100);
				Debug.Log("New version. Speed changed to 100");
			}
		}

		private void OnEnable() {
			this.buttonSaveAsync.onClick.AddListener(this.OnSaveAsyncButtonClick);
			this.buttonLoadInstantly.onClick.AddListener(this.OnLoadInstantlyButtonClick);
			this.buttonLoadWithRoutine.onClick.AddListener(this.OnLoadWithRoutineButtonClick);
		}

		private void OnDisable() {
			this.buttonSaveAsync.onClick.RemoveListener(this.OnSaveAsyncButtonClick);
			this.buttonLoadInstantly.onClick.RemoveListener(this.OnLoadInstantlyButtonClick);
			this.buttonLoadWithRoutine.onClick.RemoveListener(this.OnLoadWithRoutineButtonClick);
		}

		#endregion
		
	
		
		private void Update() {
			if (this.savingComplete) {
				this.PrintData("saved async", true);
				this.Log($"Saving complete at: {Time.time}", true);
				this.savingComplete = false;
			}
		}

		#region CALLBACKS

		private void OnSaveAsyncButtonClick() {
			this.Log($"Saving started: {Time.time}", false);
				
			Storage.instance.Set(KEY_INT, Random.Range(0, 10));
			Storage.instance.Set(KEY_FLOAT, Random.Range(0f, 10f));
			Storage.instance.Set(KEY_VECTOR3, Vector3.left);
			Storage.instance.Set(KEY_VECTOR2, Vector2.up);
			Storage.instance.Set(KEY_VERSION, 2);
				
			Storage.instance.SaveAsync(() => {
				// We cannot get Time.time (for logging the end of saving process)
				// because Unity doesn't support to do this in the side thread. That is why we use a simple flag.
				this.savingComplete = true;
			});
		}

		private void OnLoadInstantlyButtonClick() {
			Storage.instance.Load();
			this.PrintData("loaded instantly", false);
		}
		
		private void OnLoadWithRoutineButtonClick() {
			Storage.instance.LoadWithRoutine(() => this.PrintData("loaded with routine", false));
		}

		#endregion



		#region LOGGING

		private void PrintData(string process, bool append) {
			var loadedInt = Storage.instance.Get<int>(KEY_INT);
			var loadedSpeed = Storage.instance.Get<int>(KEY_SPEED);
			var loadedFloat = Storage.instance.Get<float>(KEY_FLOAT);
			var loadedVector3 = Storage.instance.Get<Vector3>(KEY_VECTOR3);
			var loadedVector2 = Storage.instance.Get<Vector2>(KEY_VECTOR2);
			var loadedVersion = Storage.instance.Get<int>(KEY_VERSION);
			
			
			this.Log($"GameData {process}:\n" +
			         $"int = {loadedInt},\n" +
			         $"speed = {loadedSpeed},\n" +
			         $"float = {loadedFloat},\n" +
			         $"vector3 = {loadedVector3},\n" +
			         $"vector2 = {loadedVector2},\n" +
			         $"version = {loadedVersion}", append);
		}
		
		private void Log(string text, bool append) {
			var logText = append ? this.textLog.text + "\n" + text : text;
			this.textLog.text = logText;
		}

		#endregion
		
	}
}