using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Projekat.Model
{
    public class ObavestenjaMenadzer : JSONSerialization<Obavestenja>
    {
        public override void Dodaj(Obavestenja element, string lokacijaFajla)
        {
            List<Obavestenja> lista = NadjiSve(lokacijaFajla);
            lista.Add(element);
            if (OglasnaTabla.oglasnaTabla == null)
            {
                OglasnaTabla.oglasnaTabla = new ObservableCollection<Obavestenja>();
            }

            OglasnaTabla.oglasnaTabla.Add(element);
            SacuvajIzmene(lokacijaFajla, lista);
        }

        public override void Izmeni(Obavestenja staroObavestenje, Obavestenja novoObavestenje, string lokacijaFajla)
        {
            foreach (Obavestenja obavestenje in NadjiSve(lokacijaFajla))
            {
                if (obavestenje.IdObavestenja == staroObavestenje.IdObavestenja)
                {
                    obavestenje.TipObavestenja = novoObavestenje.TipObavestenja;
                    obavestenje.Datum = novoObavestenje.Datum;
                    obavestenje.IdLekara = novoObavestenje.IdLekara;
                    obavestenje.ListaIdPacijenata = novoObavestenje.ListaIdPacijenata;
                    obavestenje.SadrzajObavestenja = novoObavestenje.SadrzajObavestenja;
                    obavestenje.Oznaka = novoObavestenje.Oznaka;
                    obavestenje.Notifikacija = novoObavestenje.Notifikacija;

                    int idx = OglasnaTabla.oglasnaTabla.IndexOf(staroObavestenje);
                    OglasnaTabla.oglasnaTabla.RemoveAt(idx);
                    OglasnaTabla.oglasnaTabla.Insert(idx, obavestenje);
                }
            }

            SacuvajIzmene(lokacijaFajla, OglasnaTabla.oglasnaTabla.ToList());
        }

        public override void Obrisi(Obavestenja element, string lokacijaFajla)
        {
            List<Obavestenja> lista = NadjiSve(lokacijaFajla);

            for (int i = 0; i < lista.Count; i++)
            {
                if (element.IdObavestenja == lista[i].IdObavestenja)
                {
                    lista.RemoveAt(i);
                    i--;
                }
            }
            if (OglasnaTabla.oglasnaTabla == null)
            {
                OglasnaTabla.oglasnaTabla = new ObservableCollection<Obavestenja>();
            }

            OglasnaTabla.oglasnaTabla.Remove(element);
            SacuvajIzmene(lokacijaFajla, lista);
        }
    }
}
