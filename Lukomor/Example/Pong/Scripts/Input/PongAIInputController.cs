using UnityEngine;

namespace Lukomor.Example.Pong
{
    public class PongAIInputController : PongInputController
    {
        [SerializeField] private float _speed = 1f;
        
        private PongBallView _ball;
        
        public void Bind(PongBlockView block, PongBallView ball)
        {
            base.Bind(block);

            _ball = ball;
        }

        private void Update()
        {
            var myY = transform.position.y;
            var ballY = _ball.transform.position.y;
            var y = Mathf.Clamp(ballY - myY, -1, 1) * _speed;
            
            Block.Move(y);
        }
    }
}