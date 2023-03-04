#if UNITIME_UNIRX_SUPPORT
using System;
using System.Collections;
using System.Threading;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniTime {
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public sealed class MicroCoroutineUniTimeTicker : IUniTimeTicker, IDisposable {
        readonly IDisposable _disposable;

        public event Action Tick;

        public static Action<Exception> ExceptionHandler { get; set; } = Debug.LogException;

        public MicroCoroutineUniTimeTicker() {
            var moreCancel = new CancellationDisposable();
            var token = moreCancel.Token;
            MainThreadDispatcher.StartUpdateMicroCoroutine(TickRoutine(token));
            _disposable = moreCancel;
        }

        IEnumerator TickRoutine(CancellationToken cancellation) {
            while (!cancellation.IsCancellationRequested) {
                try {
                    Tick?.Invoke();
                } catch (Exception e) {
                    ExceptionHandler(e);
                }

                yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }

        public void Dispose() => _disposable.Dispose();
    }
}
#endif
