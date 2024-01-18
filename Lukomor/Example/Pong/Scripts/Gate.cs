using Lukomor.Example.Pong;
using UnityEngine;
using UnityEngine.Events;

namespace Lukomor
{
    public class Gate : MonoBehaviour
    {
        public UnityEvent OnBallCatched;
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var ball = collision.gameObject.GetComponent<Ball>();
            if (ball)
            {
                OnBallCatched?.Invoke();
            }
        }
    }
}
