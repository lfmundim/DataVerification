using DataValidator.Cnpj;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DataValidator.Tests
{
    public class CnpjUnitTests
    {
        private ICheckCnpj checkCnpj;

        [Theory]
        [InlineData("oi meu cnpj eh esse daqui 325679210001-27", true, "32567921000127", "32.567.921/0001-27")]
        [InlineData("00000000000000000000032.567.921/0001-27", true, "32567921000127", "32.567.921/0001-27")]
        [InlineData("32.567.921/0001-27", true, "32567921000127", "32.567.921/0001-27")]
        [InlineData("36.732.527/0001-58", true, "36732527000158", "36.732.527/0001-58")]
        [InlineData("88.398.308/0001-88", true, "88398308000188", "88.398.308/0001-88")]
        [InlineData("67.613.129/0001-46", true, "67613129000146", "67.613.129/0001-46")]
        [InlineData("105340946-00", false, "", "")]
        [InlineData("02.526.904/0001-81", false, "02526904000181", "02.526.904/0001-81")]
        [InlineData("76.599.093/0001-85", false, "76599093000185", "76.599.093/0001-85")]
        [InlineData("11111111111111", false, "11111111111111", "11.111.111/1111-11")]
        [InlineData("banana", false, "", "")]
        [InlineData("opa é 249965000112", true, "00249965000112", "00.249.965/0001-12")]
        [InlineData("Meu Cnpj é 40015389000163", true, "40015389000163", "40.015.389/0001-63")]
        [InlineData("meu cnpj é 40.015.389/0001-63", true, "40015389000163", "40.015.389/0001-63")]
        [InlineData("CPF: 40.015.389/0001-63", true, "40015389000163", "40.015.389/0001-63")]
        [InlineData("oi boa noite meu cpf eh 40.015.389/0001-63", true, "40015389000163", "40.015.389/0001-63")]
        [InlineData("3519080035621300", false, "3519080035621300", "35.190.800/3562-13")]
        public void ExtractAndCheckCnpj_ShouldReturnCheckCnpj(string text, bool expectedResult, string expectedCnpj, string expectedFormatted)
        {
            // Arrange
            checkCnpj = new CheckCnpj();

            // Act
            var result = checkCnpj.ExtractAndCheckCnpj(text);

            // Arrange
            result.IsValid.ShouldBe(expectedResult);
            result.Identifier.ShouldBe(expectedCnpj);
            result.Formatted.ShouldBe(expectedFormatted);
        }
    }
}
