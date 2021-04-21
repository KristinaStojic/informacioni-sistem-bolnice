using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public class AnketaMenadzer
    {
        public static List<Anketa> ankete = new List<Anketa>();

        public static void DodajAnketu(Anketa novaAnketa)
        {
            ankete.Add(novaAnketa);
        }




    }
}
