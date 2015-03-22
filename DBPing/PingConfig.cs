using System.Collections.Generic;

namespace DBPing
{
    public class PingConfig
    {
        public virtual List<Poller> Pollers { get; set; }
        public virtual int Interval { get; set; }
    }
}