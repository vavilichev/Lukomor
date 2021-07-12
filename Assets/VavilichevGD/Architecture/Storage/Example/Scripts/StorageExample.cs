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
		
		[SerializeField] private Button _buttonSaveAsync;
		[SerializeField] private Button _buttonLoadInstantly;
		[SerializeField] private Button _buttonLoadWithRoutine;
		[SerializeField] private Text _textLog;

		[SerializeField] private string _storageFileName = "GameState.save";
		
		private bool _savingComplete;
		private Storage _storage;


		#region LIFECYCLE

		private void Start() {
			_storage = new FileStorage(_storageFileName);
			_storage.Load();
			PrintData("loaded instantly at start", false);
			
			var loadedVersion = _storage.Get<int>(KEY_VERSION);
			if (loadedVersion < 2) {
				_storage.Set(KEY_VERSION, 2);
				_storage.Set(KEY_SPEED, 100);
				Debug.Log("New version. Speed changed to 100");
			}
		}

		private void OnEnable() {
			_buttonSaveAsync.onClick.AddListener(OnSaveAsyncButtonClick);
			_buttonLoadInstantly.onClick.AddListener(OnLoadInstantlyButtonClick);
			_buttonLoadWithRoutine.onClick.AddListener(OnLoadWithRoutineButtonClick);
		}

		private void OnDisable() {
			_buttonSaveAsync.onClick.RemoveListener(OnSaveAsyncButtonClick);
			_buttonLoadInstantly.onClick.RemoveListener(OnLoadInstantlyButtonClick);
			_buttonLoadWithRoutine.onClick.RemoveListener(OnLoadWithRoutineButtonClick);
		}

		#endregion
		
	
		
		private void Update() {
			if (_savingComplete) {
				PrintData("saved async", true);
				Log($"Saving complete at: {Time.time}", true);
				_savingComplete = false;
			}
		}

		#region CALLBACKS

		private void OnSaveAsyncButtonClick() {
			Log($"Saving started: {Time.time}", false);
				
			_storage.Set(KEY_INT, Random.Range(0, 10));
			_storage.Set(KEY_FLOAT, Random.Range(0f, 10f));
			_storage.Set(KEY_VECTOR3, Vector3.left);
			_storage.Set(KEY_VECTOR2, Vector2.up);
			_storage.Set(KEY_VERSION, 2);
				
			_storage.SaveAsync(() => {
				// We cannot get Time.time (for logging the end of saving process)
				// because Unity doesn't support to do this in the side thread. That is why we use a simple flag.
				_savingComplete = true;
			});
		}

		private void OnLoadInstantlyButtonClick() {
			_storage.Load();
			PrintData("loaded instantly", false);
		}
		
		private void OnLoadWithRoutineButtonClick() {
			_storage.LoadWithRoutine(loadedData => PrintData("loaded with routine", false));
		}

		#endregion



		#region LOGGING

		private void PrintData(string process, bool append) {
			var loadedInt = _storage.Get<int>(KEY_INT);
			var loadedSpeed = _storage.Get<int>(KEY_SPEED);
			var loadedFloat = _storage.Get<float>(KEY_FLOAT);
			var loadedVector3 = _storage.Get<Vector3>(KEY_VECTOR3);
			var loadedVector2 = _storage.Get<Vector2>(KEY_VECTOR2);
			var loadedVersion = _storage.Get<int>(KEY_VERSION);
			
			
			Log($"GameData {process}:\n" +
			         $"int = {loadedInt},\n" +
			         $"speed = {loadedSpeed},\n" +
			         $"float = {loadedFloat},\n" +
			         $"vector3 = {loadedVector3},\n" +
			         $"vector2 = {loadedVector2},\n" +
			         $"version = {loadedVersion}", append);
		}
		
		private void Log(string text, bool append) {
			var logText = append ? _textLog.text + "\n" + text : text;
			_textLog.text = logText;
		}

		#endregion
		
	}
}