using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Timers;
using log4net.Config;

namespace DBPing
{
    class Program
    {
        private static readonly Timer Timer = new Timer();
        private static PingConfig Config;

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            Config = (PingConfig)ConfigurationManager.GetSection("PingConfig");
            Timer.Interval = Config.Interval * 1000;
            Timer.Elapsed += (sender, e) => OnTick(sender, e);
            Timer.Enabled = true;
            Console.WriteLine("Running. Hit enter to quit");
            Console.ReadLine();
            Timer.Dispose();
        }

        private static void OnTick(object source, ElapsedEventArgs e)
        {
            Parallel.ForEach(Config.Pollers, x =>
            {
                Task.Run(delegate { x.Execute(); });
            });
        }
    }
}
