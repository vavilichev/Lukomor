using System;
using System.Collections;
using UnityEngine;

namespace VavilichevGD.Tools {
    public class Routine : RoutineBase {

        public Func<IEnumerator> enumerator { get; }

        public Routine(Func<IEnumerator> enumerator) {
            this.enumerator = enumerator;
        }

        public Routine(Func<IEnumerator> enumerator, MonoBehaviour owner) {
            this.enumerator = enumerator;
            this.owner = owner;
        }

        private IEnumerator WorkRoutine() {
            this.NotifyAboutCoroutineStarted();
            yield return this.enumerator.Invoke();
            this.NotifyAboutCoroutineCanceled();
            this.routine = null;
        }


        public Coroutine Start() {
            this.Stop();

            if (owner == null)
                return this.routine = Coroutines.StartRoutine(WorkRoutine());

            return this.routine = this.owner.StartCoroutine(WorkRoutine());
        }

        public void Stop() {
            if (this.isActive) {
                if (owner == null)
                    Coroutines.StopRoutine(this.routine);
                else
                    this.owner.StopCoroutine(this.routine);

                NotifyAboutCoroutineCanceled();
                this.routine = null;
            }
        }
    }
}