using Infrastructure.Identity.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AuthReviewProject.Tests.UnitTests.Infrastructure.Identity.Tools
{
    public class EncryptOfSystemTest
    {
        [Theory]
        [InlineData("123", "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3")]
        [InlineData("abc", "ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad")]
        [InlineData("ABC123", "e0bebd22819993425814866b62701e2919ea26f1370499c1037b53b9d49c2c8a")]
        public void GetSHA256OfString_InsertString_And_ReturnSHA256(string password, string expected)
        {
            var encrypt = new EncryptOfSystem();

            var result = encrypt.GetSHA256OfString(password);

            Assert.Equal(expected, result);
        }
    }
}
