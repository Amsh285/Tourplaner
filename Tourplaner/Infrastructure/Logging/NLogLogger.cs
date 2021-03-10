using NLog;

namespace Tourplaner.Infrastructure.Logging
{
    public sealed class NLogLogger<T> : ILogger<T>
    {
        public NLogLogger()
        {
            this.logger = LogManager.GetLogger(typeof(T).ToString());
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        private readonly Logger logger;
    }
}
