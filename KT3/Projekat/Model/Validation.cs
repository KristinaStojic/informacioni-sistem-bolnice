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
            if (value is int)
            {
                int i = (int)value;
                if (PrebaciStaticku.aktivan)
                {
                    if (i > PrebaciStaticku.dozvoljenaKolicina) return new ValidationResult(false, "Morate unijeti manji broj");
                    if (i < 0) return new ValidationResult(false, "Morate unijeti veci broj");
                }
                else if (PrebaciDinamicku.aktivan)
                {
                    if (i > PrebaciDinamicku.dozvoljenaKolicina) return new ValidationResult(false, "Morate unijeti manji broj");
                    if (i < 0) return new ValidationResult(false, "Morate unijeti veci broj");
                }else if (PreraspodjelaDinamicke.aktivna)
                {
                    if (i > PreraspodjelaDinamicke.dozvoljenaKolicina) return new ValidationResult(false, "Morate unijeti manji broj");
                    if (i < 0) return new ValidationResult(false, "Morate unijeti veci broj");
                }else if (PreraspodjelaStaticke.aktivna)
                {
                    if (i > PreraspodjelaStaticke.dozvoljenaKolicina) return new ValidationResult(false, "Morate unijeti manji broj");
                    if (i < 0) return new ValidationResult(false, "Morate unijeti veci broj");
                }else if (SlanjeDinamicke.aktivan)
                {
                    if (i > SlanjeDinamicke.dozvoljenaKolicina) return new ValidationResult(false, "Morate unijeti manji broj");
                    if (i < 0) return new ValidationResult(false, "Morate unijeti veci broj");
                }else if (SlanjeStaticke.aktivan)
                {
                    if (i > SlanjeStaticke.dozvoljenaKolicina) return new ValidationResult(false, "Morate unijeti manji broj");
                    if (i < 0) return new ValidationResult(false, "Morate unijeti veci broj");
                }

                return new ValidationResult(true, "OK");
            }
            else
            {
                return new ValidationResult(false, "Neispravan unos");
            }

        }

    }
}
