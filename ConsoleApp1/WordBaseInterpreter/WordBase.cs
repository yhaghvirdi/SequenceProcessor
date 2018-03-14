using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace ConsoleApp1.WordBaseInterpreter {
  public class WordBase {
    public string[] Meanings { get; set; }
    public string[] Words { get; set; }

    public void Populate(string filePath ) {
      var filestream = new StreamReader( filePath );
      var words = new List<string>();
      var firstLine = true;

      while ( !filestream.EndOfStream ) {
        var lineVal = filestream.ReadLine();
        if ( firstLine ) {
          Meanings = lineVal.Split( ',' );
          firstLine = false;
          continue;
        }
        words.Add( lineVal );
      }
      Words = words.ToArray();
    }

    public void Populate( string[] wordList, string[] meaningsList = null ) {
      Words = wordList;
      Meanings = meaningsList;
    }

    //just to keep this code safe
    private static void LoadVocabularyListsToFile() {
      var url = "http://www.manythings.org/vocabulary/lists/c/";
      var web = new HtmlWeb();
      var doc = web.Load( url );
      var queryStrings = doc.DocumentNode.Descendants( "a" ).Skip( 2 ).Take( 219 ).
        Select( item => new Tuple<string, string>( item.InnerText, item.Attributes["href"].Value ) );

      var startLink = "http://www.manythings.org/vocabulary/lists/c/";

      foreach ( var queryString in queryStrings ) {
        var meanings = queryString.Item1.ToLower().Split( ' ' );
        doc = web.Load( startLink + queryString.Item2 );
        var words = doc.DocumentNode.Descendants( "li" ).Select( item => item.InnerText );
        var textForfile = meanings.Aggregate( ( item, next ) => item + ',' + next );
        textForfile = words.Aggregate( textForfile, ( current, word ) => current + ( "\r\n" + word ) );

        File.WriteAllText( "wordbase/" + queryString.Item1 + ".txt", textForfile );
      }
    }
  }
}
