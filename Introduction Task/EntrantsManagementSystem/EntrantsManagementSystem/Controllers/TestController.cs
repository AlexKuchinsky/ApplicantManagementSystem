using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntrantsManagementSystem.Models;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Collections;
using EntrantsManagementSystem.ObjectParser;
namespace EntrantsManagementSystem.Controllers
{
    public class TestController : Controller
    {
        public UniversitiesDatabaseEntities db { get; set; } = new UniversitiesDatabaseEntities();

        public class Root
        {
            public UniversitiesDatabaseEntities udb = new UniversitiesDatabaseEntities();
            public EntrantsDatabaseEntities edb = new EntrantsDatabaseEntities();

            public IEnumerable Universities => udb.Universities.ToList();
        }
        public Root root = new Root();


        public ActionResult Dropdowns()
        {
            return View();
        }
        public ActionResult Tree()
        {
            //List<List<int>> routes = new List<List<int>>();
            //routes.Add(new List<int>(new int[] {0,0,0,0 }));
            //routes.Add(new List<int>(new int[] {0,0,0,1 }));
            //routes.Add(new List<int>(new int[] {0,0,1,0 }));
            //routes.Add(new List<int>(new int[] {0,0,1,1 }));
            //routes.Add(new List<int>(new int[] {0,1,0,0 }));
            //routes.Add(new List<int>(new int[] {0,1,0,1 }));
            //routes.Add(new List<int>(new int[] {0,1,0,2 }));

            //ObjectWalker.GetSelectedItems<Speciality, object>((s) => { return null; }, routes);
            return View();
        }

        #region Dropdowns
        public JsonResult GetUniversities()
        {
            return Json(db.Universities.Select(u=>new { UniversityID = u.UniversityID, FullName = u.FullName, Abbreviation  = u.Abbreviation }),JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFaculties(string ID)
        {
            int.TryParse(ID, out int UniversityID);
            return Json(db.Universities.Find(UniversityID).Faculties.Select(f =>new { FacultyID = f.FacultyID, Title = f.Title }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSpecialities(string ID)
        {
            int.TryParse(ID, out int SpecialityID);
            return Json(db.Faculties.Find(SpecialityID).Specialities.Select(s=>new { SpecialityID = s.SpecialityID, Title = s.Title, FacultyTitle = s.Faculty.Title, UniversityTitle = s.Faculty.University.FullName }), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Tree
        public JsonResult GetChildren(string route)
        {
            List<int> r = (List<int>)new JavaScriptSerializer().Deserialize(route, typeof(List<int>));
            JsonResult result = Json(ObjectWalker.GetChildrenRecursion(root, r, r.ToList(),typeof(Entrant)), JsonRequestBehavior.AllowGet);
            return result; 
        }
        public JsonResult GetSelectedItems(string data)
        {
            List<string> string_routes = (List<string>)new JavaScriptSerializer().Deserialize(data, typeof(List<string>));
            List<List<int>> routes = new List<List<int>>();
            for (int i = 0; i < string_routes.Count; i++)
                routes.Add((List<int>)new JavaScriptSerializer().Deserialize(string_routes[i], typeof(List<int>)));

            List<object> result = ObjectWalker.GetSelectedItems<Speciality,object>(root, routes, typeof(Speciality),(o)=> new { data = o.Title + " (id "+o.SpecialityID+")" });
            return Json(result,JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}