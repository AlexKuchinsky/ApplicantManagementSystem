﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity; 
using System.Web;
using System.Web.Mvc;
using ApplicationsManagementSystem.Models;
namespace ApplicationsManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        class DurationTypeIEqualityComparer : IEqualityComparer<DurationType>
        {
            public bool Equals(DurationType x, DurationType y)
            {
                return x.DurationTypeID == y.DurationTypeID;
            }

            public int GetHashCode(DurationType obj)
            {
                throw new NotImplementedException();
            }
        }
        // GET: Home
        SpecialitiesDatabaseEntities db = new SpecialitiesDatabaseEntities();
        public ActionResult UserMenu(int id = 2)
        {
            return View(new UserPageModel(id));
        }
        public ActionResult Index()
        {
            return RedirectToAction("UserMenu");
        }
        public ActionResult NewSpecialitiesList(int UserID, int PaymentTypeID)
        {

            Application NewApplication = new Application()
            {
                UserID = UserID,
                PaymentTypeID = PaymentTypeID
            };
            db.Applications.Add(NewApplication);
            db.SaveChanges();
            ApplicationSetting appSetting = new ApplicationSetting()
            {
                ApplicationID = NewApplication.ApplicationID
            };
            db.ApplicationSettings.Add(appSetting);
            db.SaveChanges();
            return View("SpecialitiesList",new SpecialitiesListModel(NewApplication.ApplicationID)); 
        }
        public ActionResult SpecialitiesList(int ApplicationID,int? GroupID=null)
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddSpeciality(int ApplicationID,int SpecialityID,int? GroupID=null)
        {
            return View(new AddSpecialityModel(ApplicationID,SpecialityID,GroupID));
        }
        [HttpPost]
        public ActionResult AddSpeciality(AddedGruopedSpecilityID speciality)
        {
            SpecialityApplication SpecialityApplication = new SpecialityApplication();
            SpecialityApplication.ApplicationID = speciality.ApplicationID;
            SpecialityApplication.GroupedSpecialityID = speciality.GroupedSpecialityID;
            SpecialityApplication.Priority = 0;
            db.SpecialityApplications.Add(SpecialityApplication);
            db.SaveChanges();

            Application Application = db.Applications.Find(speciality.ApplicationID);
            ApplicationSetting ApplicationSetting = Application.ApplicationSettings.First();

            if (speciality.GroupID == null)
            {
                ApplicationSetting.ApplicationGroupID = db.GroupedSpecialities.Find(speciality.GroupedSpecialityID).GroupID;
                db.Entry(ApplicationSetting).State = EntityState.Modified;
                db.SaveChanges();
            }
            int GroupID = ApplicationSetting.ApplicationGroupID?? throw new NotImplementedException();
            return View("SpecialitiesList", new SpecialitiesListModel(speciality.ApplicationID,GroupID));
            
        }
        public ActionResult UserPage(int? id)
        {
            return null; 
        }


        public JsonResult GetDurationTypes(int? ApplicationID, int SpecialityID,int? GroupID=null)
        {
            List<DurationType> result = GetAvailableDurationTypesList(db.Applications.Find(ApplicationID), SpecialityID); 
            return Json(result.Select(r=>new { Description = r.Description, DurationTypeID = r.DurationTypeID }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStudyFormTypes(int ApplicationID,int SpecialityID,int DurationTypeID, int? GroupID = null)
        {
            List<StudyFormType> result = GetAvailableStudyFormTypes(db.Applications.Find(ApplicationID), SpecialityID, DurationTypeID); 
            return Json(result.Select(r=>new { Description = r.Description, StudyFormTypeID = r.StudyFormTypeID }), JsonRequestBehavior.AllowGet); 
        }
        public JsonResult GetTestOptions(int ApplicationID, int SpecialityID, int DurationTypeID,int StudyFormTypeID, int? GroupID = null)
        {
            GroupedSpeciality ChosenGroupedSpeciality = GetChosenGroupedSpeciality(db.Applications.Find(ApplicationID), SpecialityID, DurationTypeID, StudyFormTypeID);
            SpecialityTestOption result = ChosenGroupedSpeciality.SpecialityTestOption;
            SpecialitySubject FirstProfSubject = result.ProfileSubjectsGroups.First(psg => psg.Rang == 1).SpecialitySubject;
            SpecialitySubject SecondProfSubject = result.ProfileSubjectsGroups.First(psg => psg.Rang == 2).SpecialitySubject;
            var OptionalSubjects = result.OptionalSubjectsGroups.Select(osg => osg.SpecialitySubject).Select(op=> new { Title = op.Subject.Description, MinScore = op.MinScore.Score }).ToList();
            return Json(new
            {
                GroupedSpecialityID = ChosenGroupedSpeciality.GroupedSpecialityID,
                FirstProfileSubject = new { Title = FirstProfSubject.Subject.Description, MinScore = FirstProfSubject.MinScore?.Score ?? -1 },
                SecondProfileSubject = new { Title = SecondProfSubject.Subject.Description, MinScore = SecondProfSubject.MinScore?.Score ?? -1 },
                OptionalSubjects = OptionalSubjects
            },
                JsonRequestBehavior.AllowGet);
        }


        public List<GroupedSpeciality> GetAvailableGroupedSpecialities(Application Application,int SpecialityID)
        {
            List<GroupedSpeciality> userGrouped = Application.SpecialityApplications.Select(sa => sa.GroupedSpeciality).Where(gs => gs.SpecialityID == SpecialityID).ToList();
            if (Application.ApplicationSettings.First().ApplicationGroupID == null)
            {
                List<GroupedSpeciality> exsistinGrouped = db.GroupedSpecialities.Where(gs => gs.SpecialityID == SpecialityID && gs.PaymentTypeID == Application.PaymentTypeID).ToList();
                
                return exsistinGrouped.Except(userGrouped).ToList();
            }
            else
            {
                Group ApplicationGroup = db.Groups.Find(Application.ApplicationSettings.First().ApplicationGroupID);
                List<GroupedSpeciality> GroupedSpecialities = ApplicationGroup.GroupedSpecialities.Where(gs => gs.SpecialityID == SpecialityID && gs.PaymentTypeID == Application.PaymentTypeID ).ToList();
                foreach (Group g in ApplicationGroup.GroupFriendships.Select(gf => gf.Group1))
                    GroupedSpecialities.AddRange(g.GroupedSpecialities.Where(gs => gs.SpecialityID == SpecialityID && gs.PaymentTypeID == Application.PaymentTypeID));
                return GroupedSpecialities.Except(userGrouped).ToList();
                

            }

        }
        public List<GroupedSpeciality> GetAvailableGroupedSpecialities(Application Application,int SpecialityID,int DurationTypeID)
        {
            List<GroupedSpeciality> IntermediateResult = GetAvailableGroupedSpecialities(Application, SpecialityID);
            return IntermediateResult.Where(ir => ir.DurationTypeID == DurationTypeID).ToList();
        }


        public List<DurationType> GetAvailableDurationTypesList(Application Application,int SpecialityID)
        {
            HashSet<DurationType> uniqueResults = new HashSet<DurationType>(GetAvailableGroupedSpecialities(Application, SpecialityID).Select(gs => gs.DurationType));
            return uniqueResults.ToList();
            
        }
        public List<StudyFormType> GetAvailableStudyFormTypes(Application Application,int SpecialityID,int DurationTypeID)
        {
            HashSet<StudyFormType> uniqueResults = new HashSet<StudyFormType>(GetAvailableGroupedSpecialities(Application, SpecialityID, DurationTypeID).Select(gs=>gs.StudyFormType));
            return uniqueResults.ToList();
        }
        public GroupedSpeciality GetChosenGroupedSpeciality(Application Application, int SpecialityID, int DurationTypeID, int StudyFormTypeID)
        {
            List<GroupedSpeciality> IntermediateResult = GetAvailableGroupedSpecialities(Application, SpecialityID, DurationTypeID);
            return IntermediateResult.Where(ir => ir.StudyFormTypeID == StudyFormTypeID).First();
        }

        public void ChangePriority(int? ApplicationID,int? StartPosition, int? EndPosition)
        {

        }

    }
}