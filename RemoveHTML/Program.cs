using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Console;

namespace RemoveHTML
{
    class Program
    {
        private const int MaxWords = 100;
        private const int SaltLength = 16;
        private const int HashLength = 32;
        private const int HashIterations = 100;

        private static readonly List<string> NonInformative = new List<string>(new string[] { 
            "the", "a", "to", "in", "of", "for", "and", "or", "at", "on", "up", "by", "as", "an" });

        static async Task Main(string[] args)
        {
            //POS.PrintPOS("");

            var url = "https://www.stuff.co.nz"; // "https://www.tvnz.co.nz/one-news";
            string webSiteTxt = await StripHtmlFromUrlContent(url);
            List<DictEntry> res = BuildOrderedDictionary(webSiteTxt, MaxWords);

            int c = 1;
            foreach(DictEntry de in res)
            {
                WriteLine($"{c++}: {de.Id} => '{de.Word}' => {de.Count}");
            }

            WriteLine();

            ReadKey();

        }

        static async Task PrintWords(string[] args)
        {

            var url = "https://www.stuff.co.nz"; // "https://www.tvnz.co.nz/one-news";
            string webSiteTxt = await StripHtmlFromUrlContent(url);
            List<DictEntry> res = BuildOrderedDictionary(webSiteTxt, MaxWords);
        }

        /// <summary>
        /// IsCorrectWord allows to determine whether text represents alphanumeric 
        /// combination of characters (in order to catch standalone digits, numbers, 
        /// dates ... )
        /// </summary>
        /// <param name="testText">input text</param>
        /// <returns></returns>
        public static bool IsCorrectWord(string testText)
        {
            string txt = testText.Trim();

            if (txt.Length == 0) return false;

            if (NonInformative.Contains(txt)) return false;

            if (Regex.IsMatch(testText,@"\d"))
            {
                if (Regex.IsMatch(testText, @"\w"))
                    return false;
                else
                    return true;
            }

            return true;
            
        }

        /// <summary>
        /// RemovePunct removes symbols which are non-alpanumeric characters.
        /// </summary>
        /// <param name="remStr">Input text</param>
        /// <returns>Simple alpanumeris string</returns>
        public static string RemovePunct(string remStr)
        {
            string res = Regex.Replace(remStr, @"[.,;:""!@#$%^&*()_\-+=/\\|]+",
                string.Empty,
                RegexOptions.Singleline).Trim('\'').Trim();
            return res;
        }

        /// <summary>
        /// StripHtmlFromUrlContent allows to remove HTML markup from content of the specified URL
        /// </summary>
        /// <param name="Url">Input string with resource address</param>
        /// <returns>Text of the website without HTML</returns>
        public static async Task<string> StripHtmlFromUrlContent(string Url)
        {
            // create obj HtmlWeb from HtmlAgilityPack library
            HtmlWeb web = new HtmlWeb(); 
            HtmlDocument doc = await web.LoadFromWebAsync(Url);

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
        /// Build sorted list of words entries 
        /// </summary>
        /// <param name="inText"></param>
        /// <returns></returns>
        public static List<DictEntry> BuildOrderedDictionary(string inText, int listLength = 100)
        {
            // build a list of words from inText
            List<string> wordsList = new List<string>(
                inText.Split(new string[] { " " },
                    StringSplitOptions.RemoveEmptyEntries));
            
            List<DictEntry> listDE = new List<DictEntry>();

            foreach(string word in wordsList)
            {
                string normWord = RemovePunct(word).ToLower();

                if (IsCorrectWord(normWord))
                {
                    var wordInList = listDE.Where(dictEntry => dictEntry.Word == normWord);

                    if (wordInList.Count() == 0)
                    {
                        DictEntry de = new DictEntry()
                        {
                            Id = Hasher.SaltedHash(normWord, SaltLength, HashLength, HashIterations),
                            Count = 1,
                            Word = normWord
                        };
                        listDE.Add(de);
                    }
                    else
                    {
                        wordInList.First().Count++;
                    }
                }
            }

            if (listDE.Count() <= listLength)
                return listDE.OrderByDescending(dictEntry => dictEntry.Count).ToList();
            else
                return listDE.OrderByDescending(dictEntry => dictEntry.Count).Take(listLength).ToList();
        }
    }

    public class DictEntry
    {
        public string Id { get; set; }
        public string Word { get; set; }
        public int Count { get; set; }
    }
}

/*
 * План
 * 1. Прочитать содердимое сайта по ео адресу - если нет адреса поймать ошибку
 * 2. Удалить теги и вставить разделитель вместо них
 * 3. Разбить текст в массив по разделителю
 * 4. Подсчитать сколько раз входит каждое слово и записать в словарь  
 * 4а. Создать парочку Unit-тестов
 * 5. Создать БД в Azure и сценарий приложить
 * 6. Как связаться с БД Azure из веб-апп
 * 7. Создать веб-апп
 * 8. Поместить веб-апп в Azure*/