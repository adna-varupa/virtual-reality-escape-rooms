using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace VR_escape_rooms.modeli
{
    public class Ocjena
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Korisnik")]
        public int KorisnikId { get; set; }

        [JsonIgnore]
        public virtual Korisnik? Korisnik { get; set; }

        [ForeignKey("Igra")]
        public int IgraId { get; set; }
        [JsonIgnore]
        public virtual Igra? Igra { get; set; }

        [Range(1, 5)]
        public int Ocijena { get; set; }  

        public string Komentar { get; set; }
    }
}
