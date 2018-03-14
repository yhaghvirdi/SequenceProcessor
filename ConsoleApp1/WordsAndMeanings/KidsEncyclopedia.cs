using System;
using System.IO;
using System.Linq;
using System.Net;

using ConsoleApp1.Modules;

using HtmlAgilityPack;

namespace ConsoleApp1.WordsAndMeanings {
  public class KidsEncyclopedia:IWordsMaster{
    private static string url = "https://kids.britannica.com/kids/search/dictionary?query={0}";

    public string GetWordMeaning( string word ) {
      var result = string.Empty;
      var web = new HtmlWeb();
      try {
        var doc = web.Load( string.Format( url, word ) );
        result = doc.DocumentNode.Descendants( "dd" ).First().InnerText;
        result = result.Remove( 0, result.LastIndexOf( "&nbsp;", StringComparison.InvariantCultureIgnoreCase ) + 6 );
        result = result.Replace( "&gt;", " " );
        result = result.Replace( "&lt;", " " );
      }
      catch ( Exception ex ) {
        Logger.Log( $"Getting meaning for {word} failed. Reference: KidsEncyclopedia." );
      }
      return result;
    }

  }
}
