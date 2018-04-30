using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using ConsoleApp1.Modules;
using ConsoleApp1.WordBaseInterpreter;

using HtmlAgilityPack;

namespace ConsoleApp1.WordsAndMeanings {
  public class Wordsmyth: IWordsMaster {
    private static string url = "https://kids.wordsmyth.net/we/?ent={0}";

    public string GetWordDefinition( string word ) {
      var meaning = string.Empty;
      try {
        var doc = new HtmlWeb().Load( string.Format( url, word ) );
        var meaningPage = TrySimple( doc );

        if ( meaningPage == null || meaningPage.Descendants( "td" ).Count() < 2 ) {
          //var listPage = doc.DocumentNode.Descendants("div").FirstOrDefault( div => div.HasClass( "wordlist" ) );
          //if ( listPage == null || listPage.Descendants( "td" ).Count() < 2 ) {
          Logger.Log( $"Manual: Getting meaning for {word} failed. Reference: Wordsmyth." );
          return meaning;
          //}

          //doc = web.Load( listPage.Descendants( "td" ).ToList()[0].Descendants("a").First().Attributes["href"].Value );
          //meaningPage = TrySimple( doc );
        }

        meaning = meaningPage.Descendants( "td" ).ToList()[1].InnerText;
        if ( meaning.Contains( '.' ) )
          meaning = meaning.Substring( 0, meaning.IndexOf( '.' ) );
        meaning = meaning.Replace( "&nbsp;", string.Empty );
        meaning = meaning.Trim( ' ', '\n' );
      }
      catch ( Exception ex ) {
        Logger.Log( $"Getting meaning for {word} failed. Reference: Wordsmyth." );
      }
      
      return meaning;
    }

    private HtmlNode TrySimple(HtmlDocument htmlDoc ) {
      var result = htmlDoc.DocumentNode.Descendants( "tr" ).ToList();
      return result.FirstOrDefault( tr => tr.HasClass( "definition" ) );
    }

    public WordBase GetRelatedWords( string word ) {
      var doc = new HtmlWeb().Load( string.Format( url, word ) );
      var allTable = doc.DocumentNode.Descendants( "table" ).FirstOrDefault( table => table.HasClass( "wordexplorer" ) );
      if ( allTable == null ) return null;
      var allWords = allTable.Descendants( "tr" ).Where( item => item.HasClass( "hidden" ) || item.HasClass( "shown" ) );
      var allWordtexts = new List<string>();
      foreach ( var htmlNode in allWords ) {
        allWordtexts.AddRange( htmlNode.Descendants( "a" ).Select( item => item.InnerText ) );
      }
      var result = new WordBase();
      result.Populate( new[] { word }, allWordtexts.Distinct().ToArray() );
      return result;
    }
  }
}
