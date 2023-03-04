using System;
using System.Collections;

namespace UniTime.Tests {
    public sealed class UniTimeTimerProvider : IEnumerable {
        public IEnumerator GetEnumerator() {
            yield return (Func<IUniTimeProvider, IUniTimeTicker, DoubleBasedUniTimeTimer>)((timeProvider, ticker) =>
                new DoubleBasedUniTimeTimer(timeProvider, ticker));
#if UNITY_2023_1_OR_NEWER
            yield return (Func<IUniTimeProvider, IUniTimeTicker, LongBasedUniTimeTimer>)((timeProvider, ticker) =>
                new LongBasedUniTimeTimer(timeProvider, ticker));
#endif
        }
    }
}
