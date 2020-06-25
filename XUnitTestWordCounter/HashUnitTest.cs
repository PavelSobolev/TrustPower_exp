using System;
using Xunit;
using WordCounter.Services;
using Xunit.Abstractions;

namespace XUnitTestWordCounter
{
    public class UnitTestWordCounter
    {
        private readonly ITestOutputHelper Output;
        private const string TextToHash = "Enigma";

        public UnitTestWordCounter(ITestOutputHelper output)
        {
            Output = output; 
        }

        [Theory]
        [InlineData(TextToHash, 16, 32, 1000)]
        [InlineData(TextToHash, 8, 50, 1000)]
        public void ShoudReturnHashOfSpecifiedLength(string word, int saltLen, int hashLen, int iter)
        {
            string res = SaltHash.GetSaltedHash(word,saltLen, hashLen, iter);
            byte[] testBytes = Convert.FromBase64String(res);
            Assert.Equal(saltLen + hashLen, testBytes.Length);
        }

        [Theory]
        [InlineData(TextToHash, 0, 0, 0)]
        [InlineData(TextToHash, 8, 0, 1000)]
        [InlineData(TextToHash, 0, 2, 1000)]
        public void ShouldReturnEmpty(string word, int saltLen, int hashLen, int iter)
        {
            string res = SaltHash.GetSaltedHash(word, saltLen, hashLen, iter);
            Assert.Equal(0, res.Length);
        }
    }
}
