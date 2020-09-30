using System;
using System.Collections.Generic;
using System.Text;

namespace Matrip.Domain.Libraries.Text
{
    /// <summary>
    /// Classe responsável por manipular datas
    /// </summary>
    public class DateConvert
    {
        /// <summary>
        /// Método que retorna o dia e hora de acordo com o horário de Brasília
        /// </summary>
        public static DateTime HrBrasilia()
        {
            var Date = DateTime.UtcNow;
            TimeZoneInfo hrBrasilia = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(Date, hrBrasilia);
        }
    }
}
