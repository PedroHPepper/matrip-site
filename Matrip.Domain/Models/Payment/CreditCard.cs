namespace Matrip.Domain.Models.Payment
{
    public class CreditCard
    {
        //Informações necessárias para pagamento usado na classe do Pagar.me
        public string CardID { get; set; }
        public string CardNumber { get; set; }
        public string CardName { get; set; }
        public string ExpirationYear { get; set; }
        public string ExpirationMonth { get; set; }
        public string SecurityNumber { get; set; }
        public int InstallmentsNumber { get; set; }
        public double Value { get; set; }
    }
}
