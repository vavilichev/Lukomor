using UnityEngine;

namespace Lukomor.Example.Pong
{
    public class AIInputController : InputController
    {
        [SerializeField] private float _speed = 1f;
        
        private Transform _target;
        
        public void Bind(BlockView block, Transform target)
        {
            base.Bind(block);

            _target = target;
        }

        private void Update()
        {
            var myY = transform.position.y;
            var ballY = _target.position.y;
            var y = Mathf.Clamp(ballY - myY, -1, 1) * _speed;
            
            Block.Move(y);
        }
    }
}