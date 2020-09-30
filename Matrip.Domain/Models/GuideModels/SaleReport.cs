using Matrip.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matrip.Domain.Models.GuideModels
{
    public class SaleReport
    {
        public ma04guide ma04guide { get; set; }
        public List<ma25partner> ma25partnerList { get; set; }
        public List<ma22subtripsale> ma22subtripsaleList { get; set; }
    }
}
