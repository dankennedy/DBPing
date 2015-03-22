using System;
using System.Data.SqlClient;
using System.Diagnostics;
using log4net;

namespace DBPing
{
    public class Poller
    {
        public virtual string Name { get; set; }
        public virtual string ConnectionString { get; set; }
        public virtual string Sql { get; set; }
        private ILog _log;

        public void Execute()
        {
            if (_log == null)
                _log = LogManager.GetLogger(Name);

            var stopwatch = Stopwatch.StartNew();
            try
            {
                using (var con = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(Sql, con))
                {
                    _log.Info("Connecting");
                    con.Open();
                    _log.InfoFormat("Connected {0} ms. Querying", stopwatch.Elapsed.TotalMilliseconds);
                    cmd.ExecuteNonQuery();
                    _log.InfoFormat("Queried {0} ms. Disconnecting", stopwatch.Elapsed.TotalMilliseconds);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Failed to execute. {0} ms. {1}", stopwatch.Elapsed.TotalMilliseconds, ex);
            }
            finally
            {
                _log.InfoFormat("Finished. {0} ms", stopwatch.Elapsed.TotalMilliseconds);
                stopwatch.Stop();
            }
        }
    }
}