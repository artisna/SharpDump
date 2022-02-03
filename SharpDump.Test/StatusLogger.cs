using SharpDump.Logic;
using System.Collections.Generic;

namespace SharpDump.Test
{
    internal class StatusLogger : IStatusLogger
    {
        private readonly List<string> _logs;

        public IReadOnlyList<string> Logs => this._logs.AsReadOnly();

        public StatusLogger()
        {
            _logs = new List<string>();
        }

        public void Log(string message, params object[] args)
        {
            this._logs.Add(string.Format(message, args));
        }
    }
}
