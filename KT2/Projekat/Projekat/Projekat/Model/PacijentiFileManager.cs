/***********************************************************************
 * Module:  NaloziFileManager.cs
 * Author:  Teodora
 * Purpose: Definition of the Class NaloziFileManager
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Windows;
using Projekat;
using Projekat.Model;

namespace Model
{
    public static class PacijentiFileManager
    {
        static List<Pacijent> pacijenti = new List<Pacijent>();

        public static void DodajNalog(Pacijent noviNalog)
        {
            pacijenti.Add(noviNalog);
            PrikaziPacijenta.PacijentiTabela.Add(noviNalog);
        }

        public static void IzmeniNalog(Pacijent nalog)
        {
            //
        }

        public static void ObrisiNalog(Pacijent nalog)
        {
            if (nalog != null)
            {
                for (int i = 0; i < pacijenti.Count; i++)
                {
                    Pacijent p = pacijenti[i];
                    if (p.IdPacijenta == nalog.IdPacijenta)
                    {
                        pacijenti.Remove(nalog);
                        PrikaziPacijenta.PacijentiTabela.Remove(nalog);
                    }
                }
            }
            else 
            {
                MessageBox.Show("Niste selektovali pacijenta za brisanje!");
            }
        }

        public static List<Pacijent> PronadjiSve()
        {
            return pacijenti;
        }

        public static Pacijent PronadjiPoId(int id)
        {
            // TODO: implement
            return null;
        }

        private static string AdresaFajla;
    }
}