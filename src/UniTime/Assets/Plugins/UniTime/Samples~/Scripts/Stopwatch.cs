using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UniTime.Samples {
    [DisallowMultipleComponent]
    public class Stopwatch : MonoBehaviour {
        [SerializeField] TMP_Text _totalElapsed;
        [SerializeField] TMP_Text _startOrStop;
        [SerializeField] Button _startOrStopButton;
        [SerializeField] TMP_Text _lapOrReset;
        [SerializeField] Button _lapOrResetButton;
        [SerializeField] LapElement _lapTemplate;
        [SerializeField] Transform _lapHistoryContainer;
        [SerializeField] string _monospace = "0.6em";

        LongBasedUniTimeStopwatch _stopwatch;
        LapElement _currentLap;
        List<LapElement> _lapHistory;
        double _lastUpdateTime;
        double _offset;

        void Start() {
            var key = $"{nameof(Stopwatch)}.{nameof(_lapHistory)}";
            if (!PlayerPrefs.HasKey(key)) return;
            var history = PlayerPrefs.GetString(key).Split(',').Select(double.Parse).ToArray();
            for (var i = 0; i < history.Length - 1; i++) {
                _offset = history[i];
                NewLap(GetElapsedSeconds());
            }

            _offset = history[^1];
            UpdateTotalElapsed();
            Refresh();
        }

        void Update() {
            if (_stopwatch.IsRunning) UpdateTotalElapsed();
        }

        void OnEnable() {
            if (_stopwatch is null) {
                var timeProvider = new UniTimeProvider();
                _stopwatch = new LongBasedUniTimeStopwatch(timeProvider);
            }

            Refresh();
        }

        void OnDestroy() {
            var key = $"{nameof(Stopwatch)}.{nameof(_lapHistory)}";
            if (_lapHistory is null || _lapHistory.Count == 0) {
                PlayerPrefs.DeleteKey(key);
                return;
            }

            var seconds = _lapHistory.Select(x => x.StartTime).Concat(new[] { _lastUpdateTime });
            PlayerPrefs.SetString(key, string.Join(',', seconds));
            foreach (var lap in _lapHistory) Destroy(lap.gameObject);
        }

        double GetElapsedSeconds() => _stopwatch.ElapsedSeconds + _offset;

        string GetElapsedString(double value) {
            var centiseconds = (int)Math.Floor(value * 100.0);
            var seconds = Math.DivRem(centiseconds, 100, out centiseconds);
            var minutes = Math.DivRem(seconds, 60, out seconds);
            var hours = Math.DivRem(minutes, 60, out minutes);
            return hours < 1 ? $"<mspace={_monospace}>{minutes:00}:{seconds:00}.{centiseconds:00}</mspace>"
                : $"<mspace={_monospace}>{hours}:{minutes:00}:{seconds:00}.{centiseconds:00}</mspace>";
        }

        void UpdateTotalElapsed() {
            var elapsed = GetElapsedSeconds();
            _totalElapsed.text = GetElapsedString(elapsed);
            if (_currentLap) _currentLap.Elapsed = GetElapsedString(elapsed - _currentLap.StartTime);
            _lastUpdateTime = elapsed;
        }

        void Refresh() {
            _startOrStop.text = _stopwatch.IsRunning ? "Stop" : "Start";
            _startOrStopButton.targetGraphic.color =
                _stopwatch.IsRunning ? new Color(1f, 0.2f, 0.2f) : new Color(0.2f, 1f, 0.4f);
            var canReset = _lapHistory?.Count > 0;
            _lapOrReset.text = _stopwatch.IsRunning || !canReset ? "Lap" : "Reset";
            _lapOrResetButton.interactable = _stopwatch.IsRunning || canReset;
        }

        void NewLap(double startTime) {
            UpdateTotalElapsed();
            _lapHistory ??= new List<LapElement>();
            var component = Instantiate(_lapTemplate, _lapHistoryContainer);
            component.Lap = $"Lap {_lapHistory.Count + 1}";
            component.StartTime = startTime;
            _lapHistory.Add(component);
            _currentLap = component;
        }

        public void StartOrStop() {
            if (!_stopwatch.IsRunning) {
                _stopwatch.Start();
                if (_currentLap == null) NewLap(GetElapsedSeconds());
                Refresh();
                return;
            }

            _stopwatch.Stop();
            Refresh();
        }

        public void LapOrReset() {
            if (_stopwatch.IsRunning) {
                NewLap(GetElapsedSeconds());
                return;
            }

            _stopwatch.Reset();
            _offset = 0.0;
            _currentLap = null;
            foreach (var lap in _lapHistory) Destroy(lap.gameObject);
            _lapHistory.Clear();
            _totalElapsed.text = GetElapsedString(GetElapsedSeconds());
            Refresh();
        }
    }
}
