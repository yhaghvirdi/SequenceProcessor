using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConsoleApp1.Sequence;

namespace ConsoleApp1 {
  internal class Logger {
    public static void Log( string str ) {
      var temp = Console.ForegroundColor;
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine( str );
      Console.ForegroundColor = temp;
    }

    public static void PrintBitArray( BitArray arr1 ) {
      foreach ( bool o in arr1 ) {
        Console.Write( o ? 1 : 0 );
      }
      Console.WriteLine();
    }

    public static void PrintPattern(List<Pattern> patterns ) {
      Console.ForegroundColor = ConsoleColor.Yellow;
      if ( patterns.Any() ) {
        foreach ( var pattern in patterns ) {
          Console.WriteLine( $"Pattern Length:{pattern.Length}, Start:{pattern.StartIndex}" );
        }
      }
      else {
        Console.WriteLine( "No Patterns" );
      }
      Console.ResetColor();
    }

  }
}
