using System.Globalization;
using System.Windows.Controls;

namespace Projekat.Model
{
    class Validacija : ValidationRule
    {

        public override ValidationResult Validate(object vrijednost, CultureInfo cultureInfo)
        {
            if (vrijednost is int)
            {
                int unesenaVrijednost = (int)vrijednost;
                if (PrebaciStaticku.aktivan)
                {
                    if (unesenaVrijednost > PrebaciStaticku.dozvoljenaKolicina) return new ValidationResult(false, "Morate unijeti manji broj");
                    if (unesenaVrijednost < 0) return new ValidationResult(false, "Morate unijeti veci broj");
                }
                else if (PrebaciDinamicku.aktivan)
                {
                    if (unesenaVrijednost > PrebaciDinamicku.dozvoljenaKolicina) return new ValidationResult(false, "Morate unijeti manji broj");
                    if (unesenaVrijednost < 0) return new ValidationResult(false, "Morate unijeti veci broj");
                }else if (PreraspodjelaDinamicke.aktivna)
                {
                    if (unesenaVrijednost > PreraspodjelaDinamicke.dozvoljenaKolicina) return new ValidationResult(false, "Morate unijeti manji broj");
                    if (unesenaVrijednost < 0) return new ValidationResult(false, "Morate unijeti veci broj");
                }else if (PreraspodjelaStaticke.aktivna)
                {
                    if (unesenaVrijednost > PreraspodjelaStaticke.dozvoljenaKolicina) return new ValidationResult(false, "Morate unijeti manji broj");
                    if (unesenaVrijednost < 0) return new ValidationResult(false, "Morate unijeti veci broj");
                }else if (SlanjeDinamicke.aktivan)
                {
                    if (unesenaVrijednost > SlanjeDinamicke.dozvoljenaKolicina) return new ValidationResult(false, "Morate unijeti manji broj");
                    if (unesenaVrijednost < 0) return new ValidationResult(false, "Morate unijeti veci broj");
                }else if (SlanjeStaticke.aktivan)
                {
                    if (unesenaVrijednost > SlanjeStaticke.dozvoljenaKolicina) return new ValidationResult(false, "Morate unijeti manji broj");
                    if (unesenaVrijednost < 0) return new ValidationResult(false, "Morate unijeti veci broj");
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
