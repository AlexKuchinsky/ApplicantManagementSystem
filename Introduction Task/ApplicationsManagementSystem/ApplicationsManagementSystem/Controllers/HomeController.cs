using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationsManagementSystem.Models;
namespace ApplicationsManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        UniversitySpecialtiesDatabaseEntities db = new UniversitySpecialtiesDatabaseEntities();
        public ActionResult Index()
        {
            return View("AddSpeciality", db.UniversitySpecialities.Find(2));
        }
    }
}