//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain.GeneratedFromDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Lecturers
    {
        public Lecturers()
        {
            this.Courses = new HashSet<Courses>();
        }
    
        public System.Guid ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Nullable<bool> IsAcademic { get; set; }
        public string Interests { get; set; }
        public string Information { get; set; }
        public string Email { get; set; }
    
        public virtual ICollection<Courses> Courses { get; set; }
    }
}
