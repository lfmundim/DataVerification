using DataValidator.PhoneNumber;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DataValidator.Tests
{
    public class PhoneNumberUnitTests
    {
        public ICheckPhoneNumber checkPhoneNumber;

        [Theory]
        [InlineData("meu telefone é +55031986619392 vlw", true, "+55031", "986619392", "+55031986619392")]
        [InlineData("meu telefone é +55 031 9866-19392 vlw", true, "+55031", "986619392", "+55031986619392")]
        [InlineData("me liga no +5531986619392 vlw", true, "+5531", "986619392", "+5531986619392")]
        [InlineData("me liga no +5531 98661-9392 vlw", true, "+5531", "986619392", "+5531986619392")]
        [InlineData("me chama no zap 986619392", true, "", "986619392", "986619392")]
        [InlineData("me chama no zap 31986619392 vlw", true, "31", "986619392", "31986619392")]
        [InlineData("me chama no zap 031986619392 vlw", true, "031", "986619392", "031986619392")]
        [InlineData("me chama no 86619392 vlw", true, "", "86619392", "86619392")]
        [InlineData("me chama no 8661-9392 vlw", true, "", "86619392", "86619392")]
        [InlineData("me chama no zap vlw", false, "", "", "")]
        [InlineData("Meu telefone fixo é (031) 3221-3344", true, "031", "32213344", "03132213344")]
        [InlineData("03132213344", true, "031", "32213344", "03132213344")]
        public void ExtractAndValidatePhoneNumber(string text, bool isValid, string expectedRegionCode, string expectedNumber, string expectedFullNumber)
        {
            // Arrange
            checkPhoneNumber = new CheckPhoneNumber();

            // Act
            var result = checkPhoneNumber.ValidatePhoneNumber(text);

            // Arrange
            result.FullNumber.ShouldBe(expectedFullNumber);
            result.FullNumber.ShouldBe(expectedRegionCode + expectedNumber);
            result.IsValid.ShouldBe(isValid);
            result.Number.ShouldBe(expectedNumber);
            result.RegionCode.ShouldBe(expectedRegionCode);
        }
    }
}
