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
using System.Web.Script.Serialization;
using System.Data.Entity.Core.Objects; 
namespace EntrantsManagementSystem.Logging
{
   
    public class DatabaseLogger: ILogger
    {
        protected LogsDatabaseEntities ldb = new LogsDatabaseEntities();
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
                throw;
            }
        }
        public void LogUpdate(DateTime time, object newObject, object oldObject)
        {
            if (newObject == null | oldObject == null)
                throw new InvalidOperationException("One of the object's state cannot be null for update logging");

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
                    baseLog.Operation = "UPDATE";
                    baseLog.LogLevel = "Information";
                    baseLog.Time = time;

                    ldb.LogsBases.Add(baseLog);
                    ldb.SaveChanges();

                    ChangesLog changesLog = new ChangesLog();
                    changesLog.BaseLogID = baseLog.LogID;
                    changesLog.ObjectID = 0;
                    changesLog.ObjectType = newObject.GetType().Name;

                    var json = new JavaScriptSerializer().Serialize(newObject);
                    //var deserializedObj = new JavaScriptSerializer().Deserialize(json, typeof(Entrant));
                    changesLog.State = json;
                    ldb.ChangesLogs.Add(changesLog);
                    ldb.SaveChanges();
                    break;
                }
            }

        }
        public void LogDelete(DateTime time, object obj)
        {
            Type initialType = ObjectContext.GetObjectType(obj.GetType());
            Type secondaryType = obj.GetType();
            object initialObject = Activator.CreateInstance(initialType);
            List<FieldInfo> properiesInfo = initialType.GetRuntimeFields().ToList();
            List<FieldInfo> secPorpertiesInfo = secondaryType.GetRuntimeFields().ToList();
            int size1 = properiesInfo.Count();
            int size2 = secPorpertiesInfo.Count();
            foreach (FieldInfo fieldInfo in properiesInfo)
            {
                string name = fieldInfo.Name; 
                
                
                //var newValue = propertyInfo.GetValue(newObject);
                //var oldValue = propertyInfo.GetValue(oldObject);
                //string newType = newObject.GetType().Name;
                //string oldType = oldObject.GetType().Name;
                //if (!newValue.Equals(oldValue))
                //{
                //    LogsBase baseLog = new LogsBase();
                //    baseLog.Operation = "UPDATE";
                //    baseLog.LogLevel = "Information";
                //    baseLog.Time = time;

                //    ldb.LogsBases.Add(baseLog);
                //    ldb.SaveChanges();

                //    ChangesLog changesLog = new ChangesLog();
                //    changesLog.BaseLogID = baseLog.LogID;
                //    changesLog.ObjectID = 0;
                //    changesLog.ObjectType = newObject.GetType().Name;

                //    var json = new JavaScriptSerializer().Serialize(newObject);
                //    //var deserializedObj = new JavaScriptSerializer().Deserialize(json, typeof(Entrant));
                //    changesLog.State = json;
                //    ldb.ChangesLogs.Add(changesLog);
                //    ldb.SaveChanges();
                //    break;
                //}
            }

        }

        //string objectType = obj.GetType().Name;
        //LogsBase baseLog = new LogsBase();
        //    baseLog.Operation = "Delete";
        //    baseLog.LogLevel = "Information";
        //    baseLog.Time = time;

        //    ldb.LogsBases.Add(baseLog);
        //    ldb.SaveChanges();

        //    ChangesLog changesLog = new ChangesLog();
        //    changesLog.BaseLogID = baseLog.LogID;
        //    changesLog.ObjectID = 0;
        //    changesLog.ObjectType = ObjectContext.GetObjectType(obj.GetType()).Name;

        //    var json = new JavaScriptSerializer().Serialize(obj as Entrant);
        //    //var deserializedObj = new JavaScriptSerializer().Deserialize(json, typeof(Entrant));
        //    changesLog.State = json;
        //    ldb.ChangesLogs.Add(changesLog);
        //    ldb.SaveChanges();
        

    }
}