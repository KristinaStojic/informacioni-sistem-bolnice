using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Model;
using Projekat.Servis;

namespace Projekat.Model
{
    public class ZahteviZaGodisnjiMenadzer
    {
        public List<ZahtevZaGodisnji> zahtevi = new List<ZahtevZaGodisnji>();
        LekariServis lekariServis = new LekariServis();
        public void DodajZahtev(ZahtevZaGodisnji zahtev)
        {
            foreach (Lekar lekar in lekariServis.NadjiSveLekare())
            {
                if (lekar.IdLekara == zahtev.lekar.IdLekara)
                {
                    lekar.ZahteviZaOdmor.Add(zahtev.idZahteva);
                    zahtevi.Add(zahtev);
                    ZahteviZaGodisnjiLekar.TabelaZahteva.Add(zahtev);
                    sacuvajIzmjeneZahteva();
                }
            }
        }

        public List<ZahtevZaGodisnji> NadjiSveZahteve()
        {
            if (File.ReadAllText("zahteviZaOdmor.xml").Trim().Equals(""))
            {
                return this.zahtevi;
            }
            else
            {
                FileStream filestream = File.OpenRead("zahteviZaOdmor.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<ZahtevZaGodisnji>));
                zahtevi = (List<ZahtevZaGodisnji>)serializer.Deserialize(filestream);
                filestream.Close();
                return zahtevi;
            }
        }

        public void sacuvajIzmjeneZahteva()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<ZahtevZaGodisnji>));
            TextWriter filestream = new StreamWriter("zahteviZaOdmor.xml");
            serializer.Serialize(filestream, zahtevi);
            filestream.Close();
        }

    }
}
