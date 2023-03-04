#if UNITY_2023_1_OR_NEWER
using Unity.IntegerTime;
#endif

namespace UniTime {
    public interface IUniTimeProvider {
        float Time { get; }
        double TimeAsDouble { get; }
#if UNITY_2023_1_OR_NEWER
        RationalTime TimeAsRational { get; }
#endif
        float TimeSinceLevelLoad { get; }
        double TimeSinceLevelLoadAsDouble { get; }
        float DeltaTime { get; }
        float FixedTime { get; }
        double FixedTimeAsDouble { get; }
        float UnscaledTime { get; }
        double UnscaledTimeAsDouble { get; }
        float FixedUnscaledTime { get; }
        double FixedUnscaledTimeAsDouble { get; }
        float UnscaledDeltaTime { get; }
        float FixedUnscaledDeltaTime { get; }
        float FixedDeltaTime { get; }
        float MaximumDeltaTime { get; }
        float SmoothDeltaTime { get; }
        float MaximumParticleDeltaTime { get; }
        float TimeScale { get; }
        int FrameCount { get; }
        int RenderedFrameCount { get; }
        float RealtimeSinceStartup { get; }
        double RealtimeSinceStartupAsDouble { get; }
        float CaptureDeltaTime { get; }
#if UNITY_2023_1_OR_NEWER
        RationalTime CaptureDeltaTimeRational { get; }
#endif
        int CaptureFramerate { get; }
        bool InFixedTimeStep { get; }
    }
}
