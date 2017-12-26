using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace EntrantsManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    public partial class Entrant
    {
        public Entrant()
        {
            this.Marks = new HashSet<Mark>();
        }
    
        public int EntrantID { get; set; }
        [Required(ErrorMessage ="Name is required")]
        [StringLength(20,ErrorMessage ="Name should be shorter than 20 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname is required")]
        [StringLength(20, ErrorMessage = "Surname should be shorter than 20 characters")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Patronumic is required")]
        [StringLength(20, ErrorMessage = "Patronumic should be shorter than 20 characters")]
        public string Patronumic { get; set; }
        [Required(ErrorMessage ="Please, enter your email adress")]
        [StringLength(30, ErrorMessage = "Email adress should be shorter than 20 characters")]
        [RegularExpression(".+\\@.+\\..+",ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
        public Nullable<int> CityID { get; set; }
    
        public virtual City City { get; set; }
        public virtual ICollection<Mark> Marks { get; set; }
    }
}
