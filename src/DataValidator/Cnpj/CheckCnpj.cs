using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DataValidator.Cnpj
{
    public class CheckCnpj : ICheckCnpj
    {
        private static int[] Multiplier1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        private static int[] Multiplier2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        private const string GetNumberRegex = "^[0-9]+$";
        private const string CnpjRegex = @"\b((\s|\-|\.|\/)*(^\d)*\d*(\s|\-|\.|\/)*\d\s*\d\s*\d(\s|\-|\.|\/)*\d\s*\d\s*\d(\s|\-|\.|\/)*\d\s*\d\s*\d\s*\d(\s|\-|\.|\/)*\d\s*\d(^\d)*)\b";

        private readonly Regex _rgx;
        public CheckCnpj()
        {
            _rgx = new Regex(GetNumberRegex, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Returns if a CNPJ string is valid or not
        /// </summary>
        /// <param name="cnpj">CNPJ string to be validated</param>
        /// <returns></returns>
        public bool isValid(string cnpj)
        {
            var verifier = new int[2];
            cnpj = ExtractCnpj(cnpj);
            if (HaveSameDigit(cnpj) || !_rgx.IsMatch(cnpj)) return false;

            cnpj = new string(cnpj.Where(Char.IsDigit).ToArray()).PadLeft(14, '0');

            var cnpjList = cnpj.Select(c => int.Parse(c.ToString())).ToList();
            int sum = 0;

            for (int i = 0; i < 12; i++)
                sum += cnpjList[i] * Multiplier1[i];

            int remainder = sum % 11;

            verifier[0] = remainder < 2 ? 0 : (11 - remainder);
            sum = 0;

            for (int i = 0; i < 13; i++)
                sum += cnpjList[i] * Multiplier2[i];

            remainder = sum % 11;
            verifier[1] = remainder < 2 ? 0 : (11 - remainder);
            var verifierDigits = verifier[0].ToString() + verifier[1].ToString();

            return new string(cnpj.Take(14).ToArray()).EndsWith(verifierDigits);
        }

        /// <summary>
        /// Returns the extracted CNPJ value from input text and if it's valid or not
        /// </summary>
        /// <param name="text">Text containing the CNPJ to be extracted.</param>
        /// <returns></returns>
        public Cnpj ExtractAndCheckCnpj(string text)
        {
            var matches = Regex.Matches(text, CnpjRegex);
            if (matches.Count <= 0 || matches[0].Groups.Count <= 0)
                return new Cnpj(null, false);
            var result = matches[0].Groups[0].Value;

            var extracted = ExtractCnpj(result);

            return new Cnpj(extracted, isValid(extracted));
        }

        private static string ExtractCnpj(string cnpj)
        {
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace(" ", "").Replace("/", "").Trim();
            cnpj = new string(cnpj.Where(Char.IsDigit).ToArray()).TrimStart('0').PadLeft(14, '0');
            return cnpj;
        }

        private bool HaveSameDigit(string cnpj)
        {
            return cnpj.Distinct().Count() == 1;
        }
    }
}
