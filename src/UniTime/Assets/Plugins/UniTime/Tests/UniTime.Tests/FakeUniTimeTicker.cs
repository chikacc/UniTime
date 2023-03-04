using System;

namespace UniTime.Tests {
    public sealed class FakeUniTimeTicker : IUniTimeTicker {
        public event Action Tick;

        public void Raise() => Tick?.Invoke();
        public void Reset() => Tick = null;
    }
}
