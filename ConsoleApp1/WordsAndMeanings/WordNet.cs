using System.IO;
using System.Linq;
using System.Net;

using ConsoleApp1.Modules;

namespace ConsoleApp1.WordsAndMeanings {
  public class WordNet:IWordsMaster{
    private static string url = "http://wordnetweb.princeton.edu/perl/webwn?s={0}&sub=Search+WordNet&o2=&o0=&o8=1&o1=1&o7=&o5=&o9=&o6=&o3=&o4=&h=000000000000000";

    public string GetWordMeaning( string word ) {
      var result = string.Empty;
      var request = (HttpWebRequest) WebRequest.Create( string.Format( url, word ) );
      using ( var response = request.GetResponse() ) {
        using ( var stream = response.GetResponseStream() ) {
          if ( stream != null ) {
            var reader = new StreamReader( stream );
            var output = reader.ReadToEnd();
            var resultTags = TextProcessor.ExtractTagContent( "<ul>", "</ul>", output, true );
            if ( resultTags.Any() ) {
              var trimmedOutput = resultTags.First();
              trimmedOutput = TextProcessor.RemoveTag( "<a", "</a>", trimmedOutput );
              trimmedOutput = TextProcessor.RemoveTag( "<b", "</b>", trimmedOutput );
              result = TextProcessor.ExtractTagContent( "<li>", "</li>", trimmedOutput, true ).First().Trim( ' ', '(', ')', ',' );
            }
          }
        }
      }
      return result;
    }

  }
}
