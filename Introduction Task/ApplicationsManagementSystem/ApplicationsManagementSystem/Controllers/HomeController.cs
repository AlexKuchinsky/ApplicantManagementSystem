using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity; 
using System.Web;
using System.Web.Mvc;
using System.Net.Mail; 
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
        public ActionResult UserMenu(int UserID = 2)
        {
            SendMessage();
            return View(new UserPageModel(UserID));
        }
        public ActionResult Index()
        {
            SendMessage();
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
                ApplicationID = NewApplication.ApplicationID,
                NumberOfMainGroupSpecialities = 0
            };
            db.ApplicationSettings.Add(appSetting);
            db.SaveChanges();
            return RedirectToAction("SpecialitiesList",new { ApplicationID = NewApplication.ApplicationID });
        }
        public ActionResult SpecialitiesList(int ApplicationID, int? GroupID = null)
        {
            return View(new SpecialitiesListModel(ApplicationID,GroupID));
        }
        [HttpGet]
        public ActionResult AddSpeciality(int ApplicationID, int SpecialityID, int? GroupID = null)
        {
            return View(new AddSpecialityModel(ApplicationID, SpecialityID, GroupID));
        }
        [HttpPost]
        public ActionResult AddSpeciality(AddedGruopedSpecilityID speciality)
        {

            Application Application = db.Applications.Find(speciality.ApplicationID);
            ApplicationSetting ApplicationSetting = Application.ApplicationSettings.First();

            // Create SpecialityApplication
            SpecialityApplication SpecialityApplication = new SpecialityApplication();
            SpecialityApplication.ApplicationID = speciality.ApplicationID;
            SpecialityApplication.GroupedSpecialityID = speciality.GroupedSpecialityID;

            // Set main groupid if adding new speciality
            if (speciality.GroupID == null)
            {
                ApplicationSetting.ApplicationGroupID = db.GroupedSpecialities.Find(speciality.GroupedSpecialityID).GroupID;
                db.Entry(ApplicationSetting).State = EntityState.Modified;
                db.SaveChanges();
            }
            int NewSpecialityGroupID = db.GroupedSpecialities.Find(speciality.GroupedSpecialityID).GroupID;

            if (NewSpecialityGroupID == ApplicationSetting.ApplicationGroupID)
            {
                SpecialityApplication.Priority = ApplicationSetting.NumberOfMainGroupSpecialities ?? 0;
                ApplicationSetting.NumberOfMainGroupSpecialities++;
                db.Entry(ApplicationSetting).State = EntityState.Modified;
            }
            else
            {
                SpecialityApplication.Priority = Application.SpecialityApplications.Select(sa => sa.GroupedSpeciality.GroupID).Where(gID => gID == NewSpecialityGroupID).Count();
            }
            db.SpecialityApplications.Add(SpecialityApplication);
            db.SaveChanges();





            int GroupID = ApplicationSetting.ApplicationGroupID ?? throw new NotImplementedException();
            return RedirectToAction("SpecialitiesList", new { ApplicationID =  Application.ApplicationID, GroupID = ApplicationSetting.ApplicationGroupID  });

        }
        public ActionResult UserPage(int? id)
        {
            return null;
        }


        public JsonResult GetDurationTypes(int ApplicationID, int SpecialityID, int? GroupID = null)
        {
            List<DurationType> result = GetAvailableDurationTypesList(db.Applications.Find(ApplicationID), SpecialityID);
            return Json(result.Select(r => new { Description = r.Description, DurationTypeID = r.DurationTypeID }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStudyFormTypes(int ApplicationID, int SpecialityID, int DurationTypeID, int? GroupID = null)
        {
            List<StudyFormType> result = GetAvailableStudyFormTypes(db.Applications.Find(ApplicationID), SpecialityID, DurationTypeID);
            return Json(result.Select(r => new { Description = r.Description, StudyFormTypeID = r.StudyFormTypeID }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTestOptions(int ApplicationID, int SpecialityID, int DurationTypeID, int StudyFormTypeID, int? GroupID = null)
        {
            GroupedSpeciality ChosenGroupedSpeciality = GetChosenGroupedSpeciality(db.Applications.Find(ApplicationID), SpecialityID, DurationTypeID, StudyFormTypeID);
            SpecialityTestOption result = ChosenGroupedSpeciality.SpecialityTestOption;
            SpecialitySubject FirstProfSubject = result.ProfileSubjectsGroups.First(psg => psg.Rang == 1).SpecialitySubject;
            SpecialitySubject SecondProfSubject = result.ProfileSubjectsGroups.First(psg => psg.Rang == 2).SpecialitySubject;
            var OptionalSubjects = result.OptionalSubjectsGroups.Select(osg => osg.SpecialitySubject).Select(op => new { Title = op.Subject.Description, MinScore = op.MinScore.Score }).ToList();
            return Json(new
            {
                GroupedSpecialityID = ChosenGroupedSpeciality.GroupedSpecialityID,
                FirstProfileSubject = new { Title = FirstProfSubject.Subject.Description, MinScore = FirstProfSubject.MinScore?.Score ?? -1 },
                SecondProfileSubject = new { Title = SecondProfSubject.Subject.Description, MinScore = SecondProfSubject.MinScore?.Score ?? -1 },
                OptionalSubjects = OptionalSubjects
            },
                JsonRequestBehavior.AllowGet);
        }


        public List<GroupedSpeciality> GetAvailableGroupedSpecialities(Application Application, int SpecialityID)
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
                List<GroupedSpeciality> GroupedSpecialities = ApplicationGroup.GroupedSpecialities.Where(gs => gs.SpecialityID == SpecialityID && gs.PaymentTypeID == Application.PaymentTypeID).ToList();
                foreach (Group g in ApplicationGroup.GroupFriendships.Select(gf => gf.Group1))
                    GroupedSpecialities.AddRange(g.GroupedSpecialities.Where(gs => gs.SpecialityID == SpecialityID && gs.PaymentTypeID == Application.PaymentTypeID));
                return GroupedSpecialities.Except(userGrouped).ToList();


            }

        }
        public List<GroupedSpeciality> GetAvailableGroupedSpecialities(Application Application, int SpecialityID, int DurationTypeID)
        {
            List<GroupedSpeciality> IntermediateResult = GetAvailableGroupedSpecialities(Application, SpecialityID);
            return IntermediateResult.Where(ir => ir.DurationTypeID == DurationTypeID).ToList();
        }


        public List<DurationType> GetAvailableDurationTypesList(Application Application, int SpecialityID)
        {
            HashSet<DurationType> uniqueResults = new HashSet<DurationType>(GetAvailableGroupedSpecialities(Application, SpecialityID).Select(gs => gs.DurationType));
            return uniqueResults.ToList();

        }
        public List<StudyFormType> GetAvailableStudyFormTypes(Application Application, int SpecialityID, int DurationTypeID)
        {
            HashSet<StudyFormType> uniqueResults = new HashSet<StudyFormType>(GetAvailableGroupedSpecialities(Application, SpecialityID, DurationTypeID).Select(gs => gs.StudyFormType));
            return uniqueResults.ToList();
        }
        public GroupedSpeciality GetChosenGroupedSpeciality(Application Application, int SpecialityID, int DurationTypeID, int StudyFormTypeID)
        {
            List<GroupedSpeciality> IntermediateResult = GetAvailableGroupedSpecialities(Application, SpecialityID, DurationTypeID);
            return IntermediateResult.Where(ir => ir.StudyFormTypeID == StudyFormTypeID).First();
        }

        public void ChangePriority(int ApplicationID, int StartPosition, int EndPosition, int GroupID)
        {
            if (StartPosition != EndPosition)
            {
                Application Application = db.Applications.Find(ApplicationID);
                List<SpecialityApplication> SpecialityApplications = Application.SpecialityApplications.Where(sa => sa.GroupedSpeciality.GroupID == GroupID).OrderBy(sa => sa.Priority).ToList();

                if (StartPosition < EndPosition)
                {
                    for (int i = StartPosition + 1; i < SpecialityApplications.Count && i <= EndPosition; i++)
                        SpecialityApplications[i].Priority--;
                    SpecialityApplication tmp = SpecialityApplications[StartPosition];
                    tmp.Priority = EndPosition;
                    SpecialityApplications.RemoveAt(StartPosition);
                    SpecialityApplications.Insert(EndPosition, tmp);
                }
                else
                {
                    for (int i = EndPosition; i < SpecialityApplications.Count && i < StartPosition; i++)
                        SpecialityApplications[i].Priority++;
                    SpecialityApplication tmp = SpecialityApplications[StartPosition];
                    tmp.Priority = EndPosition;
                    SpecialityApplications.RemoveAt(StartPosition);
                    SpecialityApplications.Insert(EndPosition, tmp);
                }
                foreach (SpecialityApplication sp in SpecialityApplications)
                    db.Entry(sp).State = EntityState.Modified;
                db.SaveChanges();
            }



        }
        [HttpPost]
        public ActionResult SpecialitiesList(int ApplicationID)
        {
            Application Application = db.Applications.Find(ApplicationID);
            ApplicationSetting ApplicationSetting = Application.ApplicationSettings.First();
            List<IGrouping<int, SpecialityApplication>> GroupedChosenSpecialities = new List<IGrouping<int, SpecialityApplication>>();
            Group Group = db.Groups.Find(ApplicationSetting.ApplicationGroupID);
            List<int> GroupsPriority = new List<int>();
            GroupsPriority.Add(Group.GroupID);
            List<int> interRes = Group.GroupFriendships.OrderBy(gf => gf.Rang).Select(gf => gf.AccessibleGroupID).ToList();
            GroupsPriority.AddRange(interRes);

            List<IGrouping<int, SpecialityApplication>> GroupedNotOrderedSpecialities = Application.SpecialityApplications.GroupBy(sa => sa.GroupedSpeciality.GroupID).ToList();
            List<int> GroupedNotOrderdID = GroupedNotOrderedSpecialities.Select(gnos => gnos.Key).ToList();

            foreach (int priorityIndex in GroupsPriority)
                if (GroupedNotOrderdID.Contains(priorityIndex))
                    GroupedChosenSpecialities.Add(GroupedNotOrderedSpecialities.First(gnos => gnos.Key == priorityIndex));
            SpecialitiesListModel ListModel = new SpecialitiesListModel(ApplicationID);
            int priorityIndexx = 0; 
            foreach(IGrouping<int,SpecialityApplication> groupedSA in GroupedChosenSpecialities)
            {
                foreach (SpecialityApplication sa in groupedSA.OrderBy(gsa=>gsa.Priority))
                {
                    sa.Priority = priorityIndexx++;
                    db.Entry(sa).State = EntityState.Modified; 
                }
            }
            Application.Submitted = 1;
            db.Entry(Application).State = EntityState.Modified; 
            db.SaveChanges();
            return RedirectToAction("Application", new { ApplicationID }); 
        }
        public ActionResult Application(int ApplicationID)
        {
            return View(db.Applications.Find(ApplicationID)); 
        }
        public ActionResult DeleteApplication(int ApplicationID)
        {
            Application Application = db.Applications.Find(ApplicationID); 
            int UserID = Application.User.UserID;
            db.Applications.Remove(Application);
            db.SaveChanges();
            return RedirectToAction("UserMenu", new { UserID });
        }
        public ActionResult DeleteSpeciality(int ApplicationID, int GroupID, int SpecialityApplicationID)
        {
            Application Application = db.Applications.Find(ApplicationID);
            SpecialityApplication specialityApplication = db.SpecialityApplications.Find(SpecialityApplicationID);
            List<SpecialityApplication> Specialities = Application.SpecialityApplications.Where(sa => sa.GroupedSpeciality.GroupID == GroupID).OrderBy(sa => sa.Priority).ToList();
            int indexToDelete = Specialities.IndexOf(specialityApplication);

            for (int i = 0; i < Specialities.Count(); i++)
                if (i > indexToDelete)
                {
                    Specialities[i].Priority--;
                    db.Entry(Specialities[i]).State = EntityState.Modified;
                }
            db.SpecialityApplications.Remove(specialityApplication);
            db.SaveChanges();                    

              
            return RedirectToAction("SpecialitiesList", new { ApplicationID , GroupID = Application.ApplicationSettings.First().ApplicationGroupID});
        }

        public void SendMessage()
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("@gmail.com");
            mail.To.Add("");
            mail.Subject = "";
            mail.Body = "<body>" +           
            "</body>";
            mail.IsBodyHtml = true;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("@gmail.com", "");
           
            SmtpServer.EnableSsl = true; 
            SmtpServer.Send(mail); 
        }
    }
}