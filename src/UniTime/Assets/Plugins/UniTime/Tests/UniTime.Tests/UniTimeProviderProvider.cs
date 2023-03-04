using System;
using System.Collections;

namespace UniTime.Tests {
    public sealed class UniTimeProviderProvider : IEnumerable {
        public IEnumerator GetEnumerator() {
            yield return (Func<UniTimeProvider>)(() => new UniTimeProvider());
        }
    }
}
