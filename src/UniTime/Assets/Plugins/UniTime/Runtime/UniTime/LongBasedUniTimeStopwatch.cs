#if UNITY_2023_1_OR_NEWER
using System;
using Unity.IntegerTime;
#if UNITIME_VCONTAINER_SUPPORT
using VContainer;
#endif

namespace UniTime {
    public sealed class LongBasedUniTimeStopwatch : IUniTimeStopwatch {
        const long MillisecondsPerSecond = 1000L;

        readonly IUniTimeProvider _timeProvider;

        long _elapsed;
        long _startTimeStamp;

#if UNITIME_VCONTAINER_SUPPORT
        [Inject]
#endif
        public LongBasedUniTimeStopwatch(IUniTimeProvider timeProvider) => _timeProvider = timeProvider;

        public bool IsRunning { get; private set; }
        public TimeSpan Elapsed => TimeSpan.FromSeconds(GetElapsedSeconds());
        public double ElapsedSeconds => GetElapsedSeconds();
        public long ElapsedMilliseconds => (long)(GetElapsedSeconds() * MillisecondsPerSecond);
        public long ElapsedTicks => (long)GetElapsedTimeSpanTicks();

        double GetElapsedTimeSpanTicks() => GetElapsedSeconds() * TimeSpan.TicksPerSecond;

        double GetElapsedSeconds() =>
            new RationalTime(GetRawElapsed(), _timeProvider.TimeAsRational.Ticks).ToDouble();

        long GetRawElapsed() {
            if (!IsRunning) return _elapsed;
            return _elapsed + (GetTimeStamp() - _startTimeStamp);
        }

        long GetTimeStamp() => _timeProvider.TimeAsRational.Count;

        public void Start() {
            if (IsRunning) return;
            _startTimeStamp = GetTimeStamp();
            IsRunning = true;
        }

        public void Stop() {
            if (!IsRunning) return;
            _elapsed = GetRawElapsed();
            IsRunning = false;
        }

        public void Reset() {
            IsRunning = false;
            _elapsed = 0L;
            _startTimeStamp = default;
        }

        public void Restart() {
            _elapsed = 0L;
            _startTimeStamp = GetTimeStamp();
            IsRunning = true;
        }
    }
}
#endif
