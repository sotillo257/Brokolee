//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class transaction
    {
        public long id { get; set; }
        public string trans_id { get; set; }
        public Nullable<System.DateTime> date_info { get; set; }
        public string kind { get; set; }
        public Nullable<decimal> debit { get; set; }
        public Nullable<decimal> credit { get; set; }
        public Nullable<decimal> balance { get; set; }
        public string state { get; set; }
    }
}
