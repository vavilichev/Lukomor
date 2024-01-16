using System;
using Lukomor.Example.Pong;
using UnityEngine;

namespace Lukomor
{
    public class Gate : MonoBehaviour
    {
        public event Action BallCatched; 

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var ball = collision.gameObject.GetComponent<Ball>();
            if (ball)
            {
                ball.Stop();
                
                BallCatched?.Invoke();
            }
        }
    }
}
