using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VavilichevGD.Architecture.UserInterface {
	public class UIController : MonoBehaviour {

		#region EVENTS

		public event Action OnUIBuiltEvent;

		#endregion

		[SerializeField] private Camera _uiCamera;
		[SerializeField] private UILayer[] _layers;


		public Camera uiCamera => _uiCamera;
		public bool isUIBuilt { get; private set; }
		public bool isLoggingEnabled { get; set; }


		private Dictionary<Type, IUIElementOnLayer> createdUIElementsMap;
		private Dictionary<Type, UIPopup> cachedPopupsMap;
		private List<Type> uiDynamicPrefabTypes;
		private UISceneConfig uiSceneConfig;



		private void Awake() {
			createdUIElementsMap = new Dictionary<Type, IUIElementOnLayer>();
			uiDynamicPrefabTypes = new List<Type>();
			cachedPopupsMap = new Dictionary<Type, UIPopup>();

			DontDestroyOnLoad(gameObject);
		}



		#region MESSAGES

		/// <summary>
		/// Called when all repositories and interactors are created.
		/// </summary>
		public void SendMessageOnCreate() {
			var allCreatedElements = createdUIElementsMap.Values.ToArray();
			foreach (var element in allCreatedElements)
				element.OnCreate();
		}

		/// <summary>
		/// Called when all repositories and interactors are initialized.
		/// </summary>
		public void SendMessageOnInitialize() {
			var allCreatedElements = createdUIElementsMap.Values.ToArray();
			foreach (var element in allCreatedElements)
				element.OnInitialize();
		}

		/// <summary>
		/// Called when all repositories and interactors are started.
		/// </summary>
		public void SendMessageOnStart() {
			var allCreatedElements = createdUIElementsMap.Values.ToArray();
			foreach (var element in allCreatedElements)
				element.OnStart();
		}

		#endregion



		#region SHOW

		public T ShowUIElement<T>() where T : UIElement, IUIElementOnLayer {
			var type = typeof(T);

			if (createdUIElementsMap.TryGetValue(type, out var foundElement) && foundElement.isActive)
				return (T) foundElement;

			cachedPopupsMap.TryGetValue(type, out var cachedPopup);
			if (cachedPopup != null) {
				cachedPopup.Show();
				return cachedPopup as T;
			}

			var prefab = uiSceneConfig.GetPrefab(type);
			return CreateAndShowElement<T>(prefab);
		}

		private T CreateAndShowElement<T>(IUIElementOnLayer prefab) where T : UIElement, IUIElementOnLayer {
			var container = GetContainer(prefab.layer);
			var createdElementGo = Instantiate(prefab.gameObject, container);
			createdElementGo.name = prefab.name;
			var createdElement = createdElementGo.GetComponent<T>();
			var type = typeof(T);

			createdUIElementsMap[type] = createdElement;
			createdElement.Show();
			createdElement.OnElementHiddenCompletelyEvent += OnElementHiddenCompletely;
			return createdElement;
		}

		private void OnElementHiddenCompletely(IUIElement uiElement) {
			if (uiElement is IUIPopup uiPopup && uiPopup.isPreCached)
				return;

			var type = uiElement.GetType();
			uiElement.OnElementHiddenCompletelyEvent -= OnElementHiddenCompletely;
			createdUIElementsMap.Remove(type);
		}

		#endregion



		#region BUILD

		public void BuildUI(UISceneConfig uiSceneConfig) {
			this.uiSceneConfig = uiSceneConfig;

			var prefabs = uiSceneConfig.GetPrefabs();
			foreach (var uiElementPref in prefabs) {
				if (uiElementPref is UIScreen uiScreenPref && uiScreenPref.showByDefault) {
					CreateAndShowScreen(uiScreenPref);
					continue;
				}

				if (uiElementPref is UIPopup popupPref && popupPref.isPreCached)
					CreateCachedPopup(popupPref);
				else
					RememberTypeForLaterCreation(uiElementPref);
			}

			isUIBuilt = true;
			
			if (isLoggingEnabled) {
				Debug.Log($"INTERFACE CREATED SUCCESSFULLY: " +
				          $"total elements: {prefabs.Length}, " +
				          $"created: {createdUIElementsMap.Count}, " +
				          $"pre cached popups: {cachedPopupsMap.Count}");
			}

			Resources.UnloadUnusedAssets();
			OnUIBuiltEvent?.Invoke();
		}

		private void CreateCachedPopup(UIPopup popupPref) {
			var container = GetContainer(popupPref.layer);
			var createdCachedPopup = Instantiate(popupPref, container);
			createdCachedPopup.name = popupPref.name;
			var type = createdCachedPopup.GetType();

			cachedPopupsMap[type] = createdCachedPopup;
			createdUIElementsMap[type] = createdCachedPopup;

			createdCachedPopup.HideInstantly();
		}

		private void CreateAndShowScreen(UIScreen uiScreenPref) {
			var container = GetContainer(uiScreenPref.layer);
			var createdUIScreen = Instantiate(uiScreenPref, container);
			createdUIScreen.name = uiScreenPref.name;
			var type = createdUIScreen.GetType();
			createdUIElementsMap[type] = createdUIScreen;
			createdUIScreen.Show();
		}

		private Transform GetContainer(UILayerType layer) {
			return _layers.First(layerObject => layerObject.layer == layer).transform;
		}

		private void RememberTypeForLaterCreation(IUIElement uiElementPref) {
			var type = uiElementPref.GetType();
			uiDynamicPrefabTypes.Add(type);
		}

		#endregion

		
		
		public IUIElementOnLayer[] GetAllCreatedUIElements() {
			return createdUIElementsMap.Values.ToArray();
		}

		public T GetUIElement<T>() where T : UIElement {
			var type = typeof(T);
			createdUIElementsMap.TryGetValue(type, out var uiElement);
			return (T) uiElement;
		}
		

		public void Clear() {
			if (createdUIElementsMap == null)
				return;

			var allCreatedUIElements = createdUIElementsMap.Values.ToArray();
			foreach (var uiElement in allCreatedUIElements)
				Destroy(uiElement.gameObject);

			createdUIElementsMap.Clear();
			cachedPopupsMap.Clear();
			uiDynamicPrefabTypes.Clear();
		}


#if UNITY_EDITOR
		private void Reset() {
			if (_uiCamera == null)
				_uiCamera = GetComponentInChildren<Camera>();

			_layers = GetComponentsInChildren<UILayer>();
		}
#endif
		
	}
}