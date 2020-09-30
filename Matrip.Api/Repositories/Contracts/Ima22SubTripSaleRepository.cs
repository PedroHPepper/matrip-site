using Matrip.Domain.Models.Entities;
using System;
using System.Collections.Generic;


namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima22SubTripSaleRepository : IBaseRepository<ma22subtripsale>
    {
        List<ma22subtripsale> GetSubtripSaleList(ma16subtripschedule ma16Subtripschedule, DateTime Date, int ValueID);
        List<ma22subtripsale> GetSubtripReport(ma14subtrip subtrip, DateTime initialDate, DateTime finalDate, int DateType);
        List<ma22subtripsale> GetSubtripReport(DateTime initialDate, DateTime finalDate, int DateType);
    }
}
