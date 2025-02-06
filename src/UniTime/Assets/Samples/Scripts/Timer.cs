using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UniTime.Samples {
    [DisallowMultipleComponent]
    public class Timer : MonoBehaviour {
        [SerializeField] CanvasGroup _intervalGroup;
        [SerializeField] NumberPicker _hours;
        [SerializeField] NumberPicker _minutes;
        [SerializeField] NumberPicker _seconds;
        [SerializeField] CanvasGroup _periodGroup;
        [SerializeField] Annulus _annulus;
        [SerializeField] TMP_Text _period;
        [SerializeField] TMP_Text _startOrPauseOrResume;
        [SerializeField] Button _startOrPauseOrResumeButton;
        [SerializeField] Button _cancelButton;
        [SerializeField] UnityEvent _timesUp;
        [SerializeField] string _monospace = "0.6em";

        UniTimeProvider _timeProvider;
        LongBasedUniTimeTimer _timer;
        LongBasedUniTimeStopwatch _stopwatch;
        bool _started;

        void Start() {
            _hours.Value = PlayerPrefs.GetInt(
                $"{nameof(Timer)}.{nameof(_hours)}.{nameof(_hours.Value)}", 0);
            _minutes.Value = PlayerPrefs.GetInt(
                $"{nameof(Timer)}.{nameof(_minutes)}.{nameof(_minutes.Value)}", 0);
            _seconds.Value = PlayerPrefs.GetInt(
                $"{nameof(Timer)}.{nameof(_seconds)}.{nameof(_seconds.Value)}", 0);
        }

        void Update() {
            if (_timer.Enabled) UpdatePeriod();
        }

        void OnEnable() {
            if (_timer is null) {
                _timeProvider = new UniTimeProvider();
#if UNITY_2023_1_OR_NEWER
                var ticker = FindAnyObjectByType<CoroutineUniTimeTicker>();
#else
                var ticker = FindObjectOfType<CoroutineUniTicker>();
#endif
                if (!ticker)
                    ticker = new GameObject { hideFlags = HideFlags.HideAndDontSave }
                        .AddComponent<CoroutineUniTimeTicker>();
                _timer = new LongBasedUniTimeTimer(_timeProvider, ticker);
                _stopwatch = new LongBasedUniTimeStopwatch(_timeProvider);
                _timer.AutoReset = false;
                _timer.Elapsed += TimerElapsed;
            }

            Refresh();
        }

        void OnDestroy() {
            PlayerPrefs.SetInt($"{nameof(Timer)}.{nameof(_hours)}.{nameof(_hours.Value)}", _hours.Value);
            PlayerPrefs.SetInt($"{nameof(Timer)}.{nameof(_minutes)}.{nameof(_minutes.Value)}",
                _minutes.Value);
            PlayerPrefs.SetInt($"{nameof(Timer)}.{nameof(_seconds)}.{nameof(_seconds.Value)}",
                _seconds.Value);
            PlayerPrefs.Save();
        }

        float GetInterval() => (_hours.Value * 3600 + _minutes.Value * 60 + _seconds.Value) * 1000f;

        string GetPeriodString(double value) {
            var seconds = (int)Math.Round(value);
            var minutes = Math.DivRem(seconds, 60, out seconds);
            var hours = Math.DivRem(minutes, 60, out minutes);
            return hours < 1 ? $"<mspace={_monospace}>{minutes:00}:{seconds:00}</mspace>"
                : $"<mspace={_monospace}>{hours}:{minutes:00}:{seconds:00}</mspace>";
        }

        void UpdatePeriod() {
            var seconds = GetInterval() / 1000.0;
            var period = seconds - _stopwatch.ElapsedSeconds;
            _period.text = GetPeriodString(period);
            _annulus.FillAmount = (float)(period / seconds);
        }

        public void Refresh() {
            if (_started) UpdatePeriod();
            _intervalGroup.enabled = _started;
            _hours.Interactable = !_started;
            _minutes.Interactable = !_started;
            _seconds.Interactable = !_started;
            _periodGroup.enabled = !_started;
            var timerEnabled = _timer?.Enabled ?? false;
            _startOrPauseOrResume.text = _started ? timerEnabled ? "Pause" : "Resume" : "Start";
            _startOrPauseOrResumeButton.targetGraphic.color =
                timerEnabled ? new Color(1f, 0.7f, 0.2f) : new Color(0.2f, 1f, 0.4f);
            _startOrPauseOrResumeButton.interactable = _started || GetInterval() > Mathf.Epsilon;
            _cancelButton.interactable = _started;
        }

        public void StartOrPauseOrResume() {
            if (!_started) {
                _timer.Interval = GetInterval();
                _timer.Start();
                _stopwatch.Restart();
                _started = true;
                Refresh();
                return;
            }

            if (!_timer.Enabled) {
                var interval = GetInterval() - _stopwatch.ElapsedMilliseconds;
                if (interval <= 0) {
                    _started = false;
                    Refresh();
                    _timesUp?.Invoke();
                    return;
                }

                _timer.Interval = interval;
                _timer.Start();
                _stopwatch.Start();
                Refresh();
                return;
            }

            _timer.Stop();
            _stopwatch.Stop();
            Refresh();
        }

        public void Stop() {
            _timer.Stop();
            _stopwatch.Reset();
            _started = false;
            Refresh();
        }

        void TimerElapsed(object sender, UniTimeTimerElapsedEventArgs args) {
            _started = false;
            Refresh();
            _timesUp?.Invoke();
        }
    }
}
