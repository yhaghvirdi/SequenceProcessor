using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using ConsoleApp1.Models;

namespace ConsoleApp1.Modules {
  public static class SequenceProcessor {

    /// <summary>
    /// make a random bitArray
    /// </summary>
    /// <param name="arrayLength">should be divide-alble by 32</param>
    /// <returns></returns>
    public static BitArray PopulateBitArray( int arrayLength ) {
      //length of array in characters
      arrayLength = arrayLength / 8;
      //length of array in sequences of 32char GUIDs
      arrayLength = arrayLength / 32;

      var str = string.Empty;
      for ( var i = 0; i < arrayLength; i++ ) {
        str += Guid.NewGuid().ToString().Replace( "-", "" );
      }

      var bytes = Encoding.ASCII.GetBytes( str );
      var output = new BitArray( bytes );

      if ( Settings.DebugLogs )
        Logger.PrintBitArray( output );

      return output;
    }

    public static List<Pattern> CalculateSimillarPatterns( BitArray array1, BitArray array2, int minimumPatternLength ) {
      var comparedBitSequence = XNOR( array1, array2 );
      var extractedPatterns = new List<Pattern>();
      var sequenceLength = 0;

      for ( var i = 0; i < comparedBitSequence.Length; i++ ) {
        if ( !comparedBitSequence[i] || i == comparedBitSequence.Length - 1 ) {
          if ( sequenceLength >= minimumPatternLength ) {
            extractedPatterns.Add( new Pattern { StartIndex = i - sequenceLength, EndIndex = i - 1 } );
          }
          sequenceLength = 0;
          continue;
        }
        sequenceLength++;
      }

      if ( Settings.DebugLogs )
        Logger.PrintPattern( extractedPatterns );

      return extractedPatterns;
    }

    public static BitArray XNOR( BitArray arr1, BitArray arr2 ) {
      //AND operation
      var arr1Clone = (BitArray) arr1.Clone();
      var arr2Clone = (BitArray) arr2.Clone();

      var arr3 = (BitArray) arr1.Clone();
      arr3.And( arr2 );

      //NOR operation
      arr1Clone = arr1Clone.Not().And( arr2Clone.Not() );

      //XNOR output
      arr1Clone.Or( arr3 );
      return arr1Clone;
    }

  }
}
