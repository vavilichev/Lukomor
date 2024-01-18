using System;
using System.Linq;
using UnityEngine;

namespace Lukomor.Example.Pong
{
    public class Block : MonoBehaviour
    {
        private const float BALL_SPEED_INCREASING_STEP = 0.3f;
        
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _smoothing = 1f;
        [SerializeField] private float _limitY = 4.75f;
        
        public void Move(float y)
        {
            var nextPosition = Vector3.Lerp(transform.position, transform.position + Vector3.up * (y * _speed), Time.deltaTime * _smoothing);
            nextPosition.y = Mathf.Clamp(nextPosition.y, -_limitY, _limitY);

            transform.position = nextPosition;
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var ball = collision.gameObject.GetComponent<Ball>();
            
            if (ball)
            {
                var ballDirection = ball.Direction;
                var normal = collision.contacts.First().normal;
                var newDirection = Vector2.Reflect(ballDirection, normal);

                ball.Push(newDirection);
                ball.SpeedUp(BALL_SPEED_INCREASING_STEP);
            }
        }
    }
}
