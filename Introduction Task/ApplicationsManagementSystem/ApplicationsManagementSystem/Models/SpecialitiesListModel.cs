using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationsManagementSystem.Models
{
    public class SpecialitiesListModel
    {
        private SpecialitiesDatabaseEntities db = new SpecialitiesDatabaseEntities();
        public User User { get; set; }
        public PaymentType PaymentType { get; set; }
        public List<Speciality> Specialities { get; set; } = new List<Speciality>();
        public List<SpecialityApplication> ChosenSpecialities { get; set; } = new List<SpecialityApplication>();
        public List<IGrouping<int, SpecialityApplication>> GroupedChosenSpecialities { get; set; } = new List<IGrouping<int, SpecialityApplication>>();
        public Application Application { get; set; }
        
        public Group Group {get;set;}
        public SpecialitiesListModel(int ApplicationID,int? GroupID=null)
        {
            Application = db.Applications.Find(ApplicationID);
            PaymentType = Application.PaymentType;
            User = Application.User;
            ApplicationSetting ApplicationSetting = Application.ApplicationSettings.First();
            if (ApplicationSetting.ApplicationGroupID != null)
            {
                Group Group = db.Groups.Find(ApplicationSetting.ApplicationGroupID);
                List<int> GroupsPriority = new List<int>();
                GroupsPriority.Add(Group.GroupID); 
                List<int> interRes = Group.GroupFriendships.OrderBy(gf => gf.Rang).Select(gf => gf.AccessibleGroupID).ToList();
                GroupsPriority.AddRange(interRes);

                List<IGrouping<int, SpecialityApplication>> GroupedNotOrderedSpecialities = Application.SpecialityApplications.GroupBy(sa => sa.GroupedSpeciality.GroupID).ToList();
                List<int> GroupedNotOrderdID = GroupedNotOrderedSpecialities.Select(gnos => gnos.Key).ToList();

                foreach(int priorityIndex in GroupsPriority)
                    if(GroupedNotOrderdID.Contains(priorityIndex))
                        GroupedChosenSpecialities.Add(GroupedNotOrderedSpecialities.First(gnos => gnos.Key == priorityIndex));  
            }
          
            ChosenSpecialities = Application.SpecialityApplications.OrderBy(sa => sa.Priority).ToList();
            if (ChosenSpecialities.Count <= 10)
            {
                if (GroupID == null)
                {
                    IQueryable<IGrouping<int, GroupedSpeciality>> groups = db.GroupedSpecialities.Where(gs => gs.PaymentTypeID == PaymentType.PaymentTypeID /*&& Date*/).GroupBy(gs => gs.SpecialityID);
                    foreach (IGrouping<int, GroupedSpeciality> group in groups)
                    {
                        Specialities.Add(db.Specialities.Find(group.Key));
                    }
                }
                else
                {
                    Group = db.Groups.Find(GroupID);
                    List<GroupedSpeciality> AvailableSpecialities = Group.GroupedSpecialities.ToList();
                    foreach (Group friend in Group.GroupFriendships.Select(gf => gf.Group1))
                    {
                        AvailableSpecialities.AddRange(friend.GroupedSpecialities);
                    }



                    IEnumerable<IGrouping<int, GroupedSpeciality>> groups = AvailableSpecialities.Where(gs => gs.PaymentTypeID == PaymentType.PaymentTypeID).Except(Application.SpecialityApplications.Select(sa => sa.GroupedSpeciality)).GroupBy(gs => gs.SpecialityID);
                    foreach (IGrouping<int, GroupedSpeciality> group in groups)
                    {
                        Specialities.Add(db.Specialities.Find(group.Key));
                    }

                }
            }
            
            
          
        }

    }
}