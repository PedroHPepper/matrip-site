using System.Collections.Generic;

namespace Matrip.Domain.Models.TripPurchase
{
    public class TripItem
    {
        public int TripID { get; set; }
        public List<int> GuideID { get; set; }
        public int SubTripID { get; set; }
        public List<ServiceQuantity> Services { get; set; }
        public string SubtripStatus { get; set; }
        public List<TouristDiscountByAgeModel> TouristDiscountByAgeList { get; set; }
        public double TotalValue { get; set; }
    }
}
