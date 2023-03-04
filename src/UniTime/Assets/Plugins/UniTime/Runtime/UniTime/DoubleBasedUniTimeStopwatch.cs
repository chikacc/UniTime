using System;
#if UNITIME_VCONTAINER_SUPPORT
using VContainer;
#endif

namespace UniTime {
    public sealed class DoubleBasedUniTimeStopwatch : IUniTimeStopwatch {
        const long MillisecondsPerSecond = 1000L;

        readonly IUniTimeProvider _timeProvider;

        double _elapsed;
        double _startTimeStamp;

#if UNITIME_VCONTAINER_SUPPORT
        [Inject]
#endif
        public DoubleBasedUniTimeStopwatch(IUniTimeProvider timeProvider) => _timeProvider = timeProvider;

        public bool IsRunning { get; private set; }
        public TimeSpan Elapsed => TimeSpan.FromSeconds(GetElapsedSeconds());
        public double ElapsedSeconds => GetElapsedSeconds();
        public long ElapsedMilliseconds => (long)(GetElapsedSeconds() * MillisecondsPerSecond);
        public long ElapsedTicks => (long)GetElapsedTimeSpanTicks();

        double GetElapsedTimeSpanTicks() => GetElapsedSeconds() * TimeSpan.TicksPerSecond;
        double GetElapsedSeconds() => GetRawElapsedSeconds();

        double GetRawElapsedSeconds() {
            if (!IsRunning) return _elapsed;
            return _elapsed + (GetTimeStamp() - _startTimeStamp);
        }

        double GetTimeStamp() => _timeProvider.TimeAsDouble;

        public void Start() {
            if (IsRunning) return;
            _startTimeStamp = GetTimeStamp();
            IsRunning = true;
        }

        public void Stop() {
            if (!IsRunning) return;
            _elapsed = GetRawElapsedSeconds();
            IsRunning = false;
        }

        public void Reset() {
            IsRunning = false;
            _elapsed = 0.0;
            _startTimeStamp = 0.0;
        }

        public void Restart() {
            _elapsed = 0.0;
            _startTimeStamp = GetTimeStamp();
            IsRunning = true;
        }
    }
}
