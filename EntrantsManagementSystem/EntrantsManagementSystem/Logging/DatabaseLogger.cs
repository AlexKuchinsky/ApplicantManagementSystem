using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using System.Data.SqlClient;
using EntrantsManagementSystem.Models;
using System.Reflection;
using System.Data.Entity;
namespace EntrantsManagementSystem.Logging
{
    public class DatabaseLogger: LogBase
    {
        EntrantsDatabaseEntities db = new EntrantsDatabaseEntities(); 

        public override void Log(LogType logType, DateTime time, object newObject=null, object oldObject=null, Type deletedObjectType=null,
            string exceptionType = null, string exceptionDescription=null) 
        {
            
            
            if (logType == LogType.UPDATE)
            {
                if (newObject == null | oldObject == null)
                    throw new InvalidOperationException("Can't update null object");

                List<PropertyInfo> properiesInfo = newObject.GetType().GetProperties().Where(p => !p.GetMethod.IsVirtual & !p.SetMethod.IsVirtual).ToList();
                foreach (PropertyInfo propertyInfo in properiesInfo)
                {
                    var newValue = propertyInfo.GetValue(newObject);
                    var oldValue = propertyInfo.GetValue(oldObject);
                    if (!newValue.Equals(oldValue))
                    {
                        Log primaryLog = new Log();
                        primaryLog.Operation = logType.ToString();
                        primaryLog.StartTime = time;
                        db.Logs.Add(primaryLog);
                        db.SaveChanges();

                        UpdateLog updateLog = new UpdateLog();
                        updateLog.LogID = primaryLog.LogID;
                        updateLog.Object = newObject.GetType().Name;
                        updateLog.PropertyName = propertyInfo.Name;
                        db.UpdateLogs.Add(updateLog);
                       
                        db.SaveChanges();
                    }

                }
            }
            else if(logType == LogType.DELETE)
            {
                if(deletedObjectType == null)
                    throw new InvalidOperationException("Can't delete null object");
                Log primaryLog = new Log();
                primaryLog.Operation = logType.ToString();
                primaryLog.StartTime = time;
                db.Logs.Add(primaryLog);
                db.SaveChanges();

                DeleteLog deleteLog = new DeleteLog();
                deleteLog.LogID = primaryLog.LogID; 
                deleteLog.ObjectName = deletedObjectType.Name;
                db.DeleteLogs.Add(deleteLog);
                db.SaveChanges();
            }
            else if(logType == LogType.EXCEPTION)
            {
                if (exceptionType== null)
                    throw new InvalidOperationException("Exception has to have a type");

                Log primaryLog = new Log();
                primaryLog.Operation = logType.ToString();
                primaryLog.StartTime = time;
                db.Logs.Add(primaryLog);
                db.SaveChanges();

                ExceptionLog exceptionLog = new ExceptionLog();
                exceptionLog.LogID = primaryLog.LogID;
                exceptionLog.Description = exceptionDescription;
                exceptionLog.Type = exceptionType;
                db.ExceptionLogs.Add(exceptionLog);
                db.SaveChanges();

            }
  

        }
        
    }
}