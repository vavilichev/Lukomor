using System;
using UnityEngine;

namespace Lukomor.Example.Pong
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _limitY = 4.75f;
        
        public void Move(float y)
        {
            var nextPosition = transform.position + Vector3.up * (y * Time.deltaTime * _speed);
            nextPosition.y = Mathf.Clamp(nextPosition.y, -_limitY, _limitY);

            transform.position = nextPosition;
        }
    }
}
