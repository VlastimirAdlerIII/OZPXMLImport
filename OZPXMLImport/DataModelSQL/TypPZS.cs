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
    
    public partial class TypPZS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TypPZS()
        {
            this.PoskytovatelZdravotnichSluzeb = new HashSet<PoskytovatelZdravotnichSluzeb>();
        }
    
        public int Id { get; set; }
        public string Kod { get; set; }
        public string Nazev { get; set; }
        public System.DateTime DatumOd { get; set; }
        public Nullable<System.DateTime> DatumDo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PoskytovatelZdravotnichSluzeb> PoskytovatelZdravotnichSluzeb { get; set; }
    }
}
