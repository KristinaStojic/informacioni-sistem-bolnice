using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Model
{
    public class ValidacijaSekretar : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                long r;
                if (long.TryParse(s, out r))
                {
                    return new ValidationResult(true, null);
                }
                return new ValidationResult(false, "Potrebno je samo unositi brojeve!");
            }
            catch
            {
                return new ValidationResult(false, "Unknown error occured.");
            }
        }
    }

    public class MinMaxValidationRule : ValidationRule
    {
        public int Min
        {
            get;
            set;
        }

        public int Max
        {
            get;
            set;
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                long r;

                if (long.TryParse(s, out r))
                {
                    if (r.ToString().Length < Min || r.ToString().Length > Max)
                    {
                        return new ValidationResult(false, "Potrebno je uneti " + Convert.ToString(Min) + " - " + Convert.ToString(Max) + " cifara");
                    }
                    else
                    {
                        return new ValidationResult(true, "Greska");
                    }
                }
                else
                {
                    return new ValidationResult(false, "Unknown error occured.");

                }
            }
            catch
            {
                return new ValidationResult(false, "Unknown error occured.");

            }
        }
    }

}

