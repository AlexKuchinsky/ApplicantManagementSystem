using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntrantsManagementSystem.Models; 
namespace EntrantsManagementSystem.Logging
{
    public interface ILogger
    {

        void LogException(Exception exception, DateTime time, string exceptionDescription);
        void LogUpdate(DateTime time, object newObject, object oldObject);
        void LogDelete(DateTime time, object obj);
    }
}
