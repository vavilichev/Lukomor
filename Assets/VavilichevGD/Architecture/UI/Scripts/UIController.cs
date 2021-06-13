using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture.UI {
	public class UIController : MonoBehaviour, IUIController {

		#region CONSTANTS

		private const string PATH_PREFABS = "UIElements";
		private static UIController instance;

		#endregion

		#region EVENTS

		public event Action OnBuiltEvent;
		public IUIContainer container { get; set; }

		#endregion

		[SerializeField] private Camera m_uiCamera;
		[SerializeField] private UILayer[] layers;


		public Camera uiCamera => this.m_uiCamera;
		public bool isUIBuilt { get; private set; }


		private Dictionary<Type, UIElement> createdUIElementsMap;
		private Dictionary<Type, UIPopup> cachedPopupsMap;
		private Dictionary<Type, string> uiPrefabsPathsMap;
		private List<IArchitectureCaptureEvents> elementsForArchitectureEvents;
		
		
		public UIElement[] GetAllCreatedUIElements() {
			return this.createdUIElementsMap.Values.ToArray();
		}

		public T GetUIElement<T>() where T : UIElement {
			var type = typeof(T);
			this.createdUIElementsMap.TryGetValue(type, out var uiElement);
			return (T) uiElement;
		}

		
		private void Awake() {
			if (instance == null) {
				this.elementsForArchitectureEvents = new List<IArchitectureCaptureEvents>();
				instance = this;
				DontDestroyOnLoad(this.gameObject);
			}
			else {
				Destroy(this.gameObject);
			}
		}


		public void SendEventOnCreate() {
			this.CleanElementsForArchitectureEvents();
			foreach (var element in this.elementsForArchitectureEvents) 
				element.OnCreate();
		}

		public void SendEventOnInitialized() {
			this.CleanElementsForArchitectureEvents();
			foreach (var element in this.elementsForArchitectureEvents) 
				element.OnInitialized();
		}

		public void SendEventOnStarted() {
			this.CleanElementsForArchitectureEvents();
			foreach (var element in this.elementsForArchitectureEvents) 
				element.OnStarted();
		}

		private void CleanElementsForArchitectureEvents() {
			var nullElements = this.elementsForArchitectureEvents.Where(element => element == null);
			foreach (var element in nullElements) 
				this.elementsForArchitectureEvents.Remove(element);
		}
		
		
		
		#region SHOW

		public T ShowUIElement<T>() where T : UIElement, IUIElementOnLayer {
			var type = typeof(T);

			this.cachedPopupsMap.TryGetValue(type, out var cachedPopup);
			if (cachedPopup != null) {
				cachedPopup.Show();
				return cachedPopup as T;
			}

			this.uiPrefabsPathsMap.TryGetValue(type, out var prefName);
			if (!string.IsNullOrEmpty(prefName))
				return this.CreateAndShowElement<T>(prefName);

			throw new Exception($"There is no UIElements with type {type} registered");
		}

		private T CreateAndShowElement<T>(string prefName) where T : UIElement, IUIElementOnLayer {
			var path = $"{PATH_PREFABS}/{prefName}";
			var uiElementPref = Resources.Load<T>(path);
			var container = this.GetContainer(uiElementPref.layer);
			var createdElement = Instantiate(uiElementPref, container);
			createdElement.name = uiElementPref.name;
			var type = createdElement.GetType();
			
			this.createdUIElementsMap[type] = createdElement;
			createdElement.Show();
			createdElement.OnElementHiddenCompletelyEvent += this.OnElementHiddenCompletely;
			return createdElement;
		}

		private void OnElementHiddenCompletely(IUIElement uiElement) {
			if (uiElement is IUIPopup uiPopup && uiPopup.isPreCached)
				return;

			var type = uiElement.GetType();
			uiElement.OnElementHiddenCompletelyEvent -= this.OnElementHiddenCompletely;
			this.createdUIElementsMap.Remove(type);
		}

		#endregion

		

		#region BUILD

		public void BuildUI() {
			this.createdUIElementsMap = new Dictionary<Type, UIElement>();
			this.uiPrefabsPathsMap = new Dictionary<Type, string>();
			this.cachedPopupsMap = new Dictionary<Type, UIPopup>();
			
			var allPrefabs = Resources.LoadAll<UIElement>(PATH_PREFABS);
			foreach (var uiElementPref in allPrefabs) {
				if (uiElementPref is UIScreen uiScreenPref && uiScreenPref.showByDefault) {
					this.CreateAndShowScreen(uiScreenPref);
					continue;
				}
				
				if (uiElementPref is UIPopup popupPref && popupPref.isPreCached) 
					this.CreateCachedPopup(popupPref);
				else
					this.RememberPath(uiElementPref);
			}

			this.isUIBuilt = true;
			Logging.Log($"INTERFACE CREATED SUCCESSFULLY: " +
			            $"total elements: {allPrefabs.Length}, " +
			            $"created: {this.createdUIElementsMap.Count}, " +
			            $"pre cached popups: {this.cachedPopupsMap.Count}");

			Resources.UnloadUnusedAssets();
			this.OnBuiltEvent?.Invoke();
		}

		private void CreateCachedPopup(UIPopup popupPref) {
			var container = this.GetContainer(popupPref.layer);
			var createdCachedPopup = Instantiate(popupPref, container);
			createdCachedPopup.name = popupPref.name;
			var type = createdCachedPopup.GetType();
			
			this.cachedPopupsMap[type] = createdCachedPopup;
			this.createdUIElementsMap[type] = createdCachedPopup;
			
			createdCachedPopup.HideInstantly();
		}

		private void CreateAndShowScreen(UIScreen uiScreenPref) {
			var container = this.GetContainer(uiScreenPref.layer);
			var createdUIScreen = Instantiate(uiScreenPref, container);
			createdUIScreen.name = uiScreenPref.name;
			var type = createdUIScreen.GetType();
			this.createdUIElementsMap[type] = createdUIScreen;
			createdUIScreen.Show();
		}

		private Transform GetContainer(UILayerType layer) {
			foreach (var uiLayer in this.layers) {
				if (uiLayer.layer == layer)
					return uiLayer.transform;
			}
			
			throw new Exception($"There is no layer ({layer}) in uiController");
		}
		
		private void RememberPath(UIElement uiElementPref) {
			var path = uiElementPref.name;
			var type = uiElementPref.GetType();
			this.uiPrefabsPathsMap[type] = path;
		}

		#endregion

		
		public void DestroyAll() {
			var allCreatedUIElements = this.createdUIElementsMap.Values.ToArray();
			foreach (var uiElement in allCreatedUIElements) {
				var type = uiElement.GetType();
				this.createdUIElementsMap.Remove(type);

				if (uiElement is UIPopup uiPopup)
					this.cachedPopupsMap.Remove(type);
				
				Destroy(uiElement.gameObject);
			}
		}
	}
}