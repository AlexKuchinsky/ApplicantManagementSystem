using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationsManagementSystem.Models
{
    public class AddedGruopedSpecilityID
    {
        public int ApplicationID { get; set; }
        public int GroupedSpecialityID { get; set; }
        public int? GroupID { get; set; }
    }
}