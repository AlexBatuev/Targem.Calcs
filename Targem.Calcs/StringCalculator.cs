using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Targem.Calcs
{
    public static class StringCalculator
    {
        private const string ScopePattern = @"\(([1234567890\.\+\-\*\/]*)\)";
        private const string NumberPattern = @"[-]?\d+\.?\d*";
        private const string MultPattern = @"[\*\/]";
        private const string AddPattern = @"[\+\-]";

        /// <summary>
        /// Recursively parses the input string into tokens and calculates their result
        /// </summary>
        /// <param name="input">String with a math expression</param>
        /// <returns>Double-precision floating-point number, calculation result</returns>
        /// <exception cref="FormatException">Error in the input math expression</exception>
        public static double Parse(string input)
        {
            var scopesMatch = Regex.Match(input, ScopePattern);
            if (scopesMatch.Groups.Count > 1)
            {
                var inner = scopesMatch.Groups[0].Value.Substring(1, scopesMatch.Groups[0].Value.Trim().Length - 2);
                var left = input.Substring(0, scopesMatch.Index);
                var right = input.Substring(scopesMatch.Index + scopesMatch.Length);
                return Parse(left + Parse(inner) + right);
            }

            var multMatch = Regex.Match(input, $@"({NumberPattern})\s?({MultPattern})\s?({NumberPattern})\s?");
            var addMatch = Regex.Match(input, $@"({NumberPattern})\s?({AddPattern})\s?({NumberPattern})\s?");
            var match = (multMatch.Groups.Count > 1) ? multMatch : (addMatch.Groups.Count > 1) ? addMatch : null;
            if (match != null)
            {
                var left = input.Substring(0, match.Index);
                var right = input.Substring(match.Index + match.Length);
                var val = Calc(match).ToString(CultureInfo.InvariantCulture);
                return Parse($"{left}{val}{right}");
            }

            try
            {
                return double.Parse(input, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                throw new FormatException($"Invalid input string '{input}'");
            }
        }

        private static double Calc(Match match)
        {
            var a = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            var b = double.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

            switch (match.Groups[2].Value)
            {
                case "+":
                    return a + b;
                case "-":
                    return a - b;
                case "*":
                    return a * b;
                case "/":
                    return a / b;
                default:
                    throw new FormatException($"Invalid input string '{match.Value}'");
            }
        }
    }
}
