using System.Collections.Generic;
using WordCounter.Models;

namespace WordCounter.Services
{
    /// <summary>
    /// THis interface can be used for building dictionaries form different sources and using different algorithms
    /// </summary>
    public interface IWordsService
    {
        /// <summary>
        /// Method  returns a list of words (of specified length) containing words from specified source 
        /// </summary>
        /// <param name="sourceAddress">address of data source in string format</param>
        /// <param name="nonInformative">list of ignored words</param>
        /// <param name="listLength">length of list</param>
        /// <returns>list of words</returns>
        List<DictEntry> BuildDictionary(string sourceAddress, IEnumerable<string> nonInformative, int listLength);
    }
}
