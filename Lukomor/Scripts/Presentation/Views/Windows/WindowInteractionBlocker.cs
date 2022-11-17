using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lukomor.Presentation.Views.Windows
{
	public class WindowInteractionBlocker : MonoBehaviour
	{
		private bool _isInteractionsBlocked;
		private IWindow _window;
		private Dictionary<Selectable, bool> _selectableObjectsStateCache;

		private void Awake()
		{
			_isInteractionsBlocked = false;
			_window = gameObject.GetComponent<IWindow>();
			_selectableObjectsStateCache = new Dictionary<Selectable, bool>();
		}

		private void OnEnable()
		{
			_window.BlockInteractingRequested += OnBlockInteractingRequested;
		}

		private void OnDisable()
		{
			_window.BlockInteractingRequested -= OnBlockInteractingRequested;

			if (_isInteractionsBlocked)
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

				_selectableObjectsStateCache[selectableObject] = selectableObject.interactable;

				selectableObject.interactable = false;
			}
		}

		private void RevertInteractableState()
		{
			foreach (var cacheItem in _selectableObjectsStateCache)
			{
				if (cacheItem.Key != null)
				{
					var selectableItem = cacheItem.Key;
					var isInteractable = cacheItem.Value;

					selectableItem.interactable = isInteractable;
				}
			}
			
			_selectableObjectsStateCache.Clear();
		}

		private void OnBlockInteractingRequested(bool block)
		{
			if (!_isInteractionsBlocked)
			{
				BlockInteractables();
			}
			else
			{
				RevertInteractableState();
			}

			_isInteractionsBlocked = block;
		}
	}
}