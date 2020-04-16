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
    
    public partial class task
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public task()
        {
            this.carpentries = new HashSet<carpentry>();
            this.taskcomments = new HashSet<taskcomment>();
        }
    
        public long id { get; set; }
        public string task_name { get; set; }
        public string responsable { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> estimated_date { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> completed_date { get; set; }
        public Nullable<System.DateTime> archived_date { get; set; }
        public string description { get; set; }
        public string comments { get; set; }
        public Nullable<System.DateTime> task_date { get; set; }
        public Nullable<int> share { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
        public Nullable<bool> is_read { get; set; }
        public Nullable<long> community_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<carpentry> carpentries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<taskcomment> taskcomments { get; set; }
    }
}
