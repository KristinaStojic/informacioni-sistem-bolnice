using System.Collections.Generic;
using Model;
using Projekat.Model;

namespace Projekat.Servis
{
    public class PacijentiServis
    {
        #region Pacijent Menadzer
        public static void DodajNalog(Pacijent noviNalog)
        {
            PacijentiMenadzer.DodajNalog(noviNalog);
        }

        public static void IzmeniNalog(Pacijent stariNalog, Pacijent noviNalog)
        {
            PacijentiMenadzer.IzmeniNalog(stariNalog, noviNalog);
        }

        // Sanja
        public static void IzmeniNalogPacijent(Pacijent stari, Pacijent nalog)
        {
            PacijentiMenadzer.IzmeniNalogPacijent(stari, nalog);
        }

        public static void ObrisiNalog(Pacijent nalog)
        {
            PacijentiMenadzer.ObrisiNalog(nalog);
        }

        public static List<Pacijent> PronadjiSve()
        {
            return PacijentiMenadzer.PronadjiSve();
        }
        
        public static Pacijent PronadjiPoId(int id)
        {
            return PacijentiMenadzer.PronadjiPoId(id);
        }

        public static int GenerisanjeIdPacijenta()
        {
            return PacijentiMenadzer.GenerisanjeIdPacijenta();
        }

        public static void SacuvajIzmenePacijenta()
        {
            PacijentiMenadzer.SacuvajIzmenePacijenta();
        }

