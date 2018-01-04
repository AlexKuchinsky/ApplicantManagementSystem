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

            int? ObjectID = 0;
            bool LogChangedState = false;

            IsObjectWasChanged(newObject, oldObject, ref ObjectID, ref LogChangedState);
            IsCollectionWasChanged(newObject, oldObject, ref LogChangedState);
            
            if (LogChangedState)
            {
                LogsBase baseLog = new LogsBase();
                baseLog.Operation = "UPDATE";
                baseLog.LogLevel = "Information";
                baseLog.Time = time;

                ldb.LogsBases.Add(baseLog);
                ldb.SaveChanges();

                ChangesLog changesLog = new ChangesLog();
                changesLog.BaseLogID = baseLog.LogID;
                changesLog.ObjectID = ObjectID ?? 0;
                changesLog.ObjectType = newObject.GetType().Name;

                var json = new JavaScriptSerializer().Serialize(newObject);
                //var deserializedObj = new JavaScriptSerializer().Deserialize(json, typeof(Entrant));
                changesLog.State = json;
                ldb.ChangesLogs.Add(changesLog);
                ldb.SaveChanges();
            }

        }
        public void LogDelete(DateTime time, object obj)
        {
            int? ObjectID = 0; 
            // Copy all information in new not-wrapped object
            // So it can be serialized
            Type initialType = ObjectContext.GetObjectType(obj.GetType());
            object initialObject = Activator.CreateInstance(initialType);
            List<FieldInfo> properiesInfo = initialType.GetRuntimeFields().ToList();
            foreach (FieldInfo fieldInfo in properiesInfo)
            {
                fieldInfo.SetValue(initialObject, fieldInfo.GetValue(obj));
                if (fieldInfo.Name.Contains(initialType.Name + "ID"))
                {
                    ObjectID = (fieldInfo.GetValue(obj) as int?) ?? 0;
                }
            }

            LogsBase baseLog = new LogsBase();
            baseLog.Operation = "Delete";
            baseLog.LogLevel = "Information";
            baseLog.Time = time;

            ldb.LogsBases.Add(baseLog);
            ldb.SaveChanges();



            ChangesLog changesLog = new ChangesLog();
            changesLog.BaseLogID = baseLog.LogID;
            changesLog.ObjectID = ObjectID ?? 0;
            changesLog.ObjectType = initialType.Name;

            var json = new JavaScriptSerializer().Serialize(initialObject);
            changesLog.State = json;
            ldb.ChangesLogs.Add(changesLog);
            ldb.SaveChanges();


        }

        public void IsObjectWasChanged(object newObject, object oldObject,ref int? ObjectID,ref bool LogChangedState)
        {
            List<PropertyInfo> properiesInfo = newObject.GetType().GetProperties().Where(p => !p.GetMethod.IsVirtual & !p.SetMethod.IsVirtual).ToList();
            foreach (PropertyInfo propertyInfo in properiesInfo)
            {
                var newValue = propertyInfo.GetValue(newObject);
                var oldValue = propertyInfo.GetValue(oldObject);
                // If property value was changed during editing, we should log this object
                if (!newValue.Equals(oldValue))
                {
                    LogChangedState = true;
                }
                // Checking, whether we have an "ClassName"ID property
                if (propertyInfo.Name.Contains(newObject.GetType().Name + "ID"))
                {
                    ObjectID = (newValue as int?) ?? 0;
                }

            }
        }
        public void IsCollectionWasChanged(object newObject, object oldObject, ref bool LogChangedState)
        {
            int? plug = 0; 
            List<PropertyInfo> IEnumerableProps = newObject.GetType().GetProperties().Where(p => p.PropertyType.GetInterface("IEnumerable") != null && (p.PropertyType.Name != nameof(String))).ToList();
            foreach (PropertyInfo propInfo in IEnumerableProps)
            {
                System.Collections.IEnumerator enumeratorOfNewCollection = (propInfo.GetValue(newObject) as System.Collections.IEnumerable)?.GetEnumerator();
                System.Collections.IEnumerator enumeratorOfOldCollection = (propInfo.GetValue(oldObject) as System.Collections.IEnumerable)?.GetEnumerator();

                if (enumeratorOfNewCollection == null || enumeratorOfOldCollection == null)
                    break;
                while (enumeratorOfNewCollection.MoveNext() && enumeratorOfOldCollection.MoveNext())
                {
                    object newValue = enumeratorOfNewCollection.Current;
                    object oldValue = enumeratorOfOldCollection.Current;
                    IsObjectWasChanged(newValue, oldValue,ref plug,ref LogChangedState);
                    if (LogChangedState)
                    {
                        return;
                    }
                }
            }

        }


    }
}