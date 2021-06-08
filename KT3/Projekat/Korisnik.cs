using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projekat.Model;

namespace Projekat
{
    public class Korisnik: IObserver
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public long Jmbg { get; set; }
        public long BrojTelefona { get; set; }
        public string Email { get; set; }
        public string AdresaStanovanja { get; set; }

        public List<Obavestenja> obavestenjaKorisnika { get; set; } = new List<Obavestenja>();

        public Korisnik() {}

        public void Update(ISubject subject)
        {
            ObjavaObavestenja o = subject as ObjavaObavestenja;
            if (o != null)
            {
                obavestenjaKorisnika.Add(o.NovoObavestenje);
                Console.WriteLine("dodato obavestenje");
            }
        }
    }
}
