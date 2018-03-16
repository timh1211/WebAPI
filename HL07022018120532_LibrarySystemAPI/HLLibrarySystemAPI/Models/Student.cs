//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HLLibrarySystemAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            this.AccountStudents = new HashSet<AccountStudent>();
        }
    
        [Key]
        public string stuID { get; set; }
        public string stuFirstName { get; set; }
        public string stuLastName { get; set; }
        public string stuAddress { get; set; }
        public Nullable<long> stuPhone { get; set; }
        public string majorID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountStudent> AccountStudents { get; set; }
        public virtual Major Major { get; set; }
    }
}