using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lukomor.UI
{
	public class WindowInteractionBlocker : MonoBehaviour
	{
		private bool isInteractionsBlocked;
		private IWindow window;
		private Dictionary<Selectable, bool> selectableObjectsStateCache;

		private void Awake()
		{
			isInteractionsBlocked = false;
			window = gameObject.GetComponent<IWindow>();
			selectableObjectsStateCache = new Dictionary<Selectable, bool>();
		}

		private void OnEnable()
		{
			window.BlockInteractingRequested += OnBlockInteractingRequested;
		}

		private void OnDisable()
		{
			window.BlockInteractingRequested -= OnBlockInteractingRequested;

			if (isInteractionsBlocked)
			{
				
				RevertInteractableState();
			}
		}

		private void BlockInteractables()
		{
			var allSelectableObjects = gameObject.GetComponentsInChildren<Selectable>();
			var objectsCount = allSelectableObjects.Length;

			for (int i = 0; i < objectsCount; i++)
			{
				var selectableObject = allSelectableObjects[i];

				selectableObjectsStateCache[selectableObject] = selectableObject.interactable;

				selectableObject.interactable = false;
			}

			isInteractionsBlocked = true;
		}

		private void RevertInteractableState()
		{
			foreach (var cacheItem in selectableObjectsStateCache)
			{
				if (cacheItem.Key != null)
				{
					var selectableItem = cacheItem.Key;
					var isInteractable = cacheItem.Value;

					selectableItem.interactable = isInteractable;
				}
			}
			
			selectableObjectsStateCache.Clear();
			
			isInteractionsBlocked = false;
		}

		private void OnBlockInteractingRequested(bool block)
		{
			if (block && !isInteractionsBlocked)
			{
				BlockInteractables();
			}
			else if (!block && isInteractionsBlocked)
			{
				RevertInteractableState();
			}
		}
	}
}