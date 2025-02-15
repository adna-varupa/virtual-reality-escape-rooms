using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VR_escape_rooms.modeli
{
    public class Rezervacija
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int KorisnikID { get; set; }

        [JsonIgnore] 
        public Korisnik? Korisnik { get; set; }

        [Required]
        public int IgraID { get; set; }

        [JsonIgnore]
        public Igra? Igra { get; set; }

        [Required]
        public DateTime DatumRezervacije { get; set; }

        [Required]
        public string Status { get; set; }

public Rezervacija() { }

        public Rezervacija(int korisnikID, int igraID, DateTime datumRezervacije, string status)
        {
            KorisnikID = korisnikID;
            IgraID = igraID;
            DatumRezervacije = datumRezervacije;
            Status = status;
        }
    }
}
