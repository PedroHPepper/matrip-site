using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Matrip.Domain.Libraries.Text
{
    public class ValueConvert
    {
        /// <summary>
        /// Converte o valor de Double para a formatação monetária em Reais
        /// </summary>
        /// <param name="value">Valor em Double</param>
        /// <returns></returns>
        public static string ConvertToReal(double value)
        {
            return value.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
        }

        /// <summary>
        /// Converte o valor de Float para a formatação monetária em Reais
        /// </summary>
        /// <param name="value">Valor em Float</param>
        /// <returns></returns>
        public static string ConvertFloatToReal(float value)
        {
            return value.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
        }
    }
}
