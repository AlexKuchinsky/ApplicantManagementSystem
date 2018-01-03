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
        private LogsDatabaseEntities ldb = new LogsDatabaseEntities(); 

        public ActionResult UpdateLogsList()
        {
            List<ChangesLog> updateLogs = ldb.ChangesLogs.ToList();
            updateLogs.Reverse();
            return View(updateLogs);
        }
        public ActionResult ExceptionLogsList()
        {
            List<ExceptionLog> exceptionLogs = ldb.ExceptionLogs.ToList();
            exceptionLogs.Reverse();
            return View(exceptionLogs);
        }
    }
}