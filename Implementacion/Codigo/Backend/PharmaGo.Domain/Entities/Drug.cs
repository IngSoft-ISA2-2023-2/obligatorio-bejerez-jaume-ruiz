using PharmaGo.Exceptions;

namespace PharmaGo.Domain.Entities
{
    public class Drug : PharmacyItem
    {
        public string Symptom { get; set; }
        public int Quantity { get; set; }
        public int Stock { get; set; } = 0;
        public bool Prescription { get; set; }
        public UnitMeasure UnitMeasure { get; set; }
        public Presentation Presentation { get; set; }

        public void ValidOrFail()
        {
            if (string.IsNullOrEmpty(Code) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Symptom)
                    || Quantity <= 0 || Price <= 0 || Stock < 0
                    || UnitMeasure == null || Presentation == null || Pharmacy == null)
            {
                throw new InvalidResourceException("The Drug is not correctly created.");
            }
        }
    }
}