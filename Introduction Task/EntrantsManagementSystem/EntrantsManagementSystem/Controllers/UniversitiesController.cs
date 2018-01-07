using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntrantsManagementSystem.Models; 
namespace EntrantsManagementSystem.Controllers
{
    public class UniversitiesController : Controller
    {
        public UniversitiesDatabaseEntities db = new UniversitiesDatabaseEntities();
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


    }
}