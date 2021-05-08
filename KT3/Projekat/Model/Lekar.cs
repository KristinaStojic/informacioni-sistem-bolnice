/***********************************************************************
 * Module:  Lekar.cs
 * Author:  Kristina
 * Purpose: Definition of the Class Lekar
 ***********************************************************************/

using System;
using System.Collections.Generic;

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
            return this.ImeLek + " " + this.PrezimeLek;


        }
    }

}