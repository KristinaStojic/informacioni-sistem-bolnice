using Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Interfejsi
{
    public class Laboratorija : ITipUputa
    {
        public void povecajBrojUputa(Pacijent pacijent)
        {
            PacijentiServis servis = new PacijentiServis();
            foreach (Pacijent p in servis.pacijenti())
            {
                if (pacijent.IdPacijenta == p.IdPacijenta)
                {
                    pacijent.Karton.brojLaboratorijskihUputa++;
                }
            }

           // PacijentiServis.SacuvajIzmenePacijenta();

           
        }

        
    }
}
