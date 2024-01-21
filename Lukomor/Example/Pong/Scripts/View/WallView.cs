using System.Linq;
using UnityEngine;

namespace Lukomor.Example.Pong
{
    public class WallView : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var ball = collision.gameObject.GetComponent<BallView>();
            
            if (ball)
            {
                var ballDirection = ball.MoveDirection;
                var normal = collision.contacts.First().normal;
                var newDirection = Vector2.Reflect(ballDirection, normal);

                ball.Push(newDirection);
            }
        }
    }
}
