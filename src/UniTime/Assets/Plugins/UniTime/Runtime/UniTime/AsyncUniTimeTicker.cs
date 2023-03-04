#if UNITIME_UNITASK_SUPPORT
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UniTime {
    public sealed class AsyncUniTimeTicker : IUniTimeTicker, IDisposable {
        readonly CancellationTokenSource _cancellationTokenSource;
        public event Action Tick;

        public static Action<Exception> ExceptionHandler { get; set; } = Debug.LogException;

        public AsyncUniTimeTicker() {
            _cancellationTokenSource = new CancellationTokenSource();
            Run(_cancellationTokenSource.Token).Forget();
        }

        async UniTaskVoid Run(CancellationToken cancellation) {
            while (!cancellation.IsCancellationRequested) {
                try {
                    Tick?.Invoke();
                } catch (Exception e) {
                    ExceptionHandler(e);
                }

                await UniTask.Yield();
            }
            // ReSharper disable once FunctionNeverReturns
        }

        public void Dispose() {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}
#endif
