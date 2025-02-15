using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR_escape_rooms.modeli;

namespace VR_escape_rooms.modeli
{
    public class Igra
    {
        public int ID { get; set; }
        public string Naziv { get; set; }
        public Tezina Tezina { get; set; } // 'lako', 'srednje', 'tesko'
        public int Trajanje { get; set; }
        public string Opis { get; set; }
        public Zanr Zanr { get; set; }

        public string? SlikaPath { get; set; }


        [InverseProperty("Igra")]
        public ICollection<Rezervacija> Rezervacije { get; set; }
        public ICollection<Ocjena> Ocijene { get; set; } = new List<Ocjena>();


        public Igra()
        {
            Rezervacije = new List<Rezervacija>();
        }
    }
}
