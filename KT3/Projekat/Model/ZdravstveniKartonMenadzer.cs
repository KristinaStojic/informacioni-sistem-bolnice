using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Projekat.Model
{
    class ZdravstveniKartonMenadzer
    {
        public static List<ZdravstveniKarton> kartoni = new List<ZdravstveniKarton>();
        public static List<LekarskiRecept> recepti = new List<LekarskiRecept>();

        public static List<ZdravstveniKarton> NadjiSveKartone()
        {
            /*if (File.ReadAllText("kartoni.xml").Trim().Equals(""))
            {
                return kartoni;
            }
            else
            {
                FileStream fileStream = File.OpenRead("kartoni.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<Termin>));
                kartoni = (List<ZdravstveniKarton>)serializer.Deserialize(fileStream);
                fileStream.Close();
                return kartoni;
            }
            */
            Console.WriteLine(kartoni.Count + " DODATO U TABELU");
            return kartoni;
        }


        public static int GenerisanjeIdRecepta()
        {
            bool pomocna = false;
            int id = 1;

            for (id = 1; id <= recepti.Count; id++)
            {
                foreach (LekarskiRecept p in recepti)
                {
                    if (p.IdRecepta.Equals(id))
                    {
                        pomocna = true;
                        break;
                    }
                }

                if (!pomocna)
                {
                    return id;
                }
                pomocna = false;
            }

            return id;
        }

        public static void DodajRecept(LekarskiRecept recept) /*TO DO: PROMIJENITI OVO KAD SE DODA FAJL SA KARTONIMA*/
        {
            recepti.Add(recept);
            kartoni.Add(new ZdravstveniKarton(1));
            kartoni.Add(new ZdravstveniKarton(3));
            foreach (ZdravstveniKarton karton in kartoni)
            {
                Console.WriteLine(karton.idPacijenta);
                if(karton.idPacijenta == recept.idPacijenta)
                {
                        List<LekarskiRecept> lr = new List<LekarskiRecept>();
                        karton.LekarskiRecepti = lr;
                        karton.LekarskiRecepti.Add(recept);    
                        TabelaRecepata.PrikazRecepata.Add(recept);

                }
            }
        }
    }
}
