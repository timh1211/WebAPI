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
    
    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            this.Rent_Details = new HashSet<Rent_Details>();
        }
    
        public string callNumber { get; set; }
        public string ISBN_Number { get; set; }
        public string title { get; set; }
        public string authorName { get; set; }
        public string image { get; set; }
        public string categoryID { get; set; }
        public Nullable<int> quantity { get; set; }
        public string statusBook { get; set; }
        public Nullable<bool> special { get; set; }
        public Nullable<bool> lasted { get; set; }
        public Nullable<int> views { get; set; }
    
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rent_Details> Rent_Details { get; set; }
    }
}
