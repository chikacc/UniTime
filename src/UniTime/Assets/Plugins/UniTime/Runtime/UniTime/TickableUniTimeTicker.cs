#if UNITIME_VCONTAINER_SUPPORT
using System;
using JetBrains.Annotations;
using UnityEngine;
using VContainer.Unity;

namespace UniTime {
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public sealed class TickableUniTimeTicker : IUniTimeTicker, ITickable, IDisposable {
        bool _disposed;

        public event Action Tick;

        public static Action<Exception> ExceptionHandler { get; set; } = Debug.LogException;

        public void Dispose() => _disposed = true;

        void ITickable.Tick() {
            if (_disposed) return;
            try {
                Tick?.Invoke();
            } catch (Exception e) {
                ExceptionHandler(e);
            }
        }
    }
}
#endif
