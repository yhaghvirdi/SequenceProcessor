using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApp1.Sequence {
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
      Console.WriteLine( str );
      var bytes = Encoding.ASCII.GetBytes( str );
      var output = new BitArray( bytes );

#if DEBUG
      Logger.PrintBitArray( output );
#endif
      return output;
    }

    public static List<Pattern> CalculateSimillarPatterns( BitArray array1, BitArray array2, int minimumPatternLength ) {
      var comparedBitSequence = XNOR( array1, array2 );
      var extractedPatterns = new List<Pattern>();
      var sequenceLength = 0;

      for ( var i = 0; i < comparedBitSequence.Length; i++ ) {
        if ( comparedBitSequence[i] ) {
          sequenceLength++;
        }
        else {
          if ( sequenceLength >= minimumPatternLength) {
            extractedPatterns.Add( new Pattern { StartIndex = i - sequenceLength, EndIndex = i - 1 } );
          }
          sequenceLength = 0;
        }
      }

#if DEBUG
      Logger.PrintPattern( extractedPatterns );
#endif
      return extractedPatterns;
    }

    public static void PopulateBitArray( Random r, int arrayLength, BitArray arr1 ) {
      var c = r.Next( arrayLength - 1 );
      for ( var i = 0; i < c; i++ ) {
        arr1[r.Next( arrayLength - 1 )] = true;
      }
    }

    public static BitArray XNOR( BitArray arr1, BitArray arr2 ) {
      //AND operation
      var arr3 = (BitArray) arr1.Clone();
      arr3.And( arr2 );

      //NOR operation
      var arr4 = arr1.Not().And( arr2.Not() );

      //XNOR output
      arr4.Or( arr3 );
      return arr4;
    }

    public static int ReturnMaximumPatternLength( BitArray comparedBitSequence ) {
      var globalLongestSequence = 0;
      var localLongestSequence = 0;
      for ( var i = 0; i < comparedBitSequence.Length; i++ ) {
        if ( comparedBitSequence[i] ) {
          localLongestSequence++;
        }
        else {
          if ( localLongestSequence > globalLongestSequence ) {
            globalLongestSequence = localLongestSequence;
          }
          localLongestSequence = 0;
        }
      }

      return globalLongestSequence;
    }
  }
}
