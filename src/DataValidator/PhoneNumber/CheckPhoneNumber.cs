using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DataValidator.PhoneNumber
{
    public class CheckPhoneNumber : ICheckPhoneNumber
    {
        public Regex PhoneRegex { get; set; }

        public CheckPhoneNumber()
        {
            //https://regex101.com/r/U4hWBx/5
            PhoneRegex = new Regex(@"(((?<CountryState>\+\d\d\(?0?\d\d\)?)|(?<State>(\()?0?\d\d(\))?))?(?<Phone>(\d){8,}))");
        }

        /// <summary>
        /// Extracts and validates a phone number from text using Brazil's mask
        /// </summary>
        /// <param name="input">Text to search phone number on</param>
        /// <returns></returns>
        public PhoneNumber ValidatePhoneNumber(string input)
        {
            var toReturn = new PhoneNumber("", false);

            try
            {
                var clearedInput = input.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Trim();
                var collisions = PhoneRegex.Matches(clearedInput);
                if (collisions.Count == 0)
                {
                    return toReturn;
                }

                var extracted = collisions[0];
                toReturn.FullNumber = extracted.Value;
                toReturn.IsValid = true;
                toReturn.Number = extracted.Groups["Phone"].Value;
                var countryState = extracted.Groups["CountryState"].Value;
                var state = extracted.Groups["State"].Value;
                toReturn.RegionCode = !string.IsNullOrEmpty(countryState) 
                                      ? countryState 
                                      : !string.IsNullOrEmpty(state) 
                                      ? state 
                                      : "";
            }
            catch (Exception e)
            {
                toReturn.Number = $"Exception thrown: {e}";
                return toReturn;
            }

            return toReturn;
        }
    }
}
