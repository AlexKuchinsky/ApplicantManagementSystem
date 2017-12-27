using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntrantsManagementSystem.Logging
{
    public enum LogType { UPDATE, DELETE }
    public interface ILogger
    {
        void LogException(Exception exception, DateTime time, string exceptionDescription);
        void Log(LogType logType, DateTime time, object newObject = null, object oldObject = null, Type deletedObjectType = null);
    }
}
