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
    
    public partial class contract
    {
        public long id { get; set; }
        public string first_name { get; set; }
        public Nullable<System.DateTime> date_contract { get; set; }
        public Nullable<System.DateTime> date_notification { get; set; }
        public string upload_contract { get; set; }
        public string description { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
        public Nullable<long> community_id { get; set; }
    }
}
