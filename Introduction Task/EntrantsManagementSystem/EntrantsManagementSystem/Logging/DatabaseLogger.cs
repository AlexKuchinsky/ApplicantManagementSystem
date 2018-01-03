using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using System.Data.SqlClient;
using EntrantsManagementSystem.Models;
using System.Reflection;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization; 
namespace EntrantsManagementSystem.Logging
{
   
    public class DatabaseLogger: ILogger
    {
        public LogsDatabaseEntities ldb = new LogsDatabaseEntities();
        public void LogException(Exception exception, DateTime time, string exceptionDescription)
        {

            if (exception == null)
                throw new InvalidOperationException("You are trying to log null exception");

            LogsBase baseLog = new LogsBase();
            baseLog.Operation = "EXCEPTION";
            baseLog.LogLevel = "Error";
            baseLog.Time = time;

            ldb.LogsBases.Add(baseLog);
            ldb.SaveChanges();

            ExceptionLog exceptionLog = new ExceptionLog();
            exceptionLog.BaseLogID = baseLog.LogID;
            exceptionLog.ExceptionType = exception.GetType().Name;
            exceptionLog.StackTrace = exception.StackTrace;
            exceptionLog.Message = exception.Message;
            exceptionLog.Description = exceptionDescription;

            ldb.ExceptionLogs.Add(exceptionLog);
            try
            {

                ldb.SaveChanges();

            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Object: " + validationError.Entry.Entity.ToString());
                    System.Diagnostics.Debug.WriteLine("");
                        foreach (DbValidationError err in validationError.ValidationErrors)
                             {
                        System.Diagnostics.Debug.WriteLine(err.ErrorMessage + "");
                        }
                }
                               
            }
            

            //Log primaryLog = new Log();
            //primaryLog.Operation = "EXCEPTION";
            //primaryLog.StartTime = time;
            //db.Logs.Add(primaryLog);
            //db.SaveChanges();

            //ExceptionLog exceptionLog = new ExceptionLog();
            //exceptionLog.LogID = primaryLog.LogID;
            //exceptionLog.Description = exceptionDescription;
            //exceptionLog.Type = exception.GetType().Name;
            //db.ExceptionLogs.Add(exceptionLog);
            //db.SaveChanges();

        }
        public void Log(LogType logType, DateTime time, object newObject=null, object oldObject=null, Type deletedObjectType=null) 
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
                    string newType = newObject.GetType().Name;
                    string oldType = oldObject.GetType().Name;
                    if (!newValue.Equals(oldValue))
                    {
                        LogsBase baseLog = new LogsBase();
                        baseLog.Operation = logType.ToString();
                        baseLog.LogLevel = "Information";
                        baseLog.Time = time;

                        ldb.LogsBases.Add(baseLog);
                        ldb.SaveChanges();

                        ChangesLog changesLog = new ChangesLog();
                        changesLog.BaseLogID = baseLog.LogID;
                        changesLog.ObjectID = 0;
                        changesLog.ObjectType = newObject.GetType().Name;

                        MemoryStream stream1 = new MemoryStream();
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(stream1,newObject);
                        
                        return;

                    }

                }
            }
            else if(logType == LogType.DELETE)
            {
                //if(deletedObjectType == null)
                //    throw new InvalidOperationException("Can't delete null object");
                //Log primaryLog = new Log();
                //primaryLog.Operation = logType.ToString();
                //primaryLog.StartTime = time;
                //db.Logs.Add(primaryLog);
                //db.SaveChanges();

                //DeleteLog deleteLog = new DeleteLog();
                //deleteLog.LogID = primaryLog.LogID; 
                //deleteLog.ObjectName = deletedObjectType.Name;
                //db.DeleteLogs.Add(deleteLog);
                //db.SaveChanges();
            }
           

        }
        public void LogUpdate()
        {

        }
    }
}