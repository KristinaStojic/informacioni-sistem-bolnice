using Projekat.Servis;

namespace Projekat.Model
{
    public class OpremaMenadzer : Menadzer<Oprema>
    {
        public override void Izmjeni(Oprema element, Oprema element1)
        {
            foreach (Oprema oprema in lista)
            {
                if (oprema.IdOpreme == element.IdOpreme)
                {
                    oprema.NazivOpreme = element1.NazivOpreme;
                    oprema.Kolicina = element1.Kolicina;
                }
            }
            SkladisteServis.azurirajOpremuUSkladistu();
            sacuvajIzmjene("oprema.xml");
        }
      
    }
}
