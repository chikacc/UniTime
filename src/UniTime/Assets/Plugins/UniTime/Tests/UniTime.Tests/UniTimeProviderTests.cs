using System;
using NUnit.Framework;
using UnityEngine.TestTools;
using static UnityEngine.Time;

namespace UniTime.Tests {
    [RequiresPlayMode]
    [TestFixtureSource(typeof(UniTimeProviderProvider))]
    public sealed class UniTimeProviderTests {
        const float Precision = 1E-05f;

        readonly IUniTimeProvider _timeProvider;

        public UniTimeProviderTests(Func<IUniTimeProvider> createTimeProvider) => _timeProvider = createTimeProvider();

        [Test]
        public void Time() =>
            Assert.That(_timeProvider.Time,
                Is.EqualTo(time).Within(Precision));

        [Test]
        public void TimeAsDouble() =>
            Assert.That(_timeProvider.TimeAsDouble,
                Is.EqualTo(timeAsDouble).Within(Precision));

#if UNITY_2023_1_OR_NEWER
        [Test]
        public void TimeAsRational() =>
            Assert.That(_timeProvider.TimeAsRational,
                Is.EqualTo(timeAsRational));
#endif

        [Test]
        public void TimeSinceLevelLoad() =>
            Assert.That(_timeProvider.TimeSinceLevelLoad,
                Is.EqualTo(timeSinceLevelLoad).Within(Precision));

        [Test]
        public void TimeSinceLevelLoadAsDouble() =>
            Assert.That(_timeProvider.TimeSinceLevelLoadAsDouble,
                Is.EqualTo(timeSinceLevelLoadAsDouble).Within(Precision));

        [Test]
        public void DeltaTime() =>
            Assert.That(_timeProvider.DeltaTime,
                Is.EqualTo(deltaTime).Within(Precision));

        [Test]
        public void FixedTime() =>
            Assert.That(_timeProvider.FixedTime,
                Is.EqualTo(fixedTime).Within(Precision));

        [Test]
        public void FixedTimeAsDouble() =>
            Assert.That(_timeProvider.FixedTimeAsDouble,
                Is.EqualTo(fixedTimeAsDouble).Within(Precision));

        [Test]
        public void UnscaledTime() =>
            Assert.That(_timeProvider.UnscaledTime,
                Is.EqualTo(unscaledTime).Within(Precision));

        [Test]
        public void UnscaledTimeAsDouble() =>
            Assert.That(_timeProvider.UnscaledTimeAsDouble,
                Is.EqualTo(unscaledTimeAsDouble).Within(Precision));

        [Test]
        public void FixedUnscaledTime() =>
            Assert.That(_timeProvider.FixedUnscaledTime,
                Is.EqualTo(fixedUnscaledTime).Within(Precision));

        [Test]
        public void FixedUnscaledTimeAsDouble() =>
            Assert.That(_timeProvider.FixedUnscaledTimeAsDouble,
                Is.EqualTo(fixedUnscaledTimeAsDouble).Within(Precision));

        [Test]
        public void UnscaledDeltaTime() =>
            Assert.That(_timeProvider.UnscaledDeltaTime,
                Is.EqualTo(unscaledDeltaTime).Within(Precision));

        [Test]
        public void FixedUnscaledDeltaTime() =>
            Assert.That(_timeProvider.FixedUnscaledDeltaTime,
                Is.EqualTo(fixedUnscaledDeltaTime).Within(Precision));

        [Test]
        public void FixedDeltaTime() =>
            Assert.That(_timeProvider.FixedDeltaTime,
                Is.EqualTo(fixedDeltaTime).Within(Precision));

        [Test]
        public void MaximumDeltaTime() =>
            Assert.That(_timeProvider.MaximumDeltaTime,
                Is.EqualTo(maximumDeltaTime).Within(Precision));

        [Test]
        public void SmoothDeltaTime() =>
            Assert.That(_timeProvider.SmoothDeltaTime,
                Is.EqualTo(smoothDeltaTime).Within(Precision));

        [Test]
        public void MaximumParticleDeltaTime() =>
            Assert.That(_timeProvider.MaximumParticleDeltaTime,
                Is.EqualTo(maximumParticleDeltaTime).Within(Precision));

        [Test]
        public void TimeScale() =>
            Assert.That(_timeProvider.TimeScale,
                Is.EqualTo(timeScale).Within(Precision));

        [Test]
        public void FrameCount() =>
            Assert.That(_timeProvider.FrameCount,
                Is.EqualTo(frameCount));

        [Test]
        public void RenderedFrameCount() =>
            Assert.That(_timeProvider.RenderedFrameCount,
                Is.EqualTo(renderedFrameCount));

        [Test]
        public void RealtimeSinceStartup() =>
            Assert.That(_timeProvider.RealtimeSinceStartup,
                Is.EqualTo(realtimeSinceStartup).Within(Precision));

        [Test]
        public void RealtimeSinceStartupAsDouble() =>
            Assert.That(_timeProvider.RealtimeSinceStartupAsDouble,
                Is.EqualTo(realtimeSinceStartupAsDouble).Within(Precision));

        [Test]
        public void CaptureDeltaTime() =>
            Assert.That(_timeProvider.CaptureDeltaTime,
                Is.EqualTo(captureDeltaTime).Within(Precision));

#if UNITY_2023_1_OR_NEWER
        [Test]
        public void CaptureDeltaTimeRational() =>
            Assert.That(_timeProvider.CaptureDeltaTimeRational,
                Is.EqualTo(captureDeltaTimeRational));
#endif

        [Test]
        public void CaptureFramerate() =>
            Assert.That(_timeProvider.CaptureFramerate,
                Is.EqualTo(captureFramerate));

        [Test]
        public void InFixedTimeStep() =>
            Assert.That(_timeProvider.InFixedTimeStep,
                Is.EqualTo(inFixedTimeStep));
    }
}
