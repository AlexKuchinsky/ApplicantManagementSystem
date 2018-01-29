using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationsManagementSystem.Models
{
    public class AddSpecialityModel
    {
        private SpecialitiesDatabaseEntities db = new SpecialitiesDatabaseEntities();

        public Application Application { get; set; }
        public Speciality Speciality { get; set; }
        public int? GroupID { get; set; }

        public AddSpecialityModel(int ApplicationID, int SpecialityID,int? GroupID=null)
        {
            Application = db.Applications.Find(ApplicationID);
            Speciality = db.Specialities.Find(SpecialityID);
            this.GroupID = GroupID; 
        }
    }
}