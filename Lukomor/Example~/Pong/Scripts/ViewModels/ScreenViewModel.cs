using System;
using System.Reactive;
using System.Reactive.Linq;
using Lukomor.MVVM;

namespace Lukomor.Example.Pong
{
    public abstract class ScreenViewModel : IViewModel
    {
        public IObservable<Unit> Closed { get; }

        private event Action<Unit> _closed;

        protected ScreenViewModel()
        {
            Closed = Observable.FromEvent<Unit>(a => _closed += a, a => _closed -= a);
        }
        
        public void Close()
        {
            _closed?.Invoke(Unit.Default);
        }
    }
}