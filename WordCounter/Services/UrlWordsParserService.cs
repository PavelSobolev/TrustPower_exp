using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using WordCounter.Models;

namespace WordCounter.Services
{
    /// <summary>
    /// UrlWordsParserService allows to get ordered list of words of the website
    /// specified by its URL
    /// </summary>
    public class UrlWordsParserService : IWordsService
    {

        //these constants are used in methods which get salted hash
        private const int SaltLength = 16;
        private const int HashLength = 32;
        private const int HashIterations = 100;

        // these words are ignored
        private  readonly List<string> NonInformative = new List<string>(new string[] {
            "the", "a", "to", "in", "of", "for", "and", "or", "at", "on", "up", "by", "as", "an", 
            "has", "have", "is", "are", "can", "may", "must", "were", "was", "been", "be", "am"});


        /// <summary>
        /// Build sorted list of words entries 
        /// </summary>
        /// <param name="inText"></param>
        /// <returns></returns>
        public List<DictEntry> BuildDictionary(string Url, int listLength = 100)
        {
            if (listLength <= 0)
                throw new ArgumentException($"{listLength} is not valid value (positive integer is required).");

            // get text of the website (from its Url)
            string inText = StripHtmlFromUrlContent(Url);

            if (inText.Length == 0)
                return new List<DictEntry>();

            // build a list of words from inText
            List<string> wordsList = new List<string>(
                inText.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));

            // list of different words
            List<DictEntry> listDE = new List<DictEntry>();

            foreach (string word in wordsList)
            {
                string normWord = RemovePunct(word).ToLower();

                if (IsCorrectWord(normWord)) // check input string
                {
                    // trying to find word in the list
                    var wordInList = listDE.Where(dictEntry => dictEntry.Word == normWord);

                    if (wordInList.Count() == 0) // if thw word is not found
                    {
                        DictEntry de = new DictEntry()
                        {
                            Id = SaltHash.GetSaltedHash(normWord, SaltLength, HashLength, HashIterations),
                            Count = 1,
                            Word = normWord
                        };
                        listDE.Add(de);
                    }
                    else // if thw word was found
                    {
                        wordInList.First().Count++;
                    }
                }
            }

            if (listDE.Count() <= listLength) // get sorted list and take 'listLength' words
                return listDE.OrderByDescending(entry=>entry.Count).ToList();
            else
                return listDE.OrderByDescending(entry => entry.Count).Take(listLength).ToList();
        }

        /// <summary>
        /// StripHtmlFromUrlContent allows to remove HTML markup from content of the specified URL
        /// </summary>
        /// <param name="Url">Input string with resource address</param>
        /// <returns>Text of the website without HTML</returns>
        private string StripHtmlFromUrlContent(string Url)
        {
            // create obj HtmlWeb from HtmlAgilityPack library
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc;

            try
            {
                doc = web.Load(Url);
            }
            catch(Exception)
            {
                return string.Empty; //Url was not found
            }

            var output = string.Empty;

            // traversing all the child nodes of the doc
            foreach (var node in doc.DocumentNode.ChildNodes)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    // get element's inner text and remove non-printable characters 
                    string innerText = Regex.Replace(node.InnerText.Trim(),
                        @"[\s\r\n\p{Pc}]+", " ",
                        RegexOptions.Multiline);
                    // normalize innertext by removing special html words (like &nbsp; and others)
                    innerText = WebUtility.HtmlDecode(innerText);

                    if (innerText.Length > 0)
                        output += innerText;
                }
            }

            return output;
        }

        /// <summary>
        /// IsCorrectWord allows to determine whether text represents alphanumeric 
        /// combination of characters (in order to catch standalone digits, numbers, 
        /// dates ... )
        /// </summary>
        /// <param name="testText">input text</param>
        /// <returns></returns>
        private  bool IsCorrectWord(string testText)
        {
            // remove leading and trailing white-spaces
            string txt = testText.Trim();

            // don't use empty strings
            if (txt.Length == 0) return false;

            // check for presence in the list of ignored words
            if (NonInformative.Contains(txt)) return false;

            double resNum = 0.0; // check for a number
            if (double.TryParse(txt, out resNum)) return false;

            if (Regex.IsMatch(testText, @"\d"))
            {
                if (Regex.IsMatch(testText, @"[\w]"))
                    return true;
                else
                    return false;
            }

            return true;

        }

        /// <summary>
        /// RemovePunct removes symbols which are non-alpanumeric characters.
        /// </summary>
        /// <param name="remStr">Input text</param>
        /// <returns>Simple alpanumeris string</returns>
        private string RemovePunct(string remStr)
        {
            string res = Regex.Replace(remStr, @"[.,;:""(){}\[\]!?~`@#$%^&*()_\-+=/\\|]+",
                "",
                RegexOptions.Singleline).Trim('\'').Trim();
            return res;
        }
    }
}
