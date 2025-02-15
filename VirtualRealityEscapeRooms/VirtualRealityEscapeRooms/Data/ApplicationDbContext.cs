using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VirtualRealityEscapeRooms.Models;
using VR_escape_rooms.modeli;

namespace VirtualRealityEscapeRooms.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Igra> Igre { get; set; }
        public DbSet<Rezervacija> Rezervacije { get; set; }
        public DbSet<Ocjena> Ocjene { get; set; }
        public DbSet<PrijavaKorisnika> PrijaveKorisnika { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Korisnik>().ToTable("Korisnik");
            modelBuilder.Entity<Igra>().ToTable("Igra");
            modelBuilder.Entity<Rezervacija>().ToTable("Rezervacija");
            modelBuilder.Entity<Ocjena>().ToTable("Ocjena");
            modelBuilder.Entity<PrijavaKorisnika>().ToTable("PrijavaKorisnika");

            modelBuilder.Entity<Korisnik>().HasKey(k => k.ID);
            modelBuilder.Entity<Igra>().HasKey(i => i.ID);
            modelBuilder.Entity<Rezervacija>().HasKey(r => r.ID);
            modelBuilder.Entity<Ocjena>().HasKey(o => o.ID);
            modelBuilder.Entity<PrijavaKorisnika>().HasKey(p => p.ID);

            modelBuilder.Entity<Rezervacija>()
                .HasOne(r => r.Korisnik)
                .WithMany(k => k.Rezervacije)
                .HasForeignKey(r => r.KorisnikID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rezervacija>()
                .HasOne(r => r.Igra)
                .WithMany(i => i.Rezervacije)
                .HasForeignKey(r => r.IgraID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ocjena>()
                .HasOne(o => o.Korisnik)
                .WithMany(k => k.Ocijene)
                .HasForeignKey(o => o.KorisnikId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ocjena>()
                .HasOne(o => o.Igra)
                .WithMany(i => i.Ocijene)
                .HasForeignKey(o => o.IgraId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PrijavaKorisnika>()
                .HasOne(p => p.Prijavio)
                .WithMany(k => k.PrijavePoslane)
                .HasForeignKey(p => p.PrijavioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PrijavaKorisnika>()
                .HasOne(p => p.Prijavljeni)
                .WithMany(k => k.PrijavePrimljene)
                .HasForeignKey(p => p.PrijavljeniId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Korisnik>()
                .Property(k => k.TipKorisnika)
                .HasConversion<string>();

            modelBuilder.Entity<Igra>()
                .Property(i => i.Tezina)
                .HasConversion<string>();

            modelBuilder.Entity<Rezervacija>()
                .Property(r => r.Status)
                .HasConversion<string>();

            modelBuilder.Entity<PrijavaKorisnika>()
                .Property(p => p.Status)
                .HasConversion<string>();

             modelBuilder.Entity<Igra>()
        .Property(i => i.Zanr)
        .HasConversion(
            v => v.ToString(),
            v => (Zanr)Enum.Parse(typeof(Zanr), v)
        );
        }
    }
}
