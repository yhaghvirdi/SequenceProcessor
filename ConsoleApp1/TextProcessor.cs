using System;
using System.Collections.Generic;

namespace ConsoleApp1 {
  public static class TextProcessor {
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
  }
}
