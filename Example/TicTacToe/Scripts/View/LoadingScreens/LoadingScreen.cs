using System;
using System.Collections;
using Lukomor.Features.Scenes;
using UnityEngine;

namespace Lukomore.Example.TicTacToe.View
{
    public class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        private bool _isHiding;
        
        public void Show(Action onComplete = null)
        {
            gameObject.SetActive(true);
            onComplete?.Invoke();
        }

        public void Hide(Action onComplete = null)
        {
            if (_isHiding)
            {
                return;
            }

            _isHiding = true;
            
            StartCoroutine(HideRoutine(onComplete));
        }

        private IEnumerator HideRoutine(Action onComplete)
        {
            yield return new WaitForSeconds(1f);

            _isHiding = false;
            
            onComplete?.Invoke();
            
            gameObject.SetActive(false);
        }
    }
}