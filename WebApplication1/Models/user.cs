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
    
    public partial class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
            this.banks = new HashSet<bank>();
            this.chatmessages = new HashSet<chatmessage>();
            this.chatmessages1 = new HashSet<chatmessage>();
            this.communusers = new HashSet<communuser>();
            this.creditcards = new HashSet<creditcard>();
            this.emailthemes = new HashSet<emailtheme>();
            this.fees = new HashSet<fee>();
            this.friendmappings = new HashSet<friendmapping>();
            this.friendmappings1 = new HashSet<friendmapping>();
            this.onlineusers = new HashSet<onlineuser>();
            this.payments = new HashSet<payment>();
            this.payments1 = new HashSet<payment>();
            this.taskcomments = new HashSet<taskcomment>();
            this.taskusers = new HashSet<taskuser>();
            this.usoes = new HashSet<uso>();
            this.Titulos = new HashSet<Titulo>();
            this.blogs = new HashSet<blog>();
            this.BlogUserLikes = new HashSet<BlogUserLike>();
        }
    
        public long id { get; set; }
        public string first_name1 { get; set; }
        public string last_name1 { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool is_admin { get; set; }
        public bool is_logged { get; set; }
        public string first_name2 { get; set; }
        public string last_name2 { get; set; }
        public string mother_last_name1 { get; set; }
        public string mother_last_name2 { get; set; }
        public string phone_number1 { get; set; }
        public string phone_number2 { get; set; }
        public string postal_address { get; set; }
        public string residential_address { get; set; }
        public string upload_writing { get; set; }
        public string user_img { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public string colour { get; set; }
        public Nullable<int> year { get; set; }
        public string clapboard { get; set; }
        public Nullable<int> stamp_number { get; set; }
        public Nullable<bool> is_leased { get; set; }
        public Nullable<System.DateTime> since { get; set; }
        public Nullable<System.DateTime> until { get; set; }
        public string tenant_first_name1 { get; set; }
        public string tenant_last_name1 { get; set; }
        public string tenant_mother_last_name1 { get; set; }
        public string tenant_first_name2 { get; set; }
        public string tenant_last_name2 { get; set; }
        public string tenant_mother_last_name2 { get; set; }
        public string leased_postal_address { get; set; }
        public string leased_residential_address { get; set; }
        public string leased_upload_file { get; set; }
        public bool is_active { get; set; }
        public int role { get; set; }
        public System.DateTime acq_date { get; set; }
        public string siono { get; set; }
        public Nullable<long> create_userid { get; set; }
        public Nullable<long> package_id { get; set; }
        public Nullable<bool> is_paid { get; set; }
        public Nullable<System.DateTime> paid_at { get; set; }
        public Nullable<bool> is_del { get; set; }
        public string apartment { get; set; }
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        public string Codigo_Postal { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<bank> banks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<chatmessage> chatmessages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<chatmessage> chatmessages1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<communuser> communusers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<creditcard> creditcards { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<emailtheme> emailthemes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fee> fees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<friendmapping> friendmappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<friendmapping> friendmappings1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<onlineuser> onlineusers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<payment> payments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<payment> payments1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<taskcomment> taskcomments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<taskuser> taskusers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<uso> usoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Titulo> Titulos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<blog> blogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlogUserLike> BlogUserLikes { get; set; }
    }
}
