using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    interface ISerijalizacija<T>
    {
        void SacuvajIzmene(string fajl, List<T> lista);
        List<T> NadjiSve(string fajl);
    }
}
