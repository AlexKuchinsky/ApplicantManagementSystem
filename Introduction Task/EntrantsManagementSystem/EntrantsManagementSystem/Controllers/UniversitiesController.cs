using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntrantsManagementSystem.Models;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Collections;

namespace EntrantsManagementSystem.Controllers
{
    public class UniversitiesController : Controller
    {
        public class Root
        {
            public UniversitiesDatabaseEntities udb = new UniversitiesDatabaseEntities();
            public EntrantsDatabaseEntities edb = new EntrantsDatabaseEntities();

            public ICollection Universities => udb.Universities.ToList();
            public ICollection Entrants => edb.Entrants.ToList();
            public ICollection Countries => edb.Countries.ToList();
        }
        public UniversitiesDatabaseEntities db { get; set; } = new UniversitiesDatabaseEntities();

        public Root root = new Root();

        public ActionResult Index()
        {
           
            return View();
        }
        [HttpPost]
        public JsonResult GetUniversities()
        {
            return Json(db.Universities.Select(u=>new { UniversityID = u.UniversityID, FullName = u.FullName, Abbreviation  = u.Abbreviation }));
        }
        [HttpPost]
        public JsonResult GetFaculties(string ID)
        {
            int.TryParse(ID, out int UniversityID);
            return Json(db.Universities.Find(UniversityID).Faculties.Select(f =>new { FacultyID = f.FacultyID, Title = f.Title }));
        }
        [HttpPost]
        public JsonResult GetSpecialities(string ID)
        {
            int.TryParse(ID, out int SpecialityID);
            return Json(db.Faculties.Find(SpecialityID).Specialities.Select(s=>new { SpecialityID = s.SpecialityID, Title = s.Title, FacultyTitle = s.Faculty.Title, UniversityTitle = s.Faculty.University.FullName }));
        }
        public ActionResult Tree()
        {
            return View(); 
        }
        [HttpPost]
        public JsonResult GetChilds(string route)
        {
            List<int> r = (List<int>)new JavaScriptSerializer().Deserialize(route, typeof(List<int>));
            return GetChildsRecursion(root, r, r.ToList()); 
        }
        public JsonResult GetChildsRecursion(object obj, List<int> route,List<int> initialRoute)
        {
            if (route.Count == 0)
                return GetArrayedChildren(obj, initialRoute);
            else
            {
                int index = route[0];
                object newObj = GetOnPosition(obj, index);
                return GetChildsRecursion(newObj, route.Skip(1).ToList(), initialRoute);
            }
        }
        public int GetNumberOfChildren(object obj)
        {
            if (obj == null)
                return 0;
            if(!IsIEnumerable(obj.GetType()))
            {
                // Count IEnumerable props in class
                List<PropertyInfo> CollectionProps = obj.GetType().GetRuntimeProperties().Where(p => IsIEnumerable(p.PropertyType)).ToList();
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
        public JsonResult GetArrayedChildren(object obj, IList<int> path)
        {
            if (!IsIEnumerable(obj.GetType()))
            {
                // Work with class
                List<PropertyInfo> CollectionProps = obj.GetType().GetRuntimeProperties().Where(p => IsIEnumerable(p.PropertyType)).ToList();
                IEnumerable resultList = CollectionProps.Select((p, i) =>
                {
                    List<int> route = path.ToList();
                    route.Add(i);
                    return new { Name = p.Name, NumberOfChildren = GetNumberOfChildren(p.GetValue(obj)), Route = route };
                }).ToList();
                return Json(resultList);
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
                    foreach(PropertyInfo p in properties)
                    {
                        if(p.Name.ToLower().Contains("name") || p.Name.ToLower().Contains("title"))
                        {
                            elementName = (string)p.GetValue(currentObject) ?? "Element ";
                            break; 
                        }
                    }
                    currentObject.GetType();
                    resultList.Add(new { Name = elementName + ((elementName=="Element ")? (i+1).ToString():""), NumberOfChildren = GetNumberOfChildren(enumerator.Current), Route = route });
                    i++;
                }
                return Json(resultList); 
            }
        }
        public bool IsIEnumerable(Type type)
        {
            string name = type.Name;
            bool result = type.GetInterface(nameof(IEnumerable)) != null && type.Name != nameof(String);
            return result; 
        }
        public object GetOnPosition(object obj, int index)
        {
            if (IsIEnumerable(obj.GetType()))
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
                List<PropertyInfo> CollectionProps = obj.GetType().GetRuntimeProperties().Where(p => IsIEnumerable(p.PropertyType)).ToList();
                return CollectionProps[index].GetValue(obj); 
            }
        }
    }
}