using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.Example.Pong
{
    public class PongGateView : MonoBehaviour
    {
        public UnityEvent OnBallCatched;
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var ball = collision.gameObject.GetComponent<PongBallView>();
            
            if (ball)
            {
                OnBallCatched?.Invoke();
            }
        }
    }
}
