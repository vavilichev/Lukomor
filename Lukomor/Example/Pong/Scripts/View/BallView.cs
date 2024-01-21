using UnityEngine;
using Random = UnityEngine.Random;

namespace Lukomor.Example.Pong
{
    public class BallView : MonoBehaviour
    {
        [SerializeField] private float _initialSpeed = 4f;
        [SerializeField] private Vector3 _initialPosition = Vector3.zero;

        public Vector3 MoveDirection => _moveDirection;

        private Vector3 _moveDirection;
        private float _speed = 1f;

        private void Start()
        {
            Restart();
        }

        private void Update()
        {
            transform.position += _moveDirection.normalized * (Time.deltaTime * _speed);
        }
        
        public void Push(Vector3 direction)
        {
            _moveDirection = direction.normalized;
        }

        public void SpeedUp(float speedIncreasingStep)
        {
            _speed += speedIncreasingStep;
        }

        public void Restart()
        {
            _speed = _initialSpeed;
            transform.position = _initialPosition;
            
            PushRandomDirection();
        }

        private void PushRandomDirection()
        {
            var rX = Random.Range(0.3f, 1) * Random.Range(0, 2) == 0 ? 1 : -1;
            var rY = Random.Range(0.3f, 0.7f) * Random.Range(0, 2) == 0 ? 1 : -1;
            var rDirection = new Vector3(rX, rY);
            
            Push(rDirection);
        }
    }
}