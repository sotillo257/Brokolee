//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class supplier
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public supplier()
        {
            this.comments = new HashSet<comment>();
        }
    
        public long id { get; set; }
        public string logo { get; set; }
        public string municipality { get; set; }
        public int category_id { get; set; }
        public string description { get; set; }
        public Nullable<int> assessment { get; set; }
        public string company { get; set; }
        public string contact_name { get; set; }
        public string type_service { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public Nullable<System.DateTime> supplier_from { get; set; }
        public string web_page { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
        public Nullable<long> community_id { get; set; }
        public Nullable<int> status { get; set; }
    
        public virtual category category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comment> comments { get; set; }
    }
}
