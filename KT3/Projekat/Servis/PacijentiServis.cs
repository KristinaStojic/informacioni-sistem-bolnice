using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace Projekat.Servis
{
    public class PacijentiServis
    {
        #region Pacijent 
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
            if (polPacijenta.Equals("M"))
            {
                if (txtBracnoStanje.Equals("Ozenjen"))
                {
                    BracnoStanje = bracnoStanje.Ozenjen;
                    return BracnoStanje;
                }
                else if (txtBracnoStanje.Equals("Neoznjen"))
                {
                    BracnoStanje = bracnoStanje.Neozenjen;
                }
                else if (txtBracnoStanje.Equals("Udovac"))
                {
                    BracnoStanje = bracnoStanje.Udovac;
                }
                else if (txtBracnoStanje.Equals("Razveden"))
                {
                    BracnoStanje = bracnoStanje.Razveden;
                }
                else if (txtBracnoStanje.Equals("Neodredjeno"))
                {
                    BracnoStanje = bracnoStanje.Neodredjeno;
                }
            }
            else
            {
                if (txtBracnoStanje.Equals("Udata"))
                {
                    BracnoStanje = bracnoStanje.Udata;
                }
                else if (txtBracnoStanje.Equals("Neudata"))
                {
                    BracnoStanje = bracnoStanje.Neudata;
                }
                else if (txtBracnoStanje.Equals("Udovica"))
                {
                    BracnoStanje = bracnoStanje.Udovica;
                }
                else if (txtBracnoStanje.Equals("Razvedena"))
                {
                    BracnoStanje = bracnoStanje.Razvedena;
                }
                else if (txtBracnoStanje.Equals("Neodredjeno"))
                {
                    BracnoStanje = bracnoStanje.Neodredjeno;
                }
            }
            return BracnoStanje;
        }

        public static string OdrediPolPacijenta(Pacijent prijavljeniPacijent)
        {
            string txtPol;
            if (prijavljeniPacijent.Pol.Equals("M"))
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
    }
}
