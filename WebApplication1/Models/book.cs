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
    
    public partial class book
    {
        public long id { get; set; }
        public string first_name { get; set; }
        public string description { get; set; }
        public Nullable<System.TimeSpan> start_time { get; set; }
        public Nullable<System.TimeSpan> end_time { get; set; }
        public Nullable<System.DateTime> requested_date { get; set; }
        public Nullable<decimal> cost_per_reservation { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
    }
}
