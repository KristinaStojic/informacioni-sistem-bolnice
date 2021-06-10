using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Projekat.Servis
{
    public interface IAnketeServis
    {
        void sacuvajIzmene();
        List<Anketa> Ankete(); 
        List<Anketa> NadjiSveAnkete();
        Anketa NadjiAnketuPoId(int IdAnkete);
        void ObrisiAnketu(int idTerminaZaBrisanje);
        List<Anketa> SveAnketePacijenta(int idPacijent);
        ObservableCollection<Anketa> PrikaziSveAnketeZaProsleTermine(ObservableCollection<Anketa> AnketePacijenta, int idPacijent);

       /* void DodajAnketuZaLekara(Termin termin, int idPacijent);
        void DodajAnketuZaKliniku(int idPacijent);
        void IzmeniAnketuZaLekara(Termin stariTermin, Termin noviTermin);
        Anketa PronadjiAnketuZaKliniku(int idPacijent);*/
    }
}
