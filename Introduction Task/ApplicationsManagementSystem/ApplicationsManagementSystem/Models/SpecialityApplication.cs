//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApplicationsManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SpecialityApplication
    {
        public int SpecialityApplicationID { get; set; }
        public int ApplicationID { get; set; }
        public int GroupedSpecialityID { get; set; }
        public int Priority { get; set; }
    
        public virtual Application Application { get; set; }
        public virtual GroupedSpeciality GroupedSpeciality { get; set; }
    }
}
