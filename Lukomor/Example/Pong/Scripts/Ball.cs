using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lukomor.Example.Pong
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;

        public Vector3 Direction => _direction;

        private Vector3 _direction;
        private bool _isFlying = false;
        private Rigidbody2D _rb;
        
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            PushRandomDirection();
        }

        private void Update()
        {
            if (!_isFlying)
            {
                return;
            }
            
            transform.position += _direction.normalized * (Time.deltaTime * _speed);
        }

        public void PushRandomDirection()
        {
            var rX = Random.Range(0.3f, 1) * Random.Range(0, 2) == 0 ? 1 : -1;
            var rY = Random.Range(0.3f, 0.7f) * Random.Range(0, 2) == 0 ? 1 : -1;
            var rDirection = new Vector3(rX, rY);
            
            Push(rDirection);
        }
        
        public void Push(Vector3 direction)
        {
            _direction = direction.normalized;
            _isFlying = true;
        }

        public void Stop()
        {
            _isFlying = false;
        }
    }
}