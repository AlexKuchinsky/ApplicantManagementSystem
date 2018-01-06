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
            return Json(db.Universities.Select(u => u.FullName));
        }
        [HttpPost]
        public JsonResult GetFaculties(string university)
        {
            return Json(db.Faculties.Where(f => f.University.FullName == university).Select(f => f.Title)); 
        }
        [HttpPost]
        public JsonResult GetSpecialities(string faculty)
        {
            return Json(db.Specialities.Where(s => s.Faculty.Title == faculty).Select(s => s.Title));
        }


    }
}