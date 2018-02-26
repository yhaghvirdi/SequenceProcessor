using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using ConsoleApp1.Link;
using ConsoleApp1.Sequence;

namespace ConsoleApp1 {
  class Program {
    static void Main( string[] args ) {

      ////8192 = 512 character x 16bit character size
      ////english contains of a quarter million distinct words: 250,000
      //testperformance( 500000, 1024 );

      while ( true ) {
        var arr1 = SequenceProcessor.PopulateBitArray( Settings.SequenceLength );
        var arr2 = SequenceProcessor.PopulateBitArray( Settings.SequenceLength );

        arr1.Link( arr2, LinkSeverity.Weak );
        var patterns = SequenceProcessor.CalculateSimillarPatterns( arr1, arr2, Settings.MinimumPatternLength );
        
        Console.ReadLine();
      }

      //var r = new Random();
      //const int arrayLength = 128;
      //var arr1 = new BitArray( arrayLength );
      //var arr2 = new BitArray( arrayLength );

      //while ( true ) {

      //  SequenceProcessor.PopulateBitArray( r, arrayLength, arr1 );
      //  SequenceProcessor.PopulateBitArray( r, arrayLength, arr2 );
      //  SequenceProcessor.PrintBitArray( arr1 );
      //  SequenceProcessor.PrintBitArray( arr2 );

      //  var arr4 = SequenceProcessor.XNOR( arr1, arr2 );

      //  SequenceProcessor.PrintBitArray( arr4 );

      //  Console.ForegroundColor = ConsoleColor.Yellow;
      //  Console.WriteLine( SequenceProcessor.ReturnMaximumPatternLength( arr4 ) );
      //  Console.ResetColor();
      //  arr1.SetAll( false );
      //  arr2.SetAll( false );

      //  Console.ReadLine();
      //}
    }

    private static void testperformance(int iterationCount, int arrayLength) {
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

      for(var i=0; i<iterationCount;i++ ) {
        var arr4 = SequenceProcessor.XNOR( testLists1[i], testLists2[i] );
        SequenceProcessor.ReturnMaximumPatternLength( arr4 );
      }

      time.Stop();
      Console.WriteLine(time.Elapsed);
      Console.WriteLine(time.ElapsedMilliseconds);
      Console.ReadLine();
    }
  }
}
