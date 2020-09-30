using Matrip.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matrip.Domain.Models.SaleModels
{
    public class PartnerSalesModel
    {
        public ma25partner ma25partner { get; set; }
        public List<ma21saleTrip> ma21saleTripList { get; set; }
    }
}
