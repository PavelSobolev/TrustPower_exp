using System;
//using System.Collections;
//using System.Collections.Generic;
using System.IO;
using System.Text;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.maxent;
using edu.stanford.nlp.tagger.maxent;
using java.io;
using java.util;
using Console = System.Console;

namespace RemoveHTML
{
    /// <summary>
    /// Class allows to determine part of speech for the specified word
    /// using Stanford.NLP.NET library http://sergey-tihon.github.io/Stanford.NLP.NET/samples/POSTagger.html
    /// </summary>
    class POS
    {
        public static void PrintPOS(string word)
        {
            var root = @"C:\DataFiles\POS";
            var modelsDirectory = root + @"\models";
            var model = modelsDirectory + @"\english-bidirectional-distsim.tagger";
                //wsj-0-18-bidirectional-nodistsim.tagger";

            if (!System.IO.File.Exists(model))
                throw new Exception($"Check path to the model file '{model}'");

            // Loading POS Tagger
            var tagger = new MaxentTagger(model);

            // Text for tagging
            var text = "A Part-Of-Speech Tagger (POS Tagger) is a piece of software that reads text"
                       + "in some language and assigns parts of speech to each word (and other token),"
                       + " such as noun, verb, adjective, etc., although generally computational "
                       + "applications use more fine-grained POS tags like 'noun-plural'.";

            var sentences = MaxentTagger.tokenizeText(new java.io.StringReader(text)).toArray();
            foreach (ArrayList sentence in sentences)
            {
                var taggedSentence = tagger.tagSentence(sentence);
                Console.WriteLine(SentenceUtils.listToString(taggedSentence, false));
            }
        }
    }
}
