using Matrip.Domain.Models.Entities;
using System.Collections.Generic;

namespace Matrip.Domain.Models.TripModel
{
    public class TripModel
    {
        public ma05trip ma05trip { get; set; }
        public List<ma27AgeDiscount> ma27AgeDiscountList { get; set; }
        public List<SubtripModel> SubtripModelList { get; set; }
        public string tripCityName { get; set; }

        public string tripUF { get; set; }
    }
    public class SubtripModel
    {
        public ma14subtrip ma14subtrip { get; set; }
        public List<ma16subtripschedule> ma16subtripscheduleList { get; set; }
        public List<ma17SubtripValue> ma17SubtripValueList { get; set; }
        public List<ma11service> ma11serviceList { get; set; }
        public string PartnerName { get; set; }
    }
}
