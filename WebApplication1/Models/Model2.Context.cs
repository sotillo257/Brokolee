﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class pjrdev_condominiosEntities : DbContext
    {
        public pjrdev_condominiosEntities()
            : base("name=pjrdev_condominiosEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<bank> banks { get; set; }
        public virtual DbSet<blogcomment> blogcomments { get; set; }
        public virtual DbSet<carpentry> carpentries { get; set; }
        public virtual DbSet<category> categories { get; set; }
        public virtual DbSet<chatmessage> chatmessages { get; set; }
        public virtual DbSet<comment> comments { get; set; }
        public virtual DbSet<community> communities { get; set; }
        public virtual DbSet<communuser> communusers { get; set; }
        public virtual DbSet<creditcard> creditcards { get; set; }
        public virtual DbSet<document_type> document_type { get; set; }
        public virtual DbSet<emailtheme> emailthemes { get; set; }
        public virtual DbSet<emailtype> emailtypes { get; set; }
        public virtual DbSet<facsche> facsches { get; set; }
        public virtual DbSet<friendmapping> friendmappings { get; set; }
        public virtual DbSet<notification> notifications { get; set; }
        public virtual DbSet<onlineuser> onlineusers { get; set; }
        public virtual DbSet<package> packages { get; set; }
        public virtual DbSet<payment> payments { get; set; }
        public virtual DbSet<schedule> schedules { get; set; }
        public virtual DbSet<suscribe> suscribes { get; set; }
        public virtual DbSet<taskcomment> taskcomments { get; set; }
        public virtual DbSet<taskuser> taskusers { get; set; }
        public virtual DbSet<tooltip> tooltips { get; set; }
        public virtual DbSet<transaction> transactions { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<uso> usoes { get; set; }
        public virtual DbSet<Vehiculo> Vehiculos { get; set; }
        public virtual DbSet<Titulo> Titulos { get; set; }
        public virtual DbSet<BlogUserLike> BlogUserLikes { get; set; }
        public virtual DbSet<contactinfo> contactinfoes { get; set; }
        public virtual DbSet<@event> events { get; set; }
        public virtual DbSet<document> documents { get; set; }
        public virtual DbSet<efac> efacs { get; set; }
        public virtual DbSet<blog> blogs { get; set; }
        public virtual DbSet<task> tasks { get; set; }
        public virtual DbSet<fee> fees { get; set; }
        public virtual DbSet<book> books { get; set; }
        public virtual DbSet<contract> contracts { get; set; }
        public virtual DbSet<supplier> suppliers { get; set; }
    }
}
