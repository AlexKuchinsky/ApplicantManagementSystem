﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EntrantsDatabaseEntities : DbContext
    {
        public EntrantsDatabaseEntities()
            : base("name=EntrantsDatabaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CertificateMark> CertificateMarks { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Entrant> Entrants { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<Speciality> Specialities { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<University> Universities { get; set; }
    }
}
