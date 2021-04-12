using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Projekat.Model
{
    class Validation : ValidationRule
    {
        public double max { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is double)
            {
                double i = (double)value;
                if (i > max) return new ValidationResult(false, "Unesena vrijednost je prevelika");
                return new ValidationResult(true, "OK");
            }
            else
            {
                return new ValidationResult(false, "Neispravan unos");
            }

        }

    }
}
