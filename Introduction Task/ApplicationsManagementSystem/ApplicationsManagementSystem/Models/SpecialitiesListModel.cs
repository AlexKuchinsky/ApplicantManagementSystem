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
        public Application Application { get; set; }
        public Group Group {get;set;}
        public SpecialitiesListModel(int ApplicationID,int? GroupID=null)
        {
            Application = db.Applications.Find(ApplicationID);
            PaymentType = Application.PaymentType;
            User = Application.User;

            if (GroupID == null)
            {
                IQueryable<IGrouping<int,GroupedSpeciality>> groups = db.GroupedSpecialities.Where(gs => gs.PaymentTypeID == PaymentType.PaymentTypeID /*&& Date*/).GroupBy(gs => gs.SpecialityID);
                foreach(IGrouping<int,GroupedSpeciality> group in groups)
                {
                    Specialities.Add(db.Specialities.Find(group.Key));
                }
            }
            else
            {
                Group = db.Groups.Find(GroupID);
                List<GroupedSpeciality> AvailableSpecialities = Group.GroupedSpecialities.ToList();
                foreach (Group friend in Group.GroupFriendships.Select(gf => gf.Group))
                    AvailableSpecialities.AddRange(friend.GroupedSpecialities);


                IEnumerable<IGrouping<int, GroupedSpeciality>> groups = AvailableSpecialities.Where(gs => gs.PaymentTypeID == PaymentType.PaymentTypeID).Except(Application.SpecialityApplications.Select(sa => sa.GroupedSpeciality)).GroupBy(gs => gs.SpecialityID); 
                foreach (IGrouping<int, GroupedSpeciality> group in groups)
                {
                    Specialities.Add(db.Specialities.Find(group.Key));
                }

            }
            
          
        }

    }
}