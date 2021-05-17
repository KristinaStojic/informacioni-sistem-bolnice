using Model;
using Projekat.Servis;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Projekat.Model
{
    class OpremaMenadzer
    {

        public static void izmjeniOpremu(Oprema izOpreme, Oprema uOpremu)
        {
            azurirajOpremuUSkladistu();
            sacuvajIzmjene();
        }

        private static void azurirajOpremuUSkladistu()
        {
            foreach (Sala sala in SaleServis.Sale())
            {
                if (sala.Namjena.Equals("Skladiste"))
                {
                    sala.Oprema = OpremaMenadzer.oprema;
                }
            }
        }

        public static List<Oprema> NadjiSvuOpremu()
        {
            if (File.ReadAllText("oprema.xml").Trim().Equals(""))
            {
                return oprema;
            }
            else
            {
                ucitajOpremuIzFajla();
                return oprema;
            }
        }

        private static void ucitajOpremuIzFajla()
        {
            FileStream filestream = File.OpenRead("oprema.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(List<Oprema>));
            oprema = (List<Oprema>)serializer.Deserialize(filestream);
            filestream.Close();
            azurirajOpremuUSkladistu();
        }

        public static void DodajOpremu(Oprema oprema)
        {
            OpremaMenadzer.oprema.Add(oprema);
            azurirajOpremuUSkladistu();
            sacuvajIzmjene();
        }

        public static void sacuvajIzmjene()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Oprema>));
            TextWriter filestream = new StreamWriter("oprema.xml");
            serializer.Serialize(filestream, oprema);
            filestream.Close();
        }

        public static int GenerisanjeIdOpreme()
        {
            int id;
            for (id = 1; id <= oprema.Count; id++)
            {
                if (!postojiIdOpreme(id))
                {
                    return id;
                }
            }
            return id;
        }

        public static int GenerisanjeIdKreveta()
        {
            int id;
            for (id = 1; id <= kreveti.Count; id++)
            {
                if (!postojiIdKreveta(id))
                {
                    return id;
                }
            }
            return id;
        }

        private static bool postojiIdOpreme(int id)
        {
            foreach (Oprema o in oprema)
            {
                if (o.IdOpreme.Equals(id))
                {
                    return true;
                }
            }
            return false;
        }
        private static bool postojiIdKreveta(int id)
        {
            foreach (Krevet o in kreveti)
            {
                if (o.IdKreveta.Equals(id))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<Oprema> oprema = new List<Oprema>();
        /*Kristina*/
        public static List<Krevet> kreveti = new List<Krevet>();

      
    }
}
