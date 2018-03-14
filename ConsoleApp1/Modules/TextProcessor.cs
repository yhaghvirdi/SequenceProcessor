using System;
using System.Collections.Generic;
using System.Linq;

using ConsoleApp1.Models;

using Microsoft.SqlServer.Server;

namespace ConsoleApp1.Modules {
  public static class TextProcessor {
    public static IntentType FindIntent( string text ) {
      var splitted = text.Split( ' ' );

      if ( !splitted.Any() ) return IntentType.NotSure;

      if ( splitted.First().StartsWith( "-" ) ) return IntentType.Command;

      if ( splitted.Length == 1 ) return Logger.LogIntent( IntentType.SearchWord );

      if ( splitted.First().EndsWith( ":" ) ) return Logger.LogIntent( IntentType.AddMeaning );

      return Logger.LogIntent( IntentType.NotSure );
    }

    public static string RemoveTag( string tagStart, string tagEnd, string input, bool justFirst = false ) {
      if ( input.Contains( tagStart ) ) {
        var firstStartIndex = input.IndexOf( tagStart, StringComparison.Ordinal );
        var firstEndIndex = input.IndexOf( tagEnd, StringComparison.Ordinal );
        var input1 = input.Substring( 0, firstStartIndex ) + input.Substring( firstEndIndex + tagEnd.Length );
        return justFirst ? input1 : RemoveTag( tagStart, tagEnd, input1 );
      }
      return input;
    }

    public static List<string> ExtractTagContent( string tagStart, string tagEnd, string input, bool justFirst = false ) {
      if ( input.Contains( tagStart ) ) {
        var firstStartIndex = input.IndexOf( tagStart, StringComparison.Ordinal );
        var firstEndIndex = input.IndexOf( tagEnd, StringComparison.Ordinal );
        var input1 = input.Substring( firstStartIndex + tagStart.Length, firstEndIndex - firstStartIndex - tagStart.Length );
        input = RemoveTag( tagStart, tagEnd, input, true );
        var sublist = justFirst ? new List<string>() : ExtractTagContent( tagStart, tagEnd, input );
        sublist.Add( input1 );
        return sublist;
      }
      return new List<string>();
    }

    public static string[] GetWords( string sentence ) {
      return RemoveAllButAlphanumeric( sentence.Trim( ' ', '.' ) ).Split( ' ' ).Except( new[] { " " } ).ToArray();
    }

    private static string RemoveAllButAlphanumeric(string input ) {
      char[] arr = input.Where( c => ( char.IsLetterOrDigit( c ) ||
                                     char.IsWhiteSpace( c ) ) ).ToArray();

      input = new string( arr );
      return input;
    }

  }
}
