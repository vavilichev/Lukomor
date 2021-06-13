using System;
using UnityEngine;

namespace VavilichevGD.Architecture.UI {
	public abstract class UIComponent : MonoBehaviour {
		
		protected void Awake() {
			var uiController = Game.sceneManager.sceneActual.uiController;
		}

		public virtual void OnCreate() { }
		public virtual void OnInitialized() { }
		public virtual void OnStarted() { }
		
	}
}