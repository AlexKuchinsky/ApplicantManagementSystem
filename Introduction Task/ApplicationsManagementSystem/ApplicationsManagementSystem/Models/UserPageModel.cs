using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationsManagementSystem.Models
{
    public class UserPageModel
    {
        private SpecialitiesDatabaseEntities db = new SpecialitiesDatabaseEntities();
        public List<Application> UserApplications { get; set; }
        public List<PaymentType> AvailableApplicationTypes { get; set; }
        public User User { get; set; }
        public UserPageModel(int id)
        {
            User = db.Users.Find(id);
            UserApplications = User.Applications.ToList();
            List<int> ChosenPaymentGroups = UserApplications.Select(a => a.PaymentTypeID).ToList();
            AvailableApplicationTypes = db.PaymentTypes.Where(pt => !ChosenPaymentGroups.Contains(pt.PaymentTypeID)).ToList();

            
        }
    }
}