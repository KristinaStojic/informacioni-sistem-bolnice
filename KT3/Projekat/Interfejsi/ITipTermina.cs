using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Interfejsi
{
    public interface ITipTermina
    {
        Sala pronadjiPrvuSlobodnuProstoriju(String selektovaniDatum, String selektovaniSlot);
    }
}
