using System;
using System.Collections;
using UnityEngine;

namespace VavilichevGD.Tools {
    public class RoutineWithArg<T> : RoutineBase {
        
        public Func<T, IEnumerator> enumerator { get; }

        public RoutineWithArg(Func<T, IEnumerator> enumerator) {
            this.enumerator = enumerator;
        }

        public RoutineWithArg(Func<T, IEnumerator> enumerator, MonoBehaviour owner) {
            this.enumerator = enumerator;
            this.owner = owner;
        }

        private IEnumerator WorkRoutine(T arg) {
            this.NotifyAboutCoroutineStarted();
            yield return this.enumerator.Invoke(arg);
            this.NotifyAboutCoroutineCanceled();
            this.routine = null;
        }


        public Coroutine Start(T arg) {
            this.Stop();

            if (owner == null)
                return this.routine = Coroutines.StartRoutine(WorkRoutine(arg));

            return this.routine = this.owner.StartCoroutine(WorkRoutine(arg));
        }

        public void Stop() {
            if (this.isActive) {
                if (owner == null)
                    Coroutines.StopRoutine(this.routine);
                else
                    this.owner.StopCoroutine(this.routine);

                this.NotifyAboutCoroutineCanceled();
                this.routine = null;
            }
        }
    }
}