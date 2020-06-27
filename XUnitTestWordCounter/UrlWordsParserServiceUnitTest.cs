using System;
using System.Collections.Generic;
using WordCounter.Services;
using Xunit;

namespace XUnitTestWordCounter
{
    public class UrlWordsParserServiceUnitTest
    {
        private UrlWordsParserService wps = new UrlWordsParserService();

        private const string AbracadabraUrl = "http://abc.def.hij";
        private const string NormalUrl = "https://www.microsoft.com";
        List<string> ignoredWords = new List<string>(new string[] { "it", "is" });

        [Fact]
        public void ShoudReturnEmptyList()
        {
            Assert.Empty(wps.BuildDictionary(AbracadabraUrl, ignoredWords));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void ShoudThrowArgumentException(int len)
        {
            Assert.Throws<ArgumentException>(() => wps.BuildDictionary(NormalUrl, ignoredWords, len));
        }

    }
}
