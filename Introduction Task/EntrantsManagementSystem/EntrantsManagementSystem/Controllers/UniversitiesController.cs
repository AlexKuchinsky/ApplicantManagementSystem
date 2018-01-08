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
        class Root
        {
            public UniversitiesDatabaseEntities db = new UniversitiesDatabaseEntities();
            public IList Universities => db.Universities.ToList();
            public IList Entrants { get; set; }
        }
        public UniversitiesDatabaseEntities db { get; set; } = new UniversitiesDatabaseEntities();

        public object root = new Root();

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
            List<PropertyInfo> CollectionProps = obj.GetType().GetProperties().Where(p => p.PropertyType.GetInterface("ICollection") != null).ToList();
            if (CollectionProps.Count == 0)
                throw new NotImplementedException();
            IEnumerable resultList = CollectionProps.Select((p, i) =>
            {
                List<int> resultRoute = route.ToList();
                resultRoute.Add(i);
                return new { Name = p.Name, NumberOfChildren = (p.GetValue(obj) as ICollection)?.Count ?? 0, Route = resultRoute };
            }).ToList();
            return Json(resultList);
        }
    }
}