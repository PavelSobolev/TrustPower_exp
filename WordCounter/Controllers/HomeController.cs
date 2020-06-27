using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WordCounter.Contexts;
using WordCounter.Models;
using WordCounter.Services;

namespace WordCounter.Controllers
{
    /// <summary>
    /// Main controller of the app
    /// </summary>
    public class HomeController : Controller
    {

        //number of words in the list
        private const int MaxWords = 100;

        // injected services
        private readonly IWordsService _wordsUrlParser;
        private readonly DictContext _dictContext;


        public HomeController(IWordsService wordsUrlParser, DictContext dictContext)
        {
            _wordsUrlParser = wordsUrlParser;
            _dictContext = dictContext;
        }

        /// <summary>
        /// this method is used to build words' cloud for specified url
        /// </summary>
        /// <returns>view with input form and word cloud</returns>
        public IActionResult Index()
        {
            // getting of URL from the form being used 
            string Url = string.Empty;

            try
            {
                if (Request != null)
                    Url = Request.Form["url"];
            }
            catch (Exception)
            {
                // if data was not provided: empty list and empty url are sent to the app
                // list and url are sent within a tuple: Tuple<List<DictEntry>, string>
                return View((new List<DictEntry>(), Url));
            }

            if (Url.Trim().Length == 0) // empty URL was received
                return View((new List<DictEntry>(), Url));

            //var Url = "https://www.stuff.co.nz"; // "https://www.tvnz.co.nz/one-news";

            // build dictionary of MaxWords top words
            List<string> ignoredList = _dictContext.IgnoredWords.Select(entry => entry.Word).ToList();

            List<DictEntry> wordList = _wordsUrlParser.BuildDictionary(Url, ignoredList, MaxWords);

            // send data to the database
            InsertOrUpdateWords(wordList);

            // send data to the view for rendering
            return View((wordList, Url));
        }

        /// <summary>
        /// Method inserts words form the list or updates them if they already exist 
        /// </summary>
        /// <param name="wordList">list of words to be inserted</param>
        private void InsertOrUpdateWords(List<DictEntry> wordList)
        {
            wordList.ForEach(dictEntry =>
            {
                // detect this word in the DB
                var foundWords = _dictContext.Words.Where(entry => entry.Word == dictEntry.Word);

                bool entryExists = foundWords.Count() > 0;

                if (!entryExists)
                    _dictContext.Words.AddAsync(dictEntry);
                else
                {
                    foundWords.First().Count += dictEntry.Count;
                    _dictContext.Words.Update(foundWords.First());
                }
            });

            _dictContext.SaveChangesAsync();

        }

        /// <summary>
        /// Admin() gets complete list of words form the DB 
        /// </summary>
        /// <returns></returns>
        public IActionResult Admin()
        {
            ViewData["Message"] = "Complete statistics of words in the database";

            // select all the records from table containing words from visited websites
            return View(_dictContext.Words.OrderByDescending(entry => entry.Count).ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
