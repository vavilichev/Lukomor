using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.Example.Pong
{
    public class GateView : MonoBehaviour
    {
        public UnityEvent OnBallCatched;
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var ball = collision.gameObject.GetComponent<BallView>();
            
            if (ball)
            {
                OnBallCatched?.Invoke();
            }
        }
    }
}
