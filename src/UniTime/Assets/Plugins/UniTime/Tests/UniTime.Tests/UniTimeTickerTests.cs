using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace UniTime.Tests {
    [RequiresPlayMode]
    [TestFixtureSource(typeof(UniTimeTickerProvider))]
    public sealed class UniTimeTickerTests {
        [OneTimeTearDown]
        public void OneTimeTearDown() { (_timeTicker as IDisposable)?.Dispose(); }

        readonly IUniTimeTicker _timeTicker;

        public UniTimeTickerTests(Func<IUniTimeTicker> createTimeTicker) => _timeTicker = createTimeTicker();

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public async Task Tick(int times) {
            await UniTask.Yield(PlayerLoopTiming.Update, CancellationToken.None);
            var count = 0;
            _timeTicker.Tick += () => ++count;
            for (var i = 0; i < times; ++i) await UniTask.Yield(PlayerLoopTiming.Update, CancellationToken.None);
            Assert.That(count,
                Is.EqualTo(times));
        }
    }
}
