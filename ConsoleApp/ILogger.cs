using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public interface ILogger
    {
        void Log(string message);
        string GetLogs(DateTime fromm, DateTime to);
    }
}
