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
    
    public partial class @event
    {
        public long id { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public string place { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public string name { get; set; }
        public System.DateTime event_date { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
        public int share { get; set; }
        public Nullable<bool> is_active { get; set; }
    }
}
