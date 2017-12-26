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

       
        [HttpPost]
        public ActionResult Create([Bind(Include = "EntrantID,Name,Surname,Patronumic,Email,CityID")] Entrant entrant)
        {
            if (ModelState.IsValid)
            {
                if (db.Entrants.Where(e => e.Email == entrant.Email).Count() > 0)
                {
                    ModelState.AddModelError("Email", "This Email already exists, choose another one");
                    ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", entrant.CityID);
                    return View(entrant);
                }
                db.Entrants.Add(entrant);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", entrant.CityID);
            return View(entrant);
        }

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

        [HttpPost]
        public ActionResult Edit(Entrant entrant)
        {
            if (ModelState.IsValid)
            {

                if(db.Entrants.Where(e => e.Email == entrant.Email & e.EntrantID != entrant.EntrantID).Count() > 0)
                {
                    ModelState.AddModelError("Email", "This Email already exists, choose another one"); 
                    ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", entrant.CityID);
                    return View(entrant);
                }
                try
                {
                    DatabaseLogger.Log(LogType.UPDATE, DateTime.Now, entrant, db.Entrants.Find(entrant.EntrantID));
                    db.Entry(db.Entrants.Find(entrant.EntrantID)).State = EntityState.Detached;
                    db.Entry(entrant).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
                catch (Exception e)
                {
                    DatabaseLogger.LogException(e, DateTime.Now, "Exception in [httpPost] Edit() action method, during update operation and saving database. ");
                    return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed);
                } 
                
               
      
            }
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", entrant.CityID);
            return View(entrant);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
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
            try
            {
                DatabaseLogger.Log(LogType.DELETE, DateTime.Now, deletedObjectType: typeof(Entrant));
                db.Entrants.Remove(entrant);
                db.SaveChanges();
                //throw new InvalidCastException();
                return RedirectToAction("List");
            }catch(Exception e)
            {
                DatabaseLogger.LogException(e, DateTime.Now, "Exception in [httpGet] Delete() action method, duringr remove operation and saving database.");
                return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed);
            }
           
        }
  
       
    }
}
