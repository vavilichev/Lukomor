using System;
using System.Linq;
using UnityEngine;

namespace Lukomor.Example.Pong
{
    public class Wall : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var ball = collision.gameObject.GetComponent<Ball>();
            if (ball)
            {
                var ballDirection = ball.Direction;
                var normal = collision.contacts.First().normal;
                var newDirection = Vector2.Reflect(ballDirection, normal);

                ball.Push(newDirection);
                Debug.Log($"Collision detected: {collision.gameObject.name}");
            }
        }
    }
}
