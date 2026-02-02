using UnityEngine;

namespace Lukomor.Example.World
{
    public class MVVMExampleWorldWanderingObject : MonoBehaviour
    {
        [SerializeField] private float _radius = 2f;
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _pauseDuration = 0.5f;

        private Vector3 _startPos;
        private Vector3 _targetPosition;
        private float _pauseTimer;
        private bool _isMoving;

        private void Start()
        {
            _startPos = transform.position;
            PickNewTarget();
        }

        private void Update()
        {
            if (_pauseTimer > 0f)
            {
                _pauseTimer -= Time.deltaTime;
                return;
            }

            if (!_isMoving)
            {
                PickNewTarget();
            }

            transform.position = Vector3.MoveTowards(
                transform.position,
                _targetPosition,
                _speed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
            {
                _isMoving = false;
                _pauseTimer = _pauseDuration;
            }
        }

        private void PickNewTarget()
        {
            var rnd = Random.insideUnitCircle * _radius;
            _targetPosition = _startPos + new Vector3(rnd.x, 0f, rnd.y);
            _isMoving = true;
        }
    }
}