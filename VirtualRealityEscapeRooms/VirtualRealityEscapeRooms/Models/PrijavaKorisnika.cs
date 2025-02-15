using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VR_escape_rooms.modeli
{
    public class PrijavaKorisnika
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Prijavio")]
        public int PrijavioId { get; set; }

        [ForeignKey("Prijavljeni")]
        public int PrijavljeniId { get; set; }

        [Required]
        public string Razlog { get; set; } 

        public string Status { get; set; } = "na cekanju";

        public DateTime DatumPrijave { get; set; } = DateTime.Now;

        [JsonIgnore]
        public virtual Korisnik? Prijavio { get; set; } 
        [JsonIgnore]
        public virtual Korisnik? Prijavljeni { get; set; }

        public PrijavaKorisnika(int prijavioId, Korisnik prijavio, int prijavljeniId, Korisnik prijavljeni, string razlog, string status, DateTime datumPrijave)
        {
            PrijavioId = prijavioId;
            Prijavio = prijavio;
            PrijavljeniId = prijavljeniId;
            Prijavljeni = prijavljeni;
            Razlog = razlog;
            Status = status ?? "na cekanju"; 
            DatumPrijave = datumPrijave;
        }

        public PrijavaKorisnika() { }
    }
}
