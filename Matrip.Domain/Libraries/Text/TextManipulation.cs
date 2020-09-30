using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Matrip.Domain.Libraries.Text
{
    public class TextManipulation
    {
        public static bool RemoveAccents(string source, string search)
        {
            return new CultureInfo("pt-BR").CompareInfo
                .IndexOf(source, search, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
        }
    }
}
