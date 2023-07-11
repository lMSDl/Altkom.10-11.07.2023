namespace ConsoleApp
{
    public class Logger
    {
        private Dictionary<DateTime, string> _logs = new Dictionary<DateTime, string>();
        public event EventHandler<EventArgs>? MessageLogged;

        public void Log(string message)
        {
            var dateTime = DateTime.Now;
            _logs[dateTime] = message;
            MessageLogged?.Invoke(this, new LoggerEventArgs(dateTime, message));
        }


        public class LoggerEventArgs : EventArgs
        {
            public DateTime DateTime { get; }
            public string Message { get; }

            public LoggerEventArgs(DateTime dateTime, string message)
            {
                DateTime = dateTime;
                Message = message;
            }
        }

        public string GetLogs(DateTime from, DateTime to)
        {
            return string.Join("\n", _logs
                .Where(x => x.Key >= from)
                .Where(x => x.Key <= to)
                .Select(x => x.Value));
        }

    }
}
