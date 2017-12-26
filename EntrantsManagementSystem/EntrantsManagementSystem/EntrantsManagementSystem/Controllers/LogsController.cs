using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntrantsManagementSystem.Models; 
namespace EntrantsManagementSystem.Controllers
{
    public class LogsController : Controller
    {
        // GET: Logs
        private EntrantsDatabaseEntities db = new EntrantsDatabaseEntities();

        public ActionResult UpdateLogsList()
        {
            List<UpdateLog> updateLogs = db.UpdateLogs.ToList();
            updateLogs.Reverse();
            return View(updateLogs);
        }
        public ActionResult DeleteLogsList()
        {
            List<DeleteLog> deleteLogs = db.DeleteLogs.ToList();
            deleteLogs.Reverse();
            return View(deleteLogs);
        }
        public ActionResult ExceptionLogsList()
        {
            List<ExceptionLog> exceptionLogs = db.ExceptionLogs.ToList();
            exceptionLogs.Reverse();
            return View(exceptionLogs);
        }
    }
}