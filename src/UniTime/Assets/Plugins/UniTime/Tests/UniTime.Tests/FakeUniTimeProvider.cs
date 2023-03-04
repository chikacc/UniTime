#if UNITY_2023_1_OR_NEWER
using Unity.IntegerTime;
#endif

// ReSharper disable MemberCanBePrivate.Global

namespace UniTime.Tests {
    public sealed class FakeUniTimeProvider : IUniTimeProvider {
        public float Time { get; set; }
        public double TimeAsDouble { get; set; }
#if UNITY_2023_1_OR_NEWER
        public RationalTime TimeAsRational { get; set; }
#endif
        public float TimeSinceLevelLoad { get; set; }
        public double TimeSinceLevelLoadAsDouble { get; set; }
        public float DeltaTime { get; set; }
        public float FixedTime { get; set; }
        public double FixedTimeAsDouble { get; set; }
        public float UnscaledTime { get; set; }
        public double UnscaledTimeAsDouble { get; set; }
        public float FixedUnscaledTime { get; set; }
        public double FixedUnscaledTimeAsDouble { get; set; }
        public float UnscaledDeltaTime { get; set; }
        public float FixedUnscaledDeltaTime { get; set; }
        public float FixedDeltaTime { get; set; }
        public float MaximumDeltaTime { get; set; }
        public float SmoothDeltaTime { get; set; }
        public float MaximumParticleDeltaTime { get; set; }
        public float TimeScale { get; set; }
        public int FrameCount { get; set; }
        public int RenderedFrameCount { get; set; }
        public float RealtimeSinceStartup { get; set; }
        public double RealtimeSinceStartupAsDouble { get; set; }
        public float CaptureDeltaTime { get; set; }
#if UNITY_2023_1_OR_NEWER
        public RationalTime CaptureDeltaTimeRational { get; set; }
#endif
        public int CaptureFramerate { get; set; }
        public bool InFixedTimeStep { get; set; }

        public void Set(double time) {
            TimeAsDouble = time;
#if UNITY_2023_1_OR_NEWER
            TimeAsRational = RationalTime.FromDouble(time, RationalTime.TicksPerSecond.DefaultTicksPerSecond);
#endif
        }

        public void Reset() {
            Time = default;
            TimeAsDouble = default;
#if UNITY_2023_1_OR_NEWER
            TimeAsRational = RationalTime.FromDouble(default, RationalTime.TicksPerSecond.DefaultTicksPerSecond);
#endif
            TimeSinceLevelLoad = default;
            TimeSinceLevelLoadAsDouble = default;
            DeltaTime = default;
            FixedTime = default;
            FixedTimeAsDouble = default;
            UnscaledTime = default;
            UnscaledTimeAsDouble = default;
            FixedUnscaledTime = default;
            FixedUnscaledTimeAsDouble = default;
            UnscaledDeltaTime = default;
            FixedUnscaledDeltaTime = default;
            FixedDeltaTime = default;
            MaximumDeltaTime = default;
            SmoothDeltaTime = default;
            MaximumParticleDeltaTime = default;
            TimeScale = default;
            FrameCount = default;
            RenderedFrameCount = default;
            RealtimeSinceStartup = default;
            RealtimeSinceStartupAsDouble = default;
            CaptureDeltaTime = default;
#if UNITY_2023_1_OR_NEWER
            CaptureDeltaTimeRational =
                RationalTime.FromDouble(default, RationalTime.TicksPerSecond.DefaultTicksPerSecond);
#endif
            CaptureFramerate = default;
            InFixedTimeStep = default;
        }
    }
}
