/***********************************************************************
 * Module:  Lekar.cs
 * Author:  Kristina
 * Purpose: Definition of the Class Lekar
 ***********************************************************************/

using System;
using System.Collections.Generic;
public enum Specijalizacija { Opsta_praksa, Specijalista, Ginekologija, Hirurgija, Ortopedija}
namespace Model
{
   public class Lekar
   {
      public int IdLekara { get; set; }
      public string ImeLek { get; set; }
      public string PrezimeLek { get; set; }
      public Specijalizacija specijalizacija { get; set; }

    
    public Lekar(int id, string ime, string prz)
        {
            this.IdLekara = id;
            this.ImeLek = ime;
            this.PrezimeLek = prz;
        }

    public Lekar(int id)
        {
            this.IdLekara = id;
        }
    public Lekar() { }

        // Sanja
    public Lekar(int id, string ime, string prz, Specijalizacija specijalizacija)
    {
            this.ImeLek = ime;
            this.PrezimeLek = prz;
            this.specijalizacija = specijalizacija;
    }
    public override string ToString()
    {
        if(specijalizacija.Equals(Specijalizacija.Opsta_praksa))
            return this.ImeLek + " " + this.PrezimeLek +  " - opsta praksa";
        else
            return this.ImeLek + " " + this.PrezimeLek + " - specijalista";
        }

    }
    

}