using Model;
using Projekat.Model;

namespace Projekat.Servis
{
    public class IzmjenaKolicineStatickeServis
    {
        public static void PovecajKolicinuOpreme(Sala sala, int kolicina, Oprema izabranaOprema)
        {
            foreach (Oprema oprema in sala.Oprema)
            {
                if (oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    oprema.Kolicina += kolicina;
                }
            }
        }

        public static void SmanjiKolicinuOpreme(Sala sala, int kolicina, Oprema izabranaOprema)
        {
            foreach (Oprema oprema in sala.Oprema.ToArray())
            {
                if (oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    if (oprema.Kolicina - kolicina == 0)
                    {
                        sala.Oprema.Remove(oprema);
                    }else { 
                        oprema.Kolicina -= kolicina;
                    }
                }
            }
        }
    }
}
