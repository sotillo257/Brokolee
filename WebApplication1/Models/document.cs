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
    
    public partial class document
    {
        public long id { get; set; }
        public string first_name { get; set; }
        public string uploaded_by { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public int type_id { get; set; }
        public string upload_document { get; set; }
        public Nullable<int> share { get; set; }
        public Nullable<bool> notify_email { get; set; }
        public Nullable<System.DateTime> uploaded_date { get; set; }
        public string link { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
    
        public virtual document_type document_type { get; set; }
    }
}
