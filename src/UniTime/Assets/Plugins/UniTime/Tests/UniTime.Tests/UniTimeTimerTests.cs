using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.TestTools;
#if UNITY_2023_1_OR_NEWER
using Unity.IntegerTime;
#endif

namespace UniTime.Tests {
    [RequiresPlayMode(false)]
    [TestFixtureSource(typeof(UniTimeTimerProvider))]
    public sealed class UniTimeTimerTests {
        [OneTimeSetUp]
        public void OneTimeSetUp() {
            _timeProvider.Reset();
            _ticker.Reset();
        }

        [TearDown]
        public void TearDown() {
            _timeProvider.Reset();
            _ticker.Reset();
            _timer.Enabled = false;
            _timer.Interval = 100.0;
            _timer.AutoReset = true;
        }

#if UNITY_2023_1_OR_NEWER
        const double MaxSeconds = DiscreteTime.MaxValueSeconds;
#else
        const double MaxSeconds = 922337203685476.5;
#endif
        const double PrecisionMilliseconds = 1E-05;

        readonly FakeUniTimeProvider _timeProvider;
        readonly FakeUniTimeTicker _ticker;
        readonly IUniTimeTimer _timer;

        public UniTimeTimerTests(Func<IUniTimeProvider, IUniTimeTicker, IUniTimeTimer> createTimer) {
            _timeProvider = new FakeUniTimeProvider();
            _ticker = new FakeUniTimeTicker();
            _timer = createTimer(_timeProvider, _ticker);
        }

        [Test]
        public void Enabled() {
            Assert.That(_timer.Enabled,
                Is.EqualTo(false));

            _timer.Enabled = true;
            Assert.That(_timer.Enabled,
                Is.EqualTo(true));

            _timer.Enabled = false;
            Assert.That(_timer.Enabled,
                Is.EqualTo(false));

            _timer.Start();
            Assert.That(_timer.Enabled,
                Is.EqualTo(true));

            _timer.Stop();
            Assert.That(_timer.Enabled,
                Is.EqualTo(false));

            _timer.Start();
            ++_timer.Interval;
            Assert.That(_timer.Enabled,
                Is.EqualTo(true));

            _timer.AutoReset = false;
            ++_timer.Interval;
            Assert.That(_timer.Enabled,
                Is.EqualTo(false));
        }

        [TestCase(double.Epsilon)]
        [TestCase(MaxSeconds)]
        public void Interval(double interval) {
            _timer.Interval = interval;
            Assert.That(_timer.Interval,
                Is.EqualTo(interval).Within(PrecisionMilliseconds));
        }

        [TestCase(double.MinValue)]
        [TestCase(0.0)]
        public void InvalidInterval(double interval) {
                Assert.Throws<ArgumentException>(() => _timer.Interval = interval,
                    $"'{interval}' is not a valid value for 'Interval'. 'Interval' must be greater than 0.");
        }

        [Test]
        public void AutoReset() {
            Assert.That(_timer.AutoReset,
                Is.EqualTo(true));

            _timer.AutoReset = false;
            Assert.That(_timer.AutoReset,
                Is.EqualTo(false));

            _timer.AutoReset = true;
            Assert.That(_timer.AutoReset,
                Is.EqualTo(true));
        }

        [TestCase(PrecisionMilliseconds + double.Epsilon)]
        [TestCase(MaxSeconds)]
        public void Elapsed(double interval) {
            _timer.Interval = interval;
            var receiver = new Stack<(object Sender, UniTimeTimerElapsedEventArgs Args)>();
            _timer.Elapsed += (sender, args) => receiver.Push((sender, args));
            _timer.Start();
            _ticker.Raise();
            Assert.That(receiver.Count,
                Is.EqualTo(0));

            _timeProvider.Set(_timeProvider.TimeAsDouble + interval / 1000.0);
            _ticker.Raise();
            Assert.That(receiver.Count,
                Is.EqualTo(1));

            Assert.That(receiver.Peek().Sender,
                Is.EqualTo(_timer));

            Assert.That(receiver.Peek().Args.SignalTime,
                Is.EqualTo(new UniTimeTimerElapsedEventArgs(_timeProvider.TimeAsDouble).SignalTime));
        }
    }
}
