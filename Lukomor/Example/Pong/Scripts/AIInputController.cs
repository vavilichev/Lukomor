using System;
using UnityEngine;

namespace Lukomor.Example.Pong
{
    public class AIInputController : InputController
    {
        [SerializeField] private float _speed = 1f;
        
        private Ball _ball;
        
        public void Bind(Block block, Ball ball)
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