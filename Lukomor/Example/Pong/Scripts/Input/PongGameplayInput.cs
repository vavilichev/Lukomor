using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.Example.Pong
{
    public class PongGameplayInput : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnPauseClicked;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnPauseClicked.Invoke();
            }
        }
}
}