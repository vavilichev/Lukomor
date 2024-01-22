using System.Linq;
using UnityEngine;

namespace Lukomor.Example.Pong
{
    public class BlockView : MonoBehaviour
    {
        private const float BALL_SPEED_INCREASING_STEP = 0.4f;
        
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _smoothing = 1f;
        [SerializeField] private float _limitY = 4.75f;

        public bool IsActive
        {
            get => enabled;
            set => enabled = value;
        }

        public void Move(float y)
        {
            if (!IsActive)
            {
                return;
            }
            
            var nextPosition = Vector3.Lerp(transform.position, transform.position + Vector3.up * (y * _speed), Time.deltaTime * _smoothing);
            nextPosition.y = Mathf.Clamp(nextPosition.y, -_limitY, _limitY);

            transform.position = nextPosition;
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var ball = collision.gameObject.GetComponent<BallView>();
            
            if (ball)
            {
                var ballDirection = ball.MoveDirection;
                var normal = collision.contacts.First().normal;
                var newDirection = Vector2.Reflect(ballDirection, normal);

                ball.Push(newDirection);
                ball.SpeedUp(BALL_SPEED_INCREASING_STEP);
            }
        }
    }
}
