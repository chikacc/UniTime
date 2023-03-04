using System;
#if UNITIME_VCONTAINER_SUPPORT
using VContainer;
#endif

namespace UniTime {
    public sealed class DoubleBasedUniTimeTimer : IUniTimeTimer, IDisposable {
        readonly IUniTimeProvider _timeProvider;
        readonly IUniTimeTicker _ticker;

        bool _enabled;
        bool _autoReset = true;
        double _interval = 100.0;
        double _stopTimeStamp;

#if UNITIME_VCONTAINER_SUPPORT
        [Inject]
#endif
        public DoubleBasedUniTimeTimer(IUniTimeProvider timeProvider, IUniTimeTicker ticker) {
            _timeProvider = timeProvider;
            _ticker = ticker;
        }

        public void Dispose() => _ticker.Tick -= Tick;

        public bool Enabled { get => _enabled; set => SetEnabled(value); }
        public bool AutoReset { get => _autoReset; set => SetAutoReset(value); }
        public double Interval { get => _interval; set => SetInterval(value); }

        public event UniTimeTimerElapsedEventHandler Elapsed;

        double GetTimeStamp() => _timeProvider.TimeAsDouble;

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
            _stopTimeStamp = GetTimeStamp() + _interval / 1000.0;
        }

        public void Start() => Enabled = true;
        public void Stop() => Enabled = false;

        public void Tick() {
            if (!_enabled) return;
            var time = GetTimeStamp();
            if (time < _stopTimeStamp) return;
            UpdateTimer();
            Elapsed?.Invoke(this, new UniTimeTimerElapsedEventArgs(time));
        }
    }
}
