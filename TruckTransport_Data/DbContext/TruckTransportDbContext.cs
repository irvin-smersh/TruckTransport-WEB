namespace TruckTransport_Data.DbContext {
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Entities;

    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public partial class TruckTransportDbContext : DbContext {
        public TruckTransportDbContext() : base("name=TruckTransportDbContext") {
           
        }

        public virtual DbSet<dispecerlogin> dispecerlogin { get; set; }
        public virtual DbSet<geotacke> geotacke { get; set; }
        public virtual DbSet<kategorije> kategorije { get; set; }
        public virtual DbSet<logiranidogadjaji> logiranidogadjaji { get; set; }
        public virtual DbSet<nalozi> nalozi { get; set; }
        public virtual DbSet<poruke> poruke { get; set; }
        public virtual DbSet<postavke> postavke { get; set; }
        public virtual DbSet<poznatelokacije> poznatelokacije { get; set; }
        public virtual DbSet<stajalista> stajalista { get; set; }
        public virtual DbSet<stajalista_nalozi> stajalista_nalozi { get; set; }
        public virtual DbSet<stanja> stanja { get; set; }
        public virtual DbSet<vozaci> vozaci { get; set; }
        public virtual DbSet<vozila> vozila { get; set; }
        public virtual DbSet<zadaci> zadaci { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<dispecerlogin>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<dispecerlogin>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<dispecerlogin>()
                .Property(e => e.email)
                .IsUnicode(false);
         
            modelBuilder.Entity<geotacke>()
                .Property(e => e.duzina)
                .IsUnicode(false);

            modelBuilder.Entity<geotacke>()
                .Property(e => e.sirina)
                .IsUnicode(false);

            modelBuilder.Entity<kategorije>()
                .Property(e => e.naziv)
                .IsUnicode(false);

            modelBuilder.Entity<kategorije>()
                .HasMany(e => e.poznatelokacije)
                .WithRequired(e => e.kategorije)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<logiranidogadjaji>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<logiranidogadjaji>()
                .Property(e => e.ip_adresa)
                .IsUnicode(false);

            modelBuilder.Entity<logiranidogadjaji>()
                .Property(e => e.url)
                .IsUnicode(false);        

            modelBuilder.Entity<nalozi>()
                .HasMany(e => e.geotacke)
                .WithRequired(e => e.nalozi)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<nalozi>()
                .HasMany(e => e.stajalista_nalozi)
                .WithRequired(e => e.nalozi)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<poruke>()
                .Property(e => e.text)
                .IsUnicode(false);

            modelBuilder.Entity<postavke>()
                .Property(e => e.kljuc)
                .IsUnicode(false);

            modelBuilder.Entity<postavke>()
                .Property(e => e.vrijednost)
                .IsUnicode(false);

            modelBuilder.Entity<poznatelokacije>()
                .Property(e => e.duzina)
                .IsUnicode(false);

            modelBuilder.Entity<poznatelokacije>()
                .Property(e => e.sirina)
                .IsUnicode(false);

            modelBuilder.Entity<poznatelokacije>()
                .Property(e => e.naziv)
                .IsUnicode(false);

            modelBuilder.Entity<poznatelokacije>()
                .Property(e => e.opis)
                .IsUnicode(false);

            modelBuilder.Entity<poznatelokacije>()
                .HasMany(e => e.zadaci)
                .WithRequired(e => e.poznatelokacije)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<stajalista>()
                .HasMany(e => e.stajalista_nalozi)
                .WithRequired(e => e.stajalista)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<stajalista>()
                .Property(e => e.naziv)
                .IsUnicode(false);

            modelBuilder.Entity<stajalista>()
                .Property(e => e.opis)
                .IsUnicode(false);

            modelBuilder.Entity<stajalista>()
                .Property(e => e.duzina)
                .IsUnicode(false);

            modelBuilder.Entity<stajalista>()
                .Property(e => e.sirina)
                .IsUnicode(false);          
                    
            modelBuilder.Entity<stanja>()
                .Property(e => e.opis)
                .IsUnicode(false);

            modelBuilder.Entity<stanja>()
                .HasMany(e => e.nalozi)
                .WithRequired(e => e.stanja)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<vozaci>()
                .Property(e => e.user)
                .IsUnicode(false);

            modelBuilder.Entity<vozaci>()
                .Property(e => e.pass)
                .IsUnicode(false);

            modelBuilder.Entity<vozaci>()
                .Property(e => e.ime)
                .IsUnicode(false);

            modelBuilder.Entity<vozaci>()
                .Property(e => e.prezime)
                .IsUnicode(false);

            modelBuilder.Entity<vozaci>()
                .Property(e => e.jmbg)
                .IsUnicode(false);

            modelBuilder.Entity<vozaci>()
                .HasMany(e => e.nalozi)
                .WithRequired(e => e.vozaci)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<vozaci>()
                .HasMany(e => e.poruke)
                .WithRequired(e => e.vozaci)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<vozila>()
                .Property(e => e.naziv)
                .IsUnicode(false);

            modelBuilder.Entity<vozila>()
                .Property(e => e.marka)
                .IsUnicode(false);

            modelBuilder.Entity<vozila>()
                .Property(e => e.tip)
                .IsUnicode(false);

            modelBuilder.Entity<vozila>()
                .Property(e => e.nosivost)
                .IsUnicode(false);

            modelBuilder.Entity<vozila>()
                .HasMany(e => e.nalozi)
                .WithRequired(e => e.vozila)
                .WillCascadeOnDelete(false);
           
            modelBuilder.Entity<zadaci>()
                .Property(e => e.naziv)
                .IsUnicode(false);

            modelBuilder.Entity<zadaci>()
                .Property(e => e.opis)
                .IsUnicode(false);

            modelBuilder.Entity<zadaci>()
                .HasOptional(e => e.nalozi)
                .WithMany(e => e.zadaci)
                .HasForeignKey(e => e.nalog_id);                            
        }
    }
}
