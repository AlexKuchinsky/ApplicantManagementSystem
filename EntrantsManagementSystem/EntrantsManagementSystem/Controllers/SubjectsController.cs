using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using EntrantsManagementSystem.Models; 
namespace EntrantsManagementSystem.Controllers
{
    public class SubjectsController : Controller
    {
        private EntrantsDatabaseEntities db = new EntrantsDatabaseEntities(); 
        // GET: Subjects
        public ActionResult List()
        {
            return View(db.Subjects.ToList());
        }
        public ActionResult SubjectSettings(int? id)
        {
            return View(db.Subjects.Find(id)); 
        }
        [HttpPost]
        public ActionResult SubjectSettings([Bind(Include ="SubjectID,Name")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(subject.SubjectID);
        }
        public ActionResult CreateSubject()
        {
            return View(); 
        }
        [HttpPost]
        public ActionResult CreateSubject([Bind(Include = "SubjectID,Name")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Subjects.Add(subject);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View();
        }

    }
}