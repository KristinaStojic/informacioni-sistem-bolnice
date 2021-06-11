using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Projekat.Model
{
    interface ISerialization<T>
    { 
        void SacuvajIzmene(string fajl, List<T> lista);
        List<T> NadjiSve(string fajl);
    }
}
