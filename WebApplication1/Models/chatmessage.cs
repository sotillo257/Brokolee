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
    
    public partial class chatmessage
    {
        public long id { get; set; }
        public long from_user_id { get; set; }
        public long to_user_id { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public System.DateTime created_at { get; set; }
        public System.DateTime updated_at { get; set; }
        public System.DateTime viewed_at { get; set; }
        public bool is_active { get; set; }
        public Nullable<bool> is_read { get; set; }
        public Nullable<bool> is_email { get; set; }
    
        public virtual user user { get; set; }
        public virtual user user1 { get; set; }
    }
}
