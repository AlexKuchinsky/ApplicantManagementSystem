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
using EntrantsManagementSystem.Infrastructure;
using Ninject;
using System.Data.Entity.Core.Objects; 
namespace EntrantsManagementSystem.Controllers
{
    public class EntrantsController : Controller
    {
        private EntrantsDatabaseEntities db = new EntrantsDatabaseEntities();
        [Inject]
        public ILogger logger { get; set; }
       
        // GET: Entrants
        [LogException]
        public ActionResult List()
        {
            var entrants = db.Entrants;
            return View(entrants.ToList());
        }

        // GET: Entrants/Details/5
        [LogException]
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
        [LogException]
        public ActionResult DeletionConfirmation(int? id)
        {
            return View(db.Entrants.Find(id));
        }
        [LogException]
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Title");
            ViewBag.Countries = new SelectList(db.Countries, "CountryID", "Title");
            return View(new CreateEntrantModel());
        }

       
        [HttpPost]
        [LogException]
        public ActionResult Create(CreateEntrantModel createEntrant)
        {

            //if (db.Entrants.Where(e => e.Email == entrant.Email).Count() > 0)
            //{
            //    ModelState.AddModelError("Email", "This Email already exists, choose another one");
            //    ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", entrant.CityID);
            //    return View(entrant);
            //}

            Entrant entrant = new Entrant();

            entrant.Name = createEntrant.Name;
            entrant.Surname = createEntrant.Surname;
            entrant.Patronumic = createEntrant.Patronumic;
            
            
            entrant.CityID = createEntrant.CityID;
            entrant.Street = createEntrant.Street;
            entrant.House = createEntrant.House; 
            entrant.Entrance = createEntrant.Entrance;
            entrant.Flat = createEntrant.Flat;

            

            db.Entrants.Add(entrant);
            db.SaveChanges();

            for(int i=0; i<createEntrant.CertificateMarks.Count; i++)
            {
                CertificateMark mark = createEntrant.CertificateMarks[i];
                mark.Subject = null;
                mark.EntrantID = entrant.EntrantID;
                db.CertificateMarks.Add(mark); 
            }
            db.SaveChanges();

            
            return RedirectToAction("List");

            //ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", entrant.CityID);
            //return View(entrant);
        }

        [LogException]
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
            ViewBag.CurrentCountryTitle = entrant.City.Country.Title;
            ViewBag.CurrentCityTitle = entrant.City.Title; 
            return View(entrant);
        }

        [HttpPost]
        [LogException]
        public ActionResult Edit(Entrant entrant)
        {
            logger.LogUpdate(DateTime.Now, entrant, db.Entrants.Find(entrant.EntrantID));
            db.Entry(db.Entrants.Find(entrant.EntrantID)).State = EntityState.Detached;
            foreach (CertificateMark mark in entrant.CertificateMarks)
            {
                db.Entry(db.CertificateMarks.Find(mark.CertificateMarkID)).State = EntityState.Detached;
            }
                foreach (CertificateMark mark in entrant.CertificateMarks)
            {
                db.Entry(mark).State = EntityState.Modified;
            }
            //db.SaveChanges();
            db.Entry(db.Entrants.Find(entrant.EntrantID)).State = EntityState.Detached;
            db.Entry(entrant).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = entrant.EntrantID });
        }

        [HttpGet]
        [LogException]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entrant = db.Entrants.Find(id);
            if (entrant == null)
            {
                return HttpNotFound();
            }
            logger.LogDelete(DateTime.Now,entrant);
            
            
            db.Entrants.Remove(entrant);
            db.SaveChanges();
            return RedirectToAction("List");



        }
        [HttpPost]
        public JsonResult GetCountries()
        {
            return Json(db.Countries.Select(c => new { CountryID = c.CountryID, Title = c.Title }));
        }
        [HttpPost]
        public JsonResult GetCities(string ID)
        {
            int.TryParse(ID, out int CountryID);
            return Json(db.Countries.Find(CountryID).Cities.Select(c => new { CityID = c.CityID, Title = c.Title })); 
        }
       
       
    }
}
