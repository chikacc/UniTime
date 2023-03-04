using System;

namespace UniTime {
    public interface IUniTimeTicker {
        event Action Tick;
    }
}
