using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace UniTime {
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public sealed class CoroutineUniTimeTicker : MonoBehaviour, IUniTimeTicker, IDisposable {
        Coroutine _tickCoroutine;

        public event Action Tick;

        public static Action<Exception> ExceptionHandler { get; set; } = Debug.LogException;

        void OnEnable() => EnsureTickCoroutineStarted();
        void OnDisable() => EnsureTickCoroutineStopped();

        public void Dispose() {
            EnsureTickCoroutineStopped();
            if (this) Destroy(gameObject);
        }

        IEnumerator TickRoutine() {
            while (true) {
                try {
                    Tick?.Invoke();
                } catch (Exception e) {
                    ExceptionHandler(e);
                }

                yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }

        void EnsureTickCoroutineStarted() => _tickCoroutine ??= StartCoroutine(TickRoutine());

        void EnsureTickCoroutineStopped() {
            if (_tickCoroutine is null) return;
            StopCoroutine(_tickCoroutine);
            _tickCoroutine = null;
        }
    }
}
