#if UNITY_2023_1_OR_NEWER
using System;
using Unity.IntegerTime;
#if UNITIME_VCONTAINER_SUPPORT
using VContainer;
#endif

namespace UniTime {
    public sealed class LongBasedUniTimeTimer : IUniTimeTimer, IDisposable {
        readonly IUniTimeProvider _timeProvider;
        readonly IUniTimeTicker _ticker;

        bool _enabled;
        bool _autoReset = true;
        double _interval = 100.0;
        long _stopTimeStamp;

#if UNITIME_VCONTAINER_SUPPORT
        [Inject]
#endif
        public LongBasedUniTimeTimer(IUniTimeProvider timeProvider, IUniTimeTicker ticker) {
            _timeProvider = timeProvider;
            _ticker = ticker;
        }

        public void Dispose() => _ticker.Tick -= Tick;

        public bool Enabled { get => _enabled; set => SetEnabled(value); }
        public bool AutoReset { get => _autoReset; set => SetAutoReset(value); }
        public double Interval { get => _interval; set => SetInterval(value); }

        public event UniTimeTimerElapsedEventHandler Elapsed;

        long GetTimeStamp() => _timeProvider.TimeAsRational.Count;

        void SetEnabled(bool value) {
            if (_enabled == value) return;
            if (!value) {
                _enabled = false;
                UpdateTimer();
                return;
            }

            UpdateTimer();
            _ticker.Tick += Tick;
            _enabled = true;
        }

        void SetAutoReset(bool value) {
            if (_autoReset == value) return;
            _autoReset = value;
            UpdateTimer();
        }

        void SetInterval(double value) {
            if (value <= 0.0)
                throw new ArgumentException(
                    $"'{value}' is not a valid value for 'Interval'. 'Interval' must be greater than 0.");
            _interval = value;
            UpdateTimer();
        }

        void UpdateTimer() {
            if (!_autoReset) _enabled = false;
            if (!_enabled) _ticker.Tick -= Tick;
            _stopTimeStamp = GetTimeStamp()
                + RationalTime.FromDouble(_interval / 1000.0, _timeProvider.TimeAsRational.Ticks).Count;
        }

        public void Start() => Enabled = true;
        public void Stop() => Enabled = false;

        void Tick() {
            if (!_enabled) return;
            var time = GetTimeStamp();
            if (time < _stopTimeStamp) return;
            UpdateTimer();
            Elapsed?.Invoke(this, new UniTimeTimerElapsedEventArgs(
                new RationalTime(time, _timeProvider.TimeAsRational.Ticks).ToDouble()));
        }
    }
}
#endif
