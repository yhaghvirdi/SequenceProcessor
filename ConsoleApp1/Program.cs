using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using ConsoleApp1.Models;
using ConsoleApp1.Modules;
using ConsoleApp1.WordBaseInterpreter;
using ConsoleApp1.WordsAndMeanings;

using DatabaseConnector;

using HtmlAgilityPack;

using WordsDump = ConsoleApp1.WordsAndMeanings.WordsDump;

namespace ConsoleApp1 {
  class Program {

    public static bool EscapePressed = false;

    static void Main( string[] args ) {
      PatternDatabase.Initialize();
      World.Initialize( new KidsEncyclopedia() );

      //var tree = World.GetOrCreate( "tree" );
      //World.AddMeaning( tree, "tall plant" );
      //var tall = World.GetOrCreate( "tall" );
      //var plant = World.GetOrCreate( "plant" );
      //var human = World.GetOrCreate( "human" );
      //var humanLinks = World.FindLinks( human );
      //var treeLinks = World.FindLinks( tree );

      //tree.SaveToDatabase();
      //tall.SaveToDatabase();
      //plant.SaveToDatabase();
      //human.SaveToDatabase();

      //World.ConsumeDatabase();

      //var tree = World.GetOrCreate( "tree" );
      //var treelinks = World.FindLinks( tree );

      Logger.Level2Log( $"Starting Words basic" );
      var wbBasic = new WordBase();
      wbBasic.Populate( WordsDump.WordsBasic );
      World.ConsumeWordBase( wbBasic, 3 );

      Logger.Level2Log( $"Starting Words objects" );
      var wbObjects = new WordBase();
      wbObjects.Populate( WordsDump.WordsObjects );
      World.ConsumeWordBase( wbObjects, 3 );

      var files = Directory.GetFiles( "../../../../Wordbase/" );
      var filesCount = files.Length;
      for ( var i = 0; i < files.Length; i++ ) {
        if ( EscapePressed ) break;
        Logger.Level2Log( $"Starting file number {i}/{filesCount}: {files[i]}" );
        var wb = new WordBase();
        wb.Populate( files[i] );
        World.ConsumeWordBase( wb, 3 );
      }

      World.GetLinkDumpList();

      Logger.Alarm();

      while ( true ) {
        Console.Write( "Write a word: " );
        var input = Console.ReadLine();

        switch ( TextProcessor.FindIntent( input ) ) {
          case IntentType.SearchWord:
            var searchedEntity = World.GetOrCreate( input, 1 );
            if ( searchedEntity == null ) {
              Console.WriteLine( "Not found." );
              continue;
            }
            var links = World.FindLinks( searchedEntity );
            foreach ( var link in links ) {
              Console.WriteLine( $"{link.Right.Name}:" );
              Logger.PrintPattern( link.Pattern );
              Console.WriteLine( "--------------------------" );
            }
            break;

          case IntentType.AddMeaning:
            var words = TextProcessor.GetWords( input );
            var meaning = string.Join( " ", words.Skip( 1 ) );
            var entity = World.GetOrCreate( words[0], 1 );
            World.AddMeaning( entity, meaning );
            break;

          case IntentType.Command:
            var command = TextProcessor.GetWords( input );
            CommandProcessor.ApplyCommand( command );
            break;

          case IntentType.NotSure:
            Console.WriteLine( "Not sure what you mean." );
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    

    private static void Samples() {
      //var wordList = new[] { "tree", "boy", "cup", "society", "brain", "orange", "glue", "left", "interesting", "cut", "crocodile", "bora bora", "salam" };
      //foreach ( var word in wordList ) {
      //  var meaning = WordNet.GetWordMeaning( word );
      //  Console.WriteLine( $"{word}: {meaning}" );
      //}


      ////8192 = 512 character x 16bit character size
      ////english contains of a quarter million distinct words: 250,000
      //testperformance( 500000, 1024 );


      //while ( true ) {
      //  var arr1 = SequenceProcessor.PopulateBitArray( Settings.SequenceLength );
      //  var arr2 = SequenceProcessor.PopulateBitArray( Settings.SequenceLength );

      //  arr1.Link( arr2, LinkSeverity.Weak );
      //  var patterns = SequenceProcessor.CalculateSimillarPatterns( arr1, arr2, Settings.MinimumPatternLength );

      //  Console.ReadLine();
      //}
    }

  }
}
