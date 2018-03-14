using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConsoleApp1.Modules;

namespace ConsoleApp1 {
  public static class CommandProcessor {
    public static void ApplyCommand( string[] commandWords ) {
      switch ( commandWords.First().ToLower() ) {
        case "overall":
          Logger.Log( $"Number of entities: {World.Data.Count}" );
          break;
        //mutual links between two or more entities
        case "mutual":
          HandleMutualCommand();
          break;
        //entities with more than X number of links (most related entites)
        case "roots":
          HandleRoots();
          break;
        case "resetlinkdump":
          World.LinksDumpList = null;
          break;
        default:
          Logger.Log( "Command not found" );
          break;
      }
    }

    private static void HandleRoots() {
      Console.Write( "Whats the min number of links?" );
      var minNumberOfLinks = Console.ReadLine();
      if ( !int.TryParse( minNumberOfLinks, out var parsedMinNumberOfLinks ) ) {
        Logger.Log( "Command not found" );
      }
      else {
        var result = World.AllEntitiesWithMoreThanXLinks( parsedMinNumberOfLinks );
        foreach ( var entity in result ) {
          Console.ForegroundColor = ConsoleColor.Green;
          Console.WriteLine( entity.Name );
          Console.ResetColor();
        }
      }
    }

    private static void HandleMutualCommand() {
      Console.Write( "A/B (All or two specific entities)" );
      var mutualType = Console.ReadLine();
      switch ( mutualType ) {
        case "A":
          Console.Write( "What shold be the mimimum of mutual links?" );
          var minMutualLinks = Console.ReadLine();
          if ( !int.TryParse( minMutualLinks, out var parsedMinMutualLinks ) ) {
            Logger.Log( "Command not found" );
            break;
          }
          var entityResult = World.ExtractAllEntityPairsWithMutualLinks( parsedMinMutualLinks );

          Console.ForegroundColor = ConsoleColor.Green;
          foreach ( var tuple in entityResult ) {
            Console.WriteLine( $"{tuple.Item1.Name}, {tuple.Item2.Name}" );
          }
          Console.ResetColor();

          break;

        case "B":
          Console.Write( "Insert two entities (separated with comma)" );
          var entities = Console.ReadLine();
          if ( entities == null || entities.Split( ',' ).Length != 2 ) {
            Logger.Log( "Command not found" );
            break;
          }
          var twoEntites = entities.Split( ',' );
          var firstOne = World.GetOrCreate( twoEntites.First().Trim( ' ' ) );
          var secondOne = World.GetOrCreate( twoEntites.Last().Trim( ' ' ) );
          var mutuals = firstOne.ExtractMutualLinks( secondOne );

          Console.ForegroundColor = ConsoleColor.Green;
          foreach ( var mutual in mutuals ) {
            Console.WriteLine( $"{mutual.Right.Name}" );
          }
          Console.ResetColor();

          break;

        default:
          Logger.Log( "Command not found" );
          break;
      }
    }
  }
}
