using Projekat.ViewModel;
using System.Globalization;
using System.Windows.Controls;

namespace Projekat.Model
{
    class Validacija : ValidationRule
    {

        public override ValidationResult Validate(object vrijednost, CultureInfo cultureInfo)
        {
            if (jeBroj((string)vrijednost))
            {
                int unesenaVrijednost = int.Parse((string)vrijednost);
                if (SkladisteViewModel.aktivnaStaticka)
                {
                    if (unesenaVrijednost > SkladisteViewModel.dozvoljenaKolicinaStaticke) {  return new ValidationResult(false, "Morate unijeti manji broj"); }
                    if (unesenaVrijednost < 0) return new ValidationResult(false, "Morate unijeti veci broj");
                }
                else if (SkladisteViewModel.aktivnaDinamicka)
                {
                    if (unesenaVrijednost > SkladisteViewModel.dozvoljenaKolicina) return new ValidationResult(false, "Morate unijeti manji broj");
                    if (unesenaVrijednost < 0) return new ValidationResult(false, "Morate unijeti veci broj");
                }
                else if (PreraspodjelaDinamicke.aktivna)
                {
                    if (unesenaVrijednost > PreraspodjelaDinamicke.dozvoljenaKolicina) return new ValidationResult(false, "Morate unijeti manji broj");
                    if (unesenaVrijednost < 0) return new ValidationResult(false, "Morate unijeti veci broj");
                }
                else if (PreraspodjelaStaticke.aktivna)
                {
                    if (unesenaVrijednost > PreraspodjelaStaticke.dozvoljenaKolicina) return new ValidationResult(false, "Morate unijeti manji broj");
                    if (unesenaVrijednost < 0) return new ValidationResult(false, "Morate unijeti veci broj");
                }
                else if (SlanjeDinamicke.aktivan)
                {
                    if (unesenaVrijednost > SlanjeDinamicke.dozvoljenaKolicina) return new ValidationResult(false, "Morate unijeti manji broj");
                    if (unesenaVrijednost < 0) return new ValidationResult(false, "Morate unijeti veci broj");
                }
                else if (SlanjeStaticke.aktivan)
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
        public bool jeBroj(string input)
        {
            int test;
            return int.TryParse(input, out test);
        }
    }
}
