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
    
    public partial class blogcomment
    {
        public long id { get; set; }
        public string comment { get; set; }
        public string name { get; set; }
        public System.DateTime postdate { get; set; }
        public long blog_id { get; set; }
    
        public virtual blog blog { get; set; }
    }
}
