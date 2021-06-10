using Projekat.Servis;

namespace Projekat.Model
{
    public class OpremaMenadzer : Menadzer<Oprema>
    {
        private OpremaMenadzer() { }
        
        private static OpremaMenadzer instanca = null;
        public static OpremaMenadzer Instanca {
            get { 
                if(instanca == null) { 
                    instanca = new OpremaMenadzer(); 
                }
                return instanca; 
            }
        }

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
