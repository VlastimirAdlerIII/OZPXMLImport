//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OZPXMLImport.DataModelSQL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pojistovna
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pojistovna()
        {
            this.Smlouva = new HashSet<Smlouva>();
        }
    
        public int Id { get; set; }
        public string Nazev { get; set; }
        public string Zkratka { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Smlouva> Smlouva { get; set; }
    }
}
