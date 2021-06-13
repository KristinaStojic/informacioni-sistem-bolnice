using Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Interfejsi
{
    public class SpecijalistickiPregled : ITipUputa
    {

        public void povecajBrojUputa(Pacijent pacijent)
        {

            foreach (Pacijent p in PacijentiServis.pacijenti())
            {
                if (pacijent.IdPacijenta == p.IdPacijenta)
                {
                    p.Karton.brojSpecijalistickihUputa++;
                }
            }

            //PacijentiServis.SacuvajIzmenePacijenta();

            
        }

        
    }
}
