using System;
using System.Reactive.Subjects;
using Lukomor.MVVM;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lukomor.Example.World
{
    public abstract class ObjectViewModel : ViewModel
    {
        private readonly BehaviorSubject<string> _name = new(string.Empty);
        private readonly BehaviorSubject<Vector3> _initialPosition = new(Vector3.zero);
        
        public IObservable<string> Name => _name;
        public IObservable<Vector3> InitialPosition => _initialPosition;

        protected ObjectViewModel(string name)
        {
            _name.OnNext(name);

            var rPosX = Random.Range(-5, 6);
            var rPosZ = Random.Range(-5, 6);
            var rPos = new Vector3(rPosX, 0, rPosZ);
            _initialPosition.OnNext(rPos);
        }
    }
}