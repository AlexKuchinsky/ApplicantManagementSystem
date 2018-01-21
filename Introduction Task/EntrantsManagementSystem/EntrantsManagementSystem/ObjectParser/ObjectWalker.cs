using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntrantsManagementSystem.Models;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Collections;
using System.Data.Entity.Core.Objects;
namespace EntrantsManagementSystem.ObjectParser
{
    public class ObjectWalker
    {
        public static List<object> GetChildrenRecursion(object obj, List<int> route, List<int> initialRoute,Type bottomType)
        {
            if (route.Count == 0)
                return GetArrayedChildren(obj, initialRoute,bottomType);
            else
            {
                int index = route[0];
                object newObj = GetOnPosition(obj, index);
                return GetChildrenRecursion(newObj, route.Skip(1).ToList(), initialRoute,bottomType);
            }
        }

        // Walk through the object
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
        protected static List<object> GetArrayedChildren(object obj, IList<int> path,Type bottomType)
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
                    Type initialType = ObjectContext.GetObjectType(currentObject.GetType());
                    bool isAssignable = bottomType.IsAssignableFrom(enumerator.Current.GetType()); 
                    int NumberOfChildren = (isAssignable ? 0 : GetNumberOfChildren(enumerator.Current));
                    resultList.Add(new { Name = initialType.GetMethod("ToString").Invoke(currentObject,new object[] { }),NumberOfChildren, Route = route });
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

        // Get leafs' elements
    
        public static List<object> GetSelectedItems<BottomType, OutputType>(object obj,string data,Type bottomType, Func<BottomType, OutputType> miningFunction)
        {
            List<string> string_routes = (List<string>)new JavaScriptSerializer().Deserialize(data, typeof(List<string>));
            List<List<int>> routes = new List<List<int>>();
            for (int i = 0; i < string_routes.Count; i++)
                routes.Add((List<int>)new JavaScriptSerializer().Deserialize(string_routes[i], typeof(List<int>)));
            if (routes.Count == 0)
            {
                return new List<object>();
            }

            List<object> result = new List<object>();
            if (routes.Count!=0 & routes[0].Count != 0)
            {
                foreach (IGrouping<int, List<int>> rs in routes.GroupBy(r => r[0]))
                {
                    int first_element = rs.Key;
                    RouteNode node = new RouteNode();
                    node.Value = first_element;
                    foreach (List<int> r in rs.Select(r => r.Skip(1).ToList()))
                        ConstructRoute(r, node);
                    GetSelectedItemsRecursion(obj, node, bottomType, result, miningFunction); 
                }
            }
            else 
                GetAllChidren(obj, bottomType, result, miningFunction);

            return result;               
        }
        protected static void GetSelectedItemsRecursion<BottomType, OutputType>(object obj,RouteNode node, Type bottomType, List<object> result, Func<BottomType, OutputType> miningFunction)
        {
                int index = node.Value;
                object newObj = GetOnPosition(obj, index);
                if (node.Children.Count == 0)
                    GetAllChidren(newObj, bottomType, result, miningFunction);
                foreach (RouteNode child in node.Children)
                GetSelectedItemsRecursion(newObj, child, bottomType, result, miningFunction);
        }
        //protected static void GetSelectedItemsRecursion<BottomType, OutputType>(object obj,List<int> route,Type bottomType,List<object> result, Func<BottomType, OutputType> miningFunction)
        //{
        //    if (route.Count == 0)
        //    {
        //        GetAllChidren(obj, bottomType, result,miningFunction); 
        //    }
        //    else
        //    {
        //        int index = route[0];
        //        object newObj = GetOnPosition(obj, index);
        //        GetSelectedItemsRecursion(newObj, route.Skip(1).ToList(), bottomType, result,miningFunction); 
        //    }
        //}
        protected static void GetAllChidren<BottomType, OutputType>(object obj,Type bottomType,List<object> result, Func<BottomType, OutputType> miningFunction)
        {
            
            if (ObjectContext.GetObjectType(obj.GetType()) == bottomType)
                result.Add(miningFunction.Invoke((BottomType)obj));
            else
            {
                if (!IsIEnumerable(obj))
                {
                    // Work with class
                    List<PropertyInfo> CollectionProps = obj.GetType().GetRuntimeProperties().Where(p => IsIEnumerable(p.GetValue(obj))).ToList();
                    foreach(PropertyInfo prop in CollectionProps)
                        GetAllChidren(prop.GetValue(obj), bottomType, result,miningFunction); 
                }
                else
                {
  
                    IEnumerator enumerator = (obj as IEnumerable).GetEnumerator();
                
                    while (enumerator.MoveNext())
                    {
                        object currentObject = enumerator.Current;
                        GetAllChidren(currentObject, bottomType, result,miningFunction); 
                    }
                }
            }
        }

        // Constructing wise route
        public class RouteNode
        {
            public int Value=-1;
            public List<RouteNode> Children = new List<RouteNode>();
        }
        public static void ConstructRoute(List<int> route, RouteNode node)
        {
            if (route.Count == 0)
            {
                return; 
            }
            else
            {
                int FirstValue = route[0]; 
                bool Found = false;
                int i = 0; 
                for(; i<node.Children.Count; i++)
                {
                    if (node.Children[i].Value == FirstValue)
                    {
                        Found = true;
                        break;
                    }
                        
                }
                if (Found)
                {
                    ConstructRoute(route.Skip(1).ToList(), node.Children[i]);
                }
                else
                {
                    RouteNode newNode = new RouteNode();
                    newNode.Value = FirstValue;
                    node.Children.Add(newNode);
                    ConstructRoute(route.Skip(1).ToList(), newNode); 
                }
            }

        }

        
    }
}

