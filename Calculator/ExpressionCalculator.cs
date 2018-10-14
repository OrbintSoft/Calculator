using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class ExpressionCalculator
    {
        private static readonly char[] operators = { '+', '-', '/', '*', '^', 'r', 'l' };
        private static readonly char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.' };

        private bool IsDigit(char c)
        {
            foreach (char i in digits)
            {
                if (i == c) return true;
            }
            return false;
        }

        private bool Operation(char o)
        {
            foreach (char i in operators)
            {
                if (i == o) return true;
            }
            return false;
        }

        private string Logarithm(string num1, string num2)
        {
            if ((num1 == "") && (num2) == "") return "";
            else if (num1 == "") return num2;
            else if (num2 == "") return num1;
            else
                return (Convert.ToString(Math.Log(Convert.ToDouble(num1, CultureInfo.InvariantCulture), Convert.ToDouble(num2, CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture));
        }

        private string Root(string num1, string num2)
        {
            if ((num1 == "") && (num2) == "") return "";
            else if (num1 == "") return num2;
            else if (num2 == "") return num1;
            else
                return (Convert.ToString(Math.Exp(Math.Log(Convert.ToDouble(num1, CultureInfo.InvariantCulture), Math.E) / Convert.ToDouble(num2, CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture));
        }

        private string Power(string num1, string num2)
        {
            if ((num1 == "") && (num2) == "") return "";
            else if (num1 == "") return num2;
            else if (num2 == "") return num1;
            else
                return (Convert.ToString(Math.Pow(Convert.ToDouble(num1, CultureInfo.InvariantCulture), Convert.ToDouble(num2, CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture));
        }
        private string Product(string num1, string num2)
        {
            if ((num1 == "") && (num2) == "") return "";
            else if (num1 == "") return num2;
            else if (num2 == "") return num1;
            else
                return (Convert.ToString(Convert.ToDouble(num1) * Convert.ToDouble(num2, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture));
        }

        private string Division(string num1, string num2)
        {
            if ((num1 == "") && (num2) == "") return "";
            else if (num1 == "") return num2;
            else if (num2 == "") return num1;
            else
                return (Convert.ToString(Convert.ToDouble(num1) / Convert.ToDouble(num2, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture));
        }
        private string Addition(string num1, string num2)
        {
            if ((num1 == "") && (num2) == "") return "";
            else if (num1 == "") return num2;
            else if (num2 == "") return num1;
            else
                return (Convert.ToString(Convert.ToDouble(num1) + Convert.ToDouble(num2, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture));
        }
        private string Substracion(string num1, string num2)
        {
            if ((num1 == "") && (num2) == "") return "";
            else if (num1 == "") return num2;
            else if (num2 == "") return num1;
            else
                return (Convert.ToString(Convert.ToDouble(num1) - Convert.ToDouble(num2, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture));
        }
        private string ResolveNode(ref int i, int end, string exp)
        {
            string expressNode = "";
            if ((i > end) || (exp == "")) return "";
            char[] arrayEspress = exp.ToCharArray();
            i++;
            while ((arrayEspress[i] != ')') && (i <= end))
            {
                if (arrayEspress[i] == '(') expressNode += ResolveNode(ref i, end, exp);
                else
                {
                    expressNode += arrayEspress[i];
                    i++;
                }
            }
            i++;
            return Resolve(0, expressNode.Length - 1, expressNode);
        }
        private string ResolveNodes(int start, int end, string exp)
        {
            string expressNode = "";
            string before = "";
            int i = start;
            char[] arrayEspress = exp.ToCharArray();
            if ((start > end) || (exp == "")) return "";
            while (i < end)
            {
                if (arrayEspress[i] == '(')
                {
                    expressNode = ResolveNode(ref i, end, exp);
                    before += expressNode;
                }
                if (i < end) before += arrayEspress[i];
                i++;
            }
            return Resolve(start, before.Length - 1, before);
        }


        public string Resolve(int start, int end, string exp)
        {
            char[] arrayEspress = exp.ToCharArray();
            int i = end;
            string expressNode = "";
            string after = "";
            if ((start > end) || (end >= arrayEspress.Length) || (start < 0) || (exp == "")) return "";
            //risolve somma e sottrazione
            while (i > 0)
            {
                if (arrayEspress[i] == '+')
                {
                    return Addition(Resolve(0, i - 1, exp), Resolve(0, after.Length - 1, after));
                }
                else
                    if ((arrayEspress[i] == '-') && (!Operation(arrayEspress[i - 1])))
                {
                    return Substracion(Resolve(0, i - 1, exp), Resolve(0, after.Length - 1, after));
                }
                else
                    after = arrayEspress[i] + after;
                i--;
            }
            i = end;
            after = "";
            //risolve moltiplicazione e divisione
            while (i > 0)
            {
                if (arrayEspress[i] == '*')
                {
                    return Product(Resolve(0, i - 1, exp), Resolve(0, after.Length - 1, after));
                }
                else
                    if (arrayEspress[i] == '/')
                {
                    return Division(Resolve(0, i - 1, exp), Resolve(0, after.Length - 1, after));
                }
                else
                    after = arrayEspress[i] + after;
                i--;
            }
            i = end;
            after = "";
            //risolve potenza , radice e logaritmo
            while (i > 0)
            {
                if (arrayEspress[i] == '^')
                {
                    return Power(Resolve(0, i - 1, exp), Resolve(0, after.Length - 1, after));
                }
                else
                    if (arrayEspress[i] == 'r')
                {
                    return Root(Resolve(0, i - 1, exp), Resolve(0, after.Length - 1, after));
                }
                else
                        if (arrayEspress[i] == 'l')
                {
                    return Logarithm(Resolve(0, i - 1, exp), Resolve(0, after.Length - 1, after));
                }
                else
                    after = arrayEspress[i] + after;
                i--;
            }

            for (i = start; i <= end; i++)
            {
                expressNode += arrayEspress[i];
            }
            return expressNode;
        }

        public double GetResult(string espressione)
        {
            string a = ResolveNodes(0, espressione.Length, espressione);
            return Convert.ToDouble(a, CultureInfo.InvariantCulture);
        }
    }
}
