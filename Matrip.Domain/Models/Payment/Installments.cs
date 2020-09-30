namespace Matrip.Domain.Models.Payment
{
    public class Installments
    {
        public int InstallmentsNumber { get; set; }
        public decimal Value { get; set; }
        public decimal ValueByInstallments { get; set; }
        public bool interest { get; set; }
    }
}
