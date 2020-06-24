using System.Collections.Generic;
using WordCounter.Models;

namespace WordCounter.Services
{
    /// <summary>
    /// THis interface can be used for building dictionaries form different sources and using different algorithms
    /// </summary>
    public interface IWordsService
    {
        List<DictEntry> BuildDictionary(string inText, int listLength);
    }
}
