namespace Matrip.Domain.Models.TripPurchase
{
    public class TouristDiscountByAgeModel
    {
        public int AgeDiscountID { get; set; }
        public string AgeDiscountName { get; set; }
        public int TouristQuantity { get; set; }
        public int minAge { get; set; }
        public int maxAge { get; set; }
    }
}
