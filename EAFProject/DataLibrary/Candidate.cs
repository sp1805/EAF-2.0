//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class Candidate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Candidate()
        {
            this.CandidateMappings = new HashSet<CandidateMapping>();
        }
    
        public int TempId { get; set; }
        public string Name { get; set; }
        public Nullable<long> PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CandidateStatus { get; set; }
        public string resume { get; set; }
        public Nullable<System.DateTime> interviewdatetime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CandidateMapping> CandidateMappings { get; set; }
        public virtual Request Request { get; set; }
    }
}
