using System;
using System.Collections;

namespace UniTime.Tests {
    public sealed class UniTimeStopwatchProvider : IEnumerable {
        public IEnumerator GetEnumerator() {
            yield return (Func<IUniTimeProvider, DoubleBasedUniTimeStopwatch>)(timeProvider =>
                new DoubleBasedUniTimeStopwatch(timeProvider));
#if UNITY_2023_1_OR_NEWER
            yield return (Func<IUniTimeProvider, LongBasedUniTimeStopwatch>)(timeProvider =>
                new LongBasedUniTimeStopwatch(timeProvider));
#endif
        }
    }
}
