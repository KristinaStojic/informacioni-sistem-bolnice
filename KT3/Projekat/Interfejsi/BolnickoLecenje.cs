using Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Interfejsi
{
    public class BolnickoLecenje : ITipUputa
    {
        public void povecajBrojUputa(Pacijent pacijent)
        {
            PacijentiServis servis = new PacijentiServis();
            foreach (Pacijent p in servis.pacijenti())
            {
                if(pacijent.IdPacijenta == p.IdPacijenta)
                {
                    Console.WriteLine("Bilo je: " + pacijent.Karton.brojBolnickihUputa + " bolnickih uputa");
                    pacijent.Karton.brojBolnickihUputa++;
                    Console.WriteLine("Sada je: " + pacijent.Karton.brojBolnickihUputa + " bolnickih uputa");
                }
            }

           // PacijentiServis.SacuvajIzmenePacijenta();
            
        }

        
    }
}
