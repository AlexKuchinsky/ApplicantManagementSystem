using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EntrantsManagementSystem.Models
{
    public class CreateEntrantModel: Entrant
    {
        public EntrantsDatabaseEntities db = new EntrantsDatabaseEntities();
        public override IList<CertificateMark> CertificateMarks { get; set; }
        

        public CreateEntrantModel()
        {
            CertificateMarks = new List<CertificateMark>();

            List<Subject> Subjects = db.Subjects.ToList();
            foreach (Subject subject in Subjects)
            {
                CertificateMark certificateMark = new CertificateMark()
                {
                    Subject = subject,
                    SubjectID = subject.SubjectID,
                };
 
                
                CertificateMarks.Add(certificateMark);
   
            }
        }
    }
}