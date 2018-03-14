using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

using ConsoleApp1.Models;

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
        //Console.WriteLine( "No Patterns" );
      }
      Console.ResetColor();
    }

    public static void PrintLink( int startIndex, int linkSize ) {
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine( $"Link Start: {startIndex}, Size: {linkSize}" );
      Console.ResetColor();
    }

    public static void Alarm() {
      SoundPlayer alarm = new SoundPlayer();
      alarm.SoundLocation = Environment.CurrentDirectory + "\\wader.wav";
      alarm.Play();
    }

    public static IntentType LogIntent( IntentType intentType ) {
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine( $"Intent: {intentType.ToString()}" );
      Console.ResetColor();
      return intentType;
    }

    public static void Level2Log( string text ) {
      if ( !Settings.Level2Logs ) return;
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine($"{DateTime.Now:HH:mm:ss.ff}: {text}");
      Console.ResetColor();
    }
  }
}
