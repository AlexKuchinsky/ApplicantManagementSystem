//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntrantsManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CertificateMark
    {
        public int CertificateMarkID { get; set; }
        public int EntrantID { get; set; }
        public int SubjectID { get; set; }
        public int Mark { get; set; }
    
        public virtual Entrant Entrant { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
