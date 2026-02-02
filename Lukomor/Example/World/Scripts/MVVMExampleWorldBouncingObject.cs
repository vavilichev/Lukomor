using System;
using Lukomor.MVVM.Binders;
using UnityEngine;

namespace Lukomor.Example.World
{
    public class MVVMExampleWorldBouncingObject : MonoBehaviour
    {
        [SerializeField] private float _height = 0.5f;
        [SerializeField] private float _speed = 2f;
        [SerializeField] private ObservableBinder<Vector3> _positionBinder;

        private Vector3 _startPos;
        private IDisposable _subscription;
        private float _localTime;

        private void Start()
        {
            _subscription = _positionBinder.OutputStream.Subscribe(position => { _startPos = position; });
        }

        private void Update()
        {
            _localTime += Time.deltaTime;
            var y = Mathf.Sin(_localTime * _speed) * _height;
            transform.position = _startPos + Vector3.up * y;
        }
    }
}