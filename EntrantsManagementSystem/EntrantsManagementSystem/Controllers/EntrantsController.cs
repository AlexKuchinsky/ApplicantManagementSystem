using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EntrantsManagementSystem.Models;
using EntrantsManagementSystem.Logging;
namespace EntrantsManagementSystem.Controllers
{
    public class EntrantsController : Controller
    {
        private EntrantsDatabaseEntities db = new EntrantsDatabaseEntities();

        // GET: Entrants
        public ActionResult List()
        {
            var entrants = db.Entrants.Include(e => e.City);
            return View(entrants.ToList());
        }

        // GET: Entrants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entrant entrant = db.Entrants.Find(id);
            if (entrant == null)
            {
                return HttpNotFound();
            }
            return View(entrant);
        }

        // GET: Entrants/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name");
            return View();
        }

        // POST: Entrants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EntrantID,Name,Surname,Patronumic,Email,CityID")] Entrant entrant)
        {
            if (ModelState.IsValid)
            {
                db.Entrants.Add(entrant);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", entrant.CityID);
            return View(entrant);
        }

        // GET: Entrants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entrant entrant = db.Entrants.Find(id);
            if (entrant == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", entrant.CityID);
            return View(entrant);
        }

        // POST: Entrants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EntrantID,Name,Surname,Patronumic,Email,CityID")] Entrant entrant)
        {
            if (ModelState.IsValid)
            {

                new DatabaseLogger().Log(LogType.UPDATE, DateTime.Now, entrant, db.Entrants.Find(entrant.EntrantID));
                db.Entry(db.Entrants.Find(entrant.EntrantID)).State = EntityState.Detached;
                db.Entry(entrant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
               
      
            }
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", entrant.CityID);
            return View(entrant);
        }

        // GET: Entrants/Delete/5
        public ActionResult Delete(int? id)
        {

            Entrant entrant = db.Entrants.Find(id);
            new DatabaseLogger().Log(LogType.DELETE, DateTime.Now, deletedObjectType: typeof(Entrant));
            db.Entrants.Remove(entrant);
            db.SaveChanges();
            return RedirectToAction("List");
        }
        public ActionResult EditMark(int? id)
        {
            return View(db.Marks.Find(id));
        }
        [HttpPost]
        public ActionResult EditMark([Bind(Include ="EntrantID,SubjectID,Score,MarkID")] Mark mark)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mark).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details",new { id = mark.EntrantID });
            }
            return View(mark);
        }
    }
}
