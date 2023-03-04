using System;

namespace UniTime {
    public interface IUniTimeStopwatch {
        bool IsRunning { get; }
        TimeSpan Elapsed { get; }
        double ElapsedSeconds { get; }
        long ElapsedMilliseconds { get; }
        long ElapsedTicks { get; }
        void Start();
        void Stop();
        void Reset();
        void Restart();
    }
}
