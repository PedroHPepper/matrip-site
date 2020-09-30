using Matrip.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matrip.Domain.Models.PartnerModels
{
    public class PartnerModel
    {
        public string UF { get; set; }
        public string CityName { get; set; }
        public ma25partner ma25partner { get; set; }
    }
}
