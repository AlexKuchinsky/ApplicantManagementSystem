using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Web;
using System.Web.Mvc;

using System.Web.Mvc;
using EntrantsManagementSystem.Models;
using EntrantsManagementSystem.Logging; 
namespace EntrantsManagementSystem.Controllers
{
    public class CountriesController : Controller
    {
        private EntrantsDatabaseEntities db = new EntrantsDatabaseEntities(); 
        // GET: Location
        public ActionResult List()
        {
            return View(db.Countries.ToList());
        }

        public ActionResult CountrySettings(int? id)
        {
            if (id == null)
                throw new ArgumentNullException();
            Country country = db.Countries.Find(id);
            if (country == null)
                throw new NotSupportedException();
            return View(country); 
        }
        [HttpPost]
        public ActionResult CountrySettings(Country country)
        {
           
                Country databaseCountry = db.Countries.Find(country.CountryID);
                IEnumerator<City> databaseEnumerator = databaseCountry.Cities.GetEnumerator();
                IEnumerator<City> Enumerator = country.Cities.GetEnumerator();
                while(Enumerator.MoveNext() && databaseEnumerator.MoveNext())
                {
                    databaseEnumerator.Current.Name = Enumerator.Current.Name; 
                }
                db.SaveChanges();
           
            return RedirectToAction("List"); 
        }
        public ActionResult CitySettings(int? id)
        {
            if (id == null)
                throw new ArgumentNullException("City ID is null"); 
            return View(db.Cities.Find(id)); 
        }
        [HttpPost]
        public ActionResult CitySettings([Bind(Include ="CityID,Name,CountryID")] City city)
        {
           
            if (ModelState.IsValid)
            {
                db.Entry(city).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CountrySettings", new { id = city.CountryID });
            }
            return View(city); 
            
        }

        public ActionResult DeleteCity(int? id)
        {
            City city = db.Cities.Find(id);
            db.Cities.Remove(city); 
            db.SaveChanges();
            return RedirectToAction("CountrySettings",new { id = city.CountryID });
        }
        public ActionResult CreateCity(int? CountryID)
        {
            ViewBag.CountryID = CountryID;
            return View(); 
        }
        [HttpPost]
        public ActionResult CreateCity([Bind(Include ="CityID,Name,CountryID")] City city)
        {
            if (ModelState.IsValid)
            {
                db.Cities.Add(city);
                db.SaveChanges();
                return RedirectToAction("CountrySettings", new { id = city.CountryID });
            }
            ViewBag.CountryID = city.CountryID;
            return View();





        }

    }
}