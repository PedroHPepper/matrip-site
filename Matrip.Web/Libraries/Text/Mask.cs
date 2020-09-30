namespace Matrip.Web.Libraries.Text
{
    public class Mask
    {
        public static string Remove(string value)
        {
            return value.Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", "").Replace("R$", "").Replace(",", "").Replace(" ", "").Replace("$","");
        }
        public static int ConvertValuePagarMe(decimal valor)
        {
            string valorString = valor.ToString("C");
            valorString = Remove(valorString);
            int valorInt = int.Parse(valorString);
            return valorInt;
        }
        public static decimal ConvertPagarMeIntToDecimal(int valor)
        {
            //10000 -> "10000" -> "100.00" -> 100.00
            string valorPagarMeString = valor.ToString();
            string valorDecimalString = valorPagarMeString.Substring(0, valorPagarMeString.Length - 2) + "," + valorPagarMeString.Substring(valorPagarMeString.Length - 2);

            var dec = decimal.Parse(valorDecimalString);

            return dec;
        }
        public static int ExtractRequestCode(string codigoPedido, out string transactionId)
        {
            string[] resultadoSeparacao = codigoPedido.Split("-");

            transactionId = resultadoSeparacao[1];

            return int.Parse(resultadoSeparacao[0]);
        }
    }
}
