using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtualRealityEscapeRooms.Models;

namespace VR_escape_rooms.modeli
{
    public class Korisnik
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(50)]
        public string Ime { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string TipKorisnika { get; set; }

        public int HighScore { get; set; } = 0;

        public int MaxScore { get; set; } = 0;

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [InverseProperty("Korisnik")]
        public ICollection<Rezervacija> Rezervacije { get; set; }
        public ICollection<Ocjena> Ocijene { get; set; } = new List<Ocjena>();
        public ICollection<PrijavaKorisnika> PrijavePoslane { get; set; } = new List<PrijavaKorisnika>();
        public ICollection<PrijavaKorisnika> PrijavePrimljene { get; set; } = new List<PrijavaKorisnika>();

        public Korisnik()
        {
            Rezervacije = new List<Rezervacija>();
        }

        public Korisnik(int id, string ime, string email, string lozinka, string tipKorisnika, ICollection<Rezervacija> rezervacije = null)
        {
            ID = id;
            Ime = ime;
            Email = email;
            TipKorisnika = tipKorisnika;
            Rezervacije = rezervacije ?? new List<Rezervacija>(); 
        }
    }
}
