using System;
using WordCounter.Services;
using Xunit;

namespace XUnitTestWordCounter
{
    public class UrlWordsParserServiceUnitTest
    {
        private UrlWordsParserService wps = new UrlWordsParserService();

        private const string AbracadabraUrl = "http://abc.def.hij";
        private const string NormalUrl = "https://www.microsoft.com";

        [Fact]
        public void ShoudReturnEmptyList()
        {
            Assert.Empty(wps.BuildDictionary(AbracadabraUrl));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void ShoudThrowArgumentException(int len)
        {
            Assert.Throws<ArgumentException>(() => wps.BuildDictionary(NormalUrl, len));
        }

    }
}
