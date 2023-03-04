using System;

namespace UniTime {
    public sealed class UniTimeTimerElapsedEventArgs : EventArgs {
        internal UniTimeTimerElapsedEventArgs(double signalTime) => SignalTime = TimeSpan.FromSeconds(signalTime);

        public TimeSpan SignalTime { get; }
    }
}
