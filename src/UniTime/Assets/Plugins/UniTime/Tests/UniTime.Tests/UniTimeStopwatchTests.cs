using System;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace UniTime.Tests {
    [RequiresPlayMode(false)]
    [TestFixtureSource(typeof(UniTimeStopwatchProvider))]
    public sealed class UniTimeStopwatchTests {
        [OneTimeSetUp]
        public void OneTimeSetUp() => _timeProvider.Reset();

        [TearDown]
        public void TearDown() {
            _timeProvider.Reset();
            _stopwatch.Reset();
        }

        const double PrecisionSeconds = 1E-05;
        const long PrecisionMilliseconds = 1L;
        const long PrecisionTicks = 100L;

        readonly FakeUniTimeProvider _timeProvider;
        readonly IUniTimeStopwatch _stopwatch;

        public UniTimeStopwatchTests(Func<IUniTimeProvider, IUniTimeStopwatch> createStopwatch) {
            _timeProvider = new FakeUniTimeProvider();
            _stopwatch = createStopwatch(_timeProvider);
        }

        [Test]
        public void IsRunning() {
            Assert.That(_stopwatch.IsRunning,
                Is.EqualTo(false));

            _stopwatch.Start();
            Assert.That(_stopwatch.IsRunning,
                Is.EqualTo(true));

            _stopwatch.Stop();
            Assert.That(_stopwatch.IsRunning,
                Is.EqualTo(false));

            _stopwatch.Start();
            Assert.That(_stopwatch.IsRunning,
                Is.EqualTo(true));

            _stopwatch.Restart();
            Assert.That(_stopwatch.IsRunning,
                Is.EqualTo(true));

            _stopwatch.Reset();
            Assert.That(_stopwatch.IsRunning,
                Is.EqualTo(false));
        }

        [TestCase(0, 1, 2, 3, 4)]
        [TestCase(1, 2, 3, 4, 5)]
        public void Elapsed(double start, double pause, double resume, double restart, double reset) {
            Assert.That(_stopwatch.Elapsed,
                Is.EqualTo(TimeSpan.Zero).Within(PrecisionSeconds));

            _timeProvider.Set(start);
            Assert.That(_stopwatch.Elapsed,
                Is.EqualTo(TimeSpan.Zero).Within(PrecisionSeconds));

            _stopwatch.Start();
            _timeProvider.Set(pause);
            Assert.That(_stopwatch.Elapsed,
                Is.EqualTo(TimeSpan.FromSeconds(pause - start)).Within(PrecisionSeconds));

            _stopwatch.Stop();
            _timeProvider.Set(resume);
            Assert.That(_stopwatch.Elapsed,
                Is.EqualTo(TimeSpan.FromSeconds(pause - start)).Within(PrecisionSeconds));

            _stopwatch.Start();
            _timeProvider.Set(restart);
            Assert.That(_stopwatch.Elapsed,
                Is.EqualTo(TimeSpan.FromSeconds(restart - resume + (pause - start))).Within(PrecisionSeconds));

            _stopwatch.Restart();
            _timeProvider.Set(reset);
            Assert.That(_stopwatch.Elapsed,
                Is.EqualTo(TimeSpan.FromSeconds(reset - restart)).Within(PrecisionSeconds));

            _stopwatch.Reset();
            Assert.That(_stopwatch.Elapsed,
                Is.EqualTo(TimeSpan.Zero).Within(PrecisionSeconds));
        }

        [TestCase(0, 1, 2, 3, 4)]
        [TestCase(1, 2, 3, 4, 5)]
        public void ElapsedSeconds(double start, double pause, double resume, double restart, double reset) {
            Assert.That(_stopwatch.ElapsedSeconds,
                Is.EqualTo(0.0).Within(PrecisionMilliseconds));

            _timeProvider.Set(start);
            Assert.That(_stopwatch.ElapsedSeconds,
                Is.EqualTo(0.0).Within(PrecisionMilliseconds));

            _stopwatch.Start();
            _timeProvider.Set(pause);
            Assert.That(_stopwatch.ElapsedSeconds,
                Is.EqualTo(pause - start).Within(PrecisionMilliseconds));

            _stopwatch.Stop();
            _timeProvider.Set(resume);
            Assert.That(_stopwatch.ElapsedSeconds,
                Is.EqualTo(pause - start).Within(PrecisionMilliseconds));

            _stopwatch.Start();
            _timeProvider.Set(restart);
            Assert.That(_stopwatch.ElapsedSeconds,
                Is.EqualTo(restart - resume + (pause - start)).Within(PrecisionMilliseconds));

            _stopwatch.Restart();
            _timeProvider.Set(reset);
            Assert.That(_stopwatch.ElapsedSeconds,
                Is.EqualTo(reset - restart).Within(PrecisionMilliseconds));

            _stopwatch.Reset();
            Assert.That(_stopwatch.ElapsedSeconds,
                Is.EqualTo(0.0).Within(PrecisionMilliseconds));
        }

        [TestCase(0, 1, 2, 3, 4)]
        [TestCase(1, 2, 3, 4, 5)]
        public void ElapsedMilliseconds(double start, double pause, double resume, double restart, double reset) {
            Assert.That(_stopwatch.ElapsedMilliseconds,
                Is.EqualTo(0L).Within(PrecisionMilliseconds));

            _timeProvider.Set(start);
            Assert.That(_stopwatch.ElapsedMilliseconds,
                Is.EqualTo(0L).Within(PrecisionMilliseconds));

            _stopwatch.Start();
            _timeProvider.Set(pause);
            Assert.That(_stopwatch.ElapsedMilliseconds,
                Is.EqualTo((long)((pause - start) * 1000.0)).Within(PrecisionMilliseconds));

            _stopwatch.Stop();
            _timeProvider.Set(resume);
            Assert.That(_stopwatch.ElapsedMilliseconds,
                Is.EqualTo((long)((pause - start) * 1000.0)).Within(PrecisionMilliseconds));

            _stopwatch.Start();
            _timeProvider.Set(restart);
            Assert.That(_stopwatch.ElapsedMilliseconds,
                Is.EqualTo((long)((restart - resume + (pause - start)) * 1000.0)).Within(PrecisionMilliseconds));

            _stopwatch.Restart();
            _timeProvider.Set(reset);
            Assert.That(_stopwatch.ElapsedMilliseconds,
                Is.EqualTo((long)((reset - restart) * 1000.0)).Within(PrecisionMilliseconds));

            _stopwatch.Reset();
            Assert.That(_stopwatch.ElapsedMilliseconds,
                Is.EqualTo(0L).Within(PrecisionMilliseconds));
        }

        [TestCase(0, 1, 2, 3, 4)]
        [TestCase(1, 2, 3, 4, 5)]
        public void ElapsedTicks(double start, double pause, double resume, double restart, double reset) {
            Assert.That(_stopwatch.ElapsedTicks,
                Is.EqualTo(0L).Within(PrecisionTicks));

            _timeProvider.Set(start);
            Assert.That(_stopwatch.ElapsedTicks,
                Is.EqualTo(0L).Within(PrecisionTicks));

            _stopwatch.Start();
            _timeProvider.Set(pause);
            Assert.That(_stopwatch.ElapsedTicks,
                Is.EqualTo((long)((pause - start) * TimeSpan.TicksPerSecond)).Within(PrecisionTicks));

            _stopwatch.Stop();
            _timeProvider.Set(resume);
            Assert.That(_stopwatch.ElapsedTicks,
                Is.EqualTo((long)((pause - start) * TimeSpan.TicksPerSecond)).Within(PrecisionTicks));

            _stopwatch.Start();
            _timeProvider.Set(restart);
            Assert.That(_stopwatch.ElapsedTicks,
                Is.EqualTo((long)((restart - resume + (pause - start)) * TimeSpan.TicksPerSecond))
                    .Within(PrecisionTicks));

            _stopwatch.Restart();
            _timeProvider.Set(reset);
            Assert.That(_stopwatch.ElapsedTicks,
                Is.EqualTo((long)((reset - restart) * TimeSpan.TicksPerSecond)).Within(PrecisionTicks));

            _stopwatch.Reset();
            Assert.That(_stopwatch.ElapsedTicks,
                Is.EqualTo(0L).Within(PrecisionTicks));
        }
    }
}
