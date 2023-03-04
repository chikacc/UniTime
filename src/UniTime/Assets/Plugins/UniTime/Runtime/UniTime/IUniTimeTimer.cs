namespace UniTime {
    public interface IUniTimeTimer {
        bool Enabled { get; set; }
        double Interval { get; set; }
        bool AutoReset { get; set; }
        event UniTimeTimerElapsedEventHandler Elapsed;
        void Start();
        void Stop();
    }
}