        public static bool JedinstvenJmbg(long jmbg)
        {
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                if (p.Jmbg == jmbg)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Pacijent 
        public static bracnoStanje OdrediBracnoStanjePacijenta(pol polPacijenta, string txtBracnoStanje)
        {
            bracnoStanje BracnoStanje = bracnoStanje.Neodredjeno;
            if (txtBracnoStanje.Equals("Oženjen/Udata") || txtBracnoStanje.Equals("Married"))
            {
                if (polPacijenta.Equals("M"))
                {
                    BracnoStanje = bracnoStanje.Ozenjen;
                    return BracnoStanje;
                }
                else
                {
                    BracnoStanje = bracnoStanje.Udata;
                    return BracnoStanje;
                }
            }
            else if (txtBracnoStanje.Equals("Neoženjen/Neudata") || txtBracnoStanje.Equals("Unmarried"))
            {
                if (polPacijenta.Equals("M"))
                {
                    BracnoStanje = bracnoStanje.Neozenjen;
                    return BracnoStanje;
                }
                else
                {
                    BracnoStanje = bracnoStanje.Neudata;
                    return BracnoStanje;
                }
            }
            else if (txtBracnoStanje.Equals("Udovac/Udovica") || txtBracnoStanje.Equals("Widow"))
            {
                if (polPacijenta.Equals("M"))
                {
                    BracnoStanje = bracnoStanje.Udovac;
                    return BracnoStanje;
                }
                else
                {
                    BracnoStanje = bracnoStanje.Udovica;
                    return BracnoStanje;
                }
            }
            else if (txtBracnoStanje.Equals("Razveden/Razvedena") || txtBracnoStanje.Equals("Divorced"))
            {
                if (polPacijenta.Equals("M"))
                {
                    BracnoStanje = bracnoStanje.Razveden;
                    return BracnoStanje;
                }
                else
                {
                    BracnoStanje = bracnoStanje.Razvedena;
                    return BracnoStanje;
                }
            }
            else if (txtBracnoStanje.Equals("Neodređeno") || txtBracnoStanje.Equals("Indefinitely"))
            {
                BracnoStanje = bracnoStanje.Neodredjeno;
                return BracnoStanje;
            }
            return BracnoStanje;
        }

        public static string OdrediPolPacijenta(Pacijent prijavljeniPacijent)
        {
            string txtPol;
            if (prijavljeniPacijent.Pol.ToString().Equals("M"))
                txtPol = "M";
            else
                txtPol = "Z";
            return txtPol;
        }

        public static pol IzmeniPolPacijenta(string txtPol)
        {
            pol retVal;
            if (txtPol.Equals("M"))
                retVal = pol.M;
            else
                retVal = pol.Z;
            return retVal;
        }
        #endregion

        #region Metode za popunjavanje polja sa dijaloga - Sekretar
        public static pol OdreditiPolPacijenta(string combo)
        {
            pol pol;
            if (combo.Equals("M"))
            {
                pol = pol.M;
            }
            else
            {
                pol = pol.Z;
            }

            return pol;
        }

        public static statusNaloga OdrediStatusNaloga(string combo)
        {
            statusNaloga status = statusNaloga.Stalni;
            if (combo.Equals("STALAN"))
            {
                status = statusNaloga.Stalni;
            }
            else
            {
                status = statusNaloga.Guest;
            }

            return status;
        }

        public static long OdrediJmbgStaratelja(string jmbgStaratelja)
        {
            long staratelj = 0;
            if (jmbgStaratelja.Equals(""))
            {
                staratelj = 0;
            }
            else
            {
                staratelj = long.Parse(jmbgStaratelja);
            }

            return staratelj;
        }

        public static bool MaloletnoLice(bool oznaceno)
        {
            bool maloletnik = false;
            if (oznaceno)
            {
                maloletnik = true;
            }
            else
            {
                maloletnik = false;
            }

            return maloletnik;
        }

        public static bracnoStanje OdreditiBracnoStanje(int selectedIndex, string polPacijenta)
        {
            bracnoStanje brStanje = bracnoStanje.Neudata;
            if (selectedIndex == 0 && polPacijenta.Equals("Z"))
            {
                brStanje = bracnoStanje.Neudata;
            }
            else if (selectedIndex == 1 && polPacijenta.Equals("Z"))
            {
                brStanje = bracnoStanje.Udata;
            }
            else if (selectedIndex == 2 && polPacijenta.Equals("Z"))
            {
                brStanje = bracnoStanje.Udovica;
            }
            else if (selectedIndex == 3 && polPacijenta.Equals("Z"))
            {
                brStanje = bracnoStanje.Razvedena;
            }
            else if (selectedIndex == 0 && polPacijenta.Equals("M"))
            {
                brStanje = bracnoStanje.Neozenjen;
            }
            else if (selectedIndex == 1 && polPacijenta.Equals("M"))
            {
                brStanje = bracnoStanje.Ozenjen;
            }
            else if (selectedIndex == 2 && polPacijenta.Equals("M"))
            {
                brStanje = bracnoStanje.Udovac;
            }
            else if (selectedIndex == 3 && polPacijenta.Equals("M"))
            {
                brStanje = bracnoStanje.Razveden;
            }

            return brStanje;
        }

        public static int UcitajIndeksPola(Pacijent izabraniNalog)
        {
            int polPacijenta = 0;
            if (izabraniNalog.Pol.Equals(pol.M))
            {
                polPacijenta = 0;
            }
            else if (izabraniNalog.Pol.Equals(pol.Z))
            {
                polPacijenta = 1;
            }

            return polPacijenta;
        }

        public static int UcitajIndeksBracnogStanja(Pacijent izabraniNalog)
        {
            int bracnoStanjePacijenta = 0;
            if (izabraniNalog.BracnoStanje.Equals(bracnoStanje.Neozenjen) || izabraniNalog.BracnoStanje.Equals(bracnoStanje.Neudata))
            {
                bracnoStanjePacijenta = 0;
            }
            else if (izabraniNalog.BracnoStanje.Equals(bracnoStanje.Ozenjen) || izabraniNalog.BracnoStanje.Equals(bracnoStanje.Udata))
            {
                bracnoStanjePacijenta = 1;
            }
            else if (izabraniNalog.BracnoStanje.Equals(bracnoStanje.Udovac) || izabraniNalog.BracnoStanje.Equals(bracnoStanje.Udovica))
            {
                bracnoStanjePacijenta = 2;
            }
            else if (izabraniNalog.BracnoStanje.Equals(bracnoStanje.Razveden) || izabraniNalog.BracnoStanje.Equals(bracnoStanje.Razvedena))
            {
                bracnoStanjePacijenta = 3;
            }

            return bracnoStanjePacijenta;
        }

        public static int UcitajIndeksStatusaNaloga(Pacijent izabraniNalog)
        {
            int statusPacijenta = 0;
            if (izabraniNalog.StatusNaloga.Equals(statusNaloga.Stalni))
            {
                statusPacijenta = 0;
            }
            else if (izabraniNalog.StatusNaloga.Equals(statusNaloga.Guest))
            {
                statusPacijenta = 1;
            }

            return statusPacijenta;
        }

        #endregion

        public static List<Pacijent> pacijenti()
        {
            return PacijentiMenadzer.pacijenti;
        }
    }
}
