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
    [LogException]
    public class EntrantsController : Controller
    {
        private EntrantsDatabaseEntities db = new EntrantsDatabaseEntities();

        [Inject]
        public ILogger logger { get; set; }
       

        public ActionResult List()
        {
            var entrants = db.Entrants;
            return View(entrants.ToList());
        }

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
        public ActionResult DeletionConfirmation(int? id)
        {
            return View(db.Entrants.Find(id));
        }
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Title");
            ViewBag.Countries = new SelectList(db.Countries, "CountryID", "Title");
            return View(new CreateEntrantModel());
        }

       
        [HttpPost]
        public ActionResult Create(CreateEntrantModel createEntrant)
        {
            
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors); 

            if (ModelState.IsValid)
            {
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

                for (int i = 0; i < createEntrant.CertificateMarks.Count; i++)
                {
                    CertificateMark mark = createEntrant.CertificateMarks[i];
                    mark.Subject = null;
                    mark.EntrantID = entrant.EntrantID;
                    db.CertificateMarks.Add(mark);
                }
                db.SaveChanges();


                return RedirectToAction("List");
            }
            else
            {
                foreach (CertificateMark certificateMark in createEntrant.CertificateMarks)
                    certificateMark.Subject = db.Subjects.Find(certificateMark.SubjectID);
                return View(createEntrant);
            }
           
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
            ViewBag.CurrentCountryTitle = entrant.City.Country.Title;
            ViewBag.CurrentCityTitle = entrant.City.Title; 
            return View(entrant);
        }

        [HttpPost]
        public ActionResult Edit(Entrant entrant)
        {
            if (ModelState.IsValid)
            {
                logger.LogUpdate(DateTime.Now, entrant, db.Entrants.Find(entrant.EntrantID));
                db.Entry(db.Entrants.Find(entrant.EntrantID)).State = EntityState.Detached;
                foreach (CertificateMark mark in entrant.CertificateMarks)
                    db.Entry(db.CertificateMarks.Find(mark.CertificateMarkID)).State = EntityState.Detached;
                foreach (CertificateMark mark in entrant.CertificateMarks)
                    db.Entry(mark).State = EntityState.Modified;
                db.Entry(db.Entrants.Find(entrant.EntrantID)).State = EntityState.Detached;
                db.Entry(entrant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = entrant.EntrantID });
            }
            return View(db.Entrants.Find(entrant.EntrantID)); 
            
        }

        [HttpGet]
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
        public JsonResult GetCountries()
        {
            return Json(db.Countries.Select(c => new { CountryID = c.CountryID, Title = c.Title }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCities(string ID)
        {
            int.TryParse(ID, out int CountryID);
            return Json(db.Countries.Find(CountryID).Cities.Select(c => new { CityID = c.CityID, Title = c.Title }), JsonRequestBehavior.AllowGet); 
        }
       
       
    }
}
