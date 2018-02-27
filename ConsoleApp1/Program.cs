using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

using ConsoleApp1.Models;
using ConsoleApp1.Modules;
using ConsoleApp1.WordNetInterpreter;

namespace ConsoleApp1 {
  class Program {

    static void Main( string[] args ) {


      //foreach ( var word in WordsDump.WordsTest ) {
      //  var meaning = WordNet.GetWordMeaning( word );
      //  if ( string.IsNullOrEmpty( meaning ) ) continue;
      //  //get or create entity for word
      //  var wordEntity = World.GetOrCreate( word );
      //  //split meaning sentence
      //  var sentenceWords = TextProcessor.GetWords( meaning );
      //  //foreach word in sentence
      //  foreach ( var sentenceWord in sentenceWords ) {
      //    //get or create entity for word
      //    var sentenceWordEntity = World.GetOrCreate( sentenceWord );
      //    //link word to it
      //    sentenceWordEntity.CreateLink( wordEntity, LinkSeverity.Weak );
      //  }
      //}

      foreach ( var word in WordsDump.WordsBasic ) {
        World.GetOrCreate( word, 1 );
      }

      while ( true ) {
        Console.Write( "Write a word: " );
        var input = Console.ReadLine();
        var entity = World.GetOrCreate( input );
        if ( entity == null ) {
          Console.WriteLine( "Not found." );
          continue;
        }
        var links = World.FindLinks( entity );
        foreach ( var link in links ) {
          Console.WriteLine($"{link.Right.Name}:" );
          Logger.PrintPattern( link.Pattern );
          Console.WriteLine("--------------------------");
        }

        Console.ReadLine();
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

    private static void testperformance( int iterationCount, int arrayLength ) {
      List<BitArray> testLists1 = new List<BitArray>();
      List<BitArray> testLists2 = new List<BitArray>();

      Random r = new Random( DateTime.Now.Millisecond );
      for ( var i = 0; i < iterationCount; i++ ) {
        var tempArr = new BitArray( arrayLength );
        SequenceProcessor.PopulateBitArray( r, arrayLength, tempArr );
        testLists1.Add( tempArr );
      }
      for ( var i = 0; i < iterationCount; i++ ) {
        var tempArr = new BitArray( arrayLength );
        SequenceProcessor.PopulateBitArray( r, arrayLength, tempArr );
        testLists2.Add( tempArr );
      }

      Stopwatch time = new Stopwatch();
      time.Start();

      for ( var i = 0; i < iterationCount; i++ ) {
        var arr4 = SequenceProcessor.XNOR( testLists1[i], testLists2[i] );
        SequenceProcessor.ReturnMaximumPatternLength( arr4 );
      }

      time.Stop();
      Console.WriteLine( time.Elapsed );
      Console.WriteLine( time.ElapsedMilliseconds );
      Console.ReadLine();
    }
  }
}
