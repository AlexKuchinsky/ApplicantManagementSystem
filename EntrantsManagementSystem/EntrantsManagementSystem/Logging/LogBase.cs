using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EntrantsManagementSystem.Logging
{
    public enum LogType { UPDATE, DELETE, EXCEPTION }
    public abstract class LogBase
    {
        public abstract void Log(LogType logType, DateTime time, object newObject = null, object oldObject = null, Type deletedObjectType=null,
            string exceptionType = null, string exceptionDescription = null);
    }
}