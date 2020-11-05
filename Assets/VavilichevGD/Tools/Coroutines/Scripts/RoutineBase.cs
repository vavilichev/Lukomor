using UnityEngine;

namespace VavilichevGD.Tools {
    public abstract class RoutineBase {

        public delegate void RoutineHandler(RoutineBase routineBase, Coroutine routine);

        public event RoutineHandler OnStartedEvent;
        public event RoutineHandler OnCanceledEvent;
        
        public MonoBehaviour owner { get; protected set; }
        public Coroutine routine { get; protected set; }
        
        public bool isActive => routine != null;

        protected void NotifyAboutCoroutineStarted() {
            this.OnStartedEvent?.Invoke(this, this.routine);
        }

        protected void NotifyAboutCoroutineCanceled() {
            this.OnCanceledEvent?.Invoke(this, this.routine);
        }
    }
}