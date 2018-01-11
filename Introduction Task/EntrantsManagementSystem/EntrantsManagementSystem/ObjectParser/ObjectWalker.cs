using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntrantsManagementSystem.Models;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Collections;

namespace EntrantsManagementSystem.ObjectParser
{
    public class ObjectWalker
    {
        public static List<object> GetChildrenRecursion(object obj, List<int> route, List<int> initialRoute)
        {
            if (route.Count == 0)
                return GetArrayedChildren(obj, initialRoute);
            else
            {
                int index = route[0];
                object newObj = GetOnPosition(obj, index);
                return GetChildrenRecursion(newObj, route.Skip(1).ToList(), initialRoute);
            }
        }

        protected static int GetNumberOfChildren(object obj)
        {
            if (obj == null)
                return 0;
            if (!IsIEnumerable(obj))
            {
                // Count IEnumerable props in class
                List<PropertyInfo> CollectionProps = obj.GetType().GetRuntimeProperties().Where(p => IsIEnumerable(p.GetValue(obj))).ToList();
                return CollectionProps.Count;
            }
            else
            {
                IEnumerator enumerator = (obj as IEnumerable).GetEnumerator();
                int size = 0;
                while (enumerator.MoveNext())
                    size++;
                return size;
            }

        }
        protected static List<object> GetArrayedChildren(object obj, IList<int> path)
        {
            if (!IsIEnumerable(obj))
            {
                // Work with class
                List<PropertyInfo> CollectionProps = obj.GetType().GetRuntimeProperties().Where(p => IsIEnumerable(p.GetValue(obj))).ToList();
                IEnumerable<object> resultList = CollectionProps.Select((p, i) =>
                {
                    List<int> route = path.ToList();
                    route.Add(i);
                    return new { Name = p.Name, NumberOfChildren = GetNumberOfChildren(p.GetValue(obj)), Route = route };
                }).ToList();
                return resultList.ToList();
            }
            else
            {
                List<object> resultList = new List<object>();
                IEnumerator enumerator = (obj as IEnumerable).GetEnumerator();
                int i = 0;
                while (enumerator.MoveNext())
                {
                    List<int> route = path.ToList();
                    route.Add(i);
                    object currentObject = enumerator.Current;
                    string elementName = "Element ";
                    List<PropertyInfo> properties = currentObject.GetType().GetRuntimeProperties().ToList();
                    foreach (PropertyInfo p in properties)
                    {
                        if (p.Name.ToLower().Contains("name") || p.Name.ToLower().Contains("title"))
                        {
                            elementName = (string)p.GetValue(currentObject) ?? "Element ";
                            break;
                        }
                    }
                    currentObject.GetType();
                    resultList.Add(new { Name = elementName + ((elementName == "Element ") ? (i + 1).ToString() : ""), NumberOfChildren = GetNumberOfChildren(enumerator.Current), Route = route });
                    i++;
                }
                return resultList;
            }
        }
        protected static bool IsIEnumerable(object obj)
        {
            bool result = (obj is IEnumerable) && obj.GetType().Name != nameof(String);
            return result;
        }
        protected static object GetOnPosition(object obj, int index)
        {
            if (IsIEnumerable(obj))
            {
                IEnumerator enumerator = (obj as IEnumerable).GetEnumerator();
                int i = 0;
                while (enumerator.MoveNext())
                {
                    if (i == index)
                        return enumerator.Current;
                    i++;
                }
                throw new ArgumentException("index wasn't found");

            }
            else
            {
                List<PropertyInfo> CollectionProps = obj.GetType().GetRuntimeProperties().Where(p => IsIEnumerable(p.GetValue(obj))).ToList();
                return CollectionProps[index].GetValue(obj);
            }
        }
    }
}