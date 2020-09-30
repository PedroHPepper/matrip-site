using Matrip.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matrip.Domain.Models.SaleModels
{
    public class SaleViewModel
    {
        public ma32sale sale { get; set; }
        public ma33UserAddress userAddress { get; set; }
    }
}
