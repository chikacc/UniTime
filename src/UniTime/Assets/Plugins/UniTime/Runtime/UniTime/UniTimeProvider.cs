#if UNITY_2023_1_OR_NEWER
using Unity.IntegerTime;
#endif
using JetBrains.Annotations;
using static UnityEngine.Time;

namespace UniTime {
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public sealed class UniTimeProvider : IUniTimeProvider {
        public float Time => time;
        public double TimeAsDouble => timeAsDouble;
#if UNITY_2023_1_OR_NEWER
        public RationalTime TimeAsRational => timeAsRational;
#endif
        public float TimeSinceLevelLoad => timeSinceLevelLoad;
        public double TimeSinceLevelLoadAsDouble => timeSinceLevelLoadAsDouble;
        public float DeltaTime => deltaTime;
        public float FixedTime => fixedTime;
        public double FixedTimeAsDouble => fixedTimeAsDouble;
        public float UnscaledTime => unscaledTime;
        public double UnscaledTimeAsDouble => unscaledTimeAsDouble;
        public float FixedUnscaledTime => fixedUnscaledTime;
        public double FixedUnscaledTimeAsDouble => fixedUnscaledTimeAsDouble;
        public float UnscaledDeltaTime => unscaledDeltaTime;
        public float FixedUnscaledDeltaTime => fixedUnscaledDeltaTime;
        public float FixedDeltaTime => fixedDeltaTime;
        public float MaximumDeltaTime => maximumDeltaTime;
        public float SmoothDeltaTime => smoothDeltaTime;
        public float MaximumParticleDeltaTime => maximumParticleDeltaTime;
        public float TimeScale => timeScale;
        public int FrameCount => frameCount;
        public int RenderedFrameCount => renderedFrameCount;
        public float RealtimeSinceStartup => realtimeSinceStartup;
        public double RealtimeSinceStartupAsDouble => realtimeSinceStartupAsDouble;
        public float CaptureDeltaTime => captureDeltaTime;
#if UNITY_2023_1_OR_NEWER
        public RationalTime CaptureDeltaTimeRational => captureDeltaTimeRational;
#endif
        public int CaptureFramerate => captureFramerate;
        public bool InFixedTimeStep => inFixedTimeStep;
    }
}
