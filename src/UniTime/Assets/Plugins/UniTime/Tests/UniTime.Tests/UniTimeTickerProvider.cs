using System;
using System.Collections;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace UniTime.Tests {
    public sealed class UniTimeTickerProvider : IEnumerable {
        public IEnumerator GetEnumerator() {
            yield return (Func<CoroutineUniTimeTicker>)(() =>
                new GameObject("UniTime Ticker").AddComponent<CoroutineUniTimeTicker>());
#if UNITIME_UNITASK_SUPPORT
            yield return (Func<AsyncUniTimeTicker>)(() =>
                new AsyncUniTimeTicker());
#endif
#if UNITIME_UNIRX_SUPPORT
            yield return (Func<MicroCoroutineUniTimeTicker>)(() =>
                new MicroCoroutineUniTimeTicker());
#endif
#if UNITIME_VCONTAINER_SUPPORT
            yield return (Func<TickableUniTimeTicker>)(() =>
                LifetimeScope.Create(builder => builder.RegisterEntryPoint<TickableUniTimeTicker>().AsSelf()).Container
                    .Resolve<TickableUniTimeTicker>());
#endif
        }
    }
}
