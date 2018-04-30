using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConsoleApp1.Models;
using ConsoleApp1.Modules;

namespace ConsoleApp1 {
  public enum CommandType {
    EntitySearch,
    AddLink,
    MeaningSearch,
    AddMeaning,
    RefreshMeaning,
    Overall,
    Mutual,
    Roots,
    ResetLinkDump
  }

  public static class CommandProcessor {
    private static void NotFound() {
      Console.WriteLine( "Not found." );
    }

    public static void ProcessInput( string input ) {
      switch ( TextProcessor.FindIntent( input ) ) {
        case IntentType.SearchLinks:
          HandleSearchLinks( input );
          break;

        case IntentType.SearchMeanings:
          //HandleSearchMeaning( input );
          break;

        case IntentType.AddDefinition:
          HandleAddDefinitionAndMeaning( input, false );
          break;

        case IntentType.AddMeaning:
          HandleAddDefinitionAndMeaning( input, true );
          break;

        case IntentType.Command:
          var command = TextProcessor.GetWords( input );
          ApplyCommand( command );
          break;

        case IntentType.NotSure:
          Console.WriteLine( "Not sure what you mean." );
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private static void ApplyCommand( string[] commandWords ) {
      switch ( commandWords.First().ToLower() ) {
        case "overall":
          Logger.Log( $"Number of entities: {World.Data.Count}" );
          break;
        //refresh a word's meaning from wordsMaster
        case "refreshmeaning":
          HandleRefreshMeaning();
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


    private static void HandleRefreshMeaning() {
      Console.Write( "What entity's meaning do you want to refetch? " );
      var word = Console.ReadLine();
      if ( string.IsNullOrEmpty( word ) )
        NotFound();
      World.GetOrCreate( word, 1, true, true );
      Console.WriteLine( $"Meaning for {word} refetched" );
    }

    private static void HandleAddDefinitionAndMeaning( string input, bool shouldAddAsWisdom ) {
      var words = TextProcessor.GetWords( input );
      var meaning = string.Join( " ", words.Skip( 1 ) );
      var entity = World.GetOrCreate( words[0], 1 );
      World.AddDefinition( entity, meaning, shouldAddAsWisdom: shouldAddAsWisdom );
    }

    private static void HandleSearchLinks( string input ) {
      var searchedEntity = World.GetOrCreate( input, 1 );
      if ( searchedEntity == null ) {
        NotFound();
        return;
      }

      var links = World.FindLinks( searchedEntity );
      foreach ( var link in links ) {
        Console.WriteLine( $"{link.Right.Name}" );
        Logger.PrintPattern( link.Pattern );
      }
    }

    //private static void HandleSearchMeaning( string input ) {
    //  input = TextProcessor.GetSubSentence( input, 2 );
    //  var splitted = input.Split( ',' );
    //  var entityList = new List<Entity>();

    //  foreach ( var word in splitted ) {
    //    entityList.Add( World.GetOrCreate( word ) );
    //  }

    //  var meanings = World.FindMeanings( entityList );
    //  var index = 0;
    //  foreach ( var entityMeaning in meanings ) {
    //    switch ( entityMeaning.GetMeaningType() ) {
    //      case Meaningtype.NotSure:
    //        Console.WriteLine( $"{index++}: Not sure what it means" );
    //        break;
    //      case Meaningtype.Far:
    //        var realMeaning = World.FindEntityForMeaning( entityMeaning.Right );
    //        if ( realMeaning == null ) {
    //          Console.WriteLine( $"{index++}: Maybe {entityMeaning.Right.Name }" );
    //          break;
    //        }
    //        Console.WriteLine( $"{index++}: I Know {realMeaning.Left.Name} is {entityMeaning.Right.Name}" );
    //        break;
    //      case Meaningtype.Close:
    //        Console.WriteLine( $"{index++}: Probably {entityMeaning.Right.Name}" );
    //        break;
    //      case Meaningtype.Exact:
    //        Console.WriteLine( $"{index++}: {entityMeaning.Left.Name} is {entityMeaning.Right.Name}" );
    //        break;
    //      default:
    //        throw new ArgumentOutOfRangeException();
    //    }
    //    Logger.PrintPattern( entityMeaning.Pattern );
    //  }

    //  //no meanings
    //  if ( !meanings.Any() ) {
    //    Console.WriteLine( "Not sure what it means." );
    //  }
    //  //more than one meaning
    //  else if ( meanings.Count > 1 ) {
    //    Console.Write( "Which one do you think is right? " );
    //    var nextInput = Console.ReadLine();
    //    if ( int.TryParse( nextInput, out var parsedNextInput ) ) {
    //      //if link already strong, no need to strengthen
    //      if ( meanings[parsedNextInput].GetMeaningType() == Meaningtype.Exact ) {
    //        Console.WriteLine( "Link already too strong" );
    //        return;
    //      }

    //      //strengthen and report
    //      var updatedLink = meanings[parsedNextInput].Right.StrengthenLink(
    //        searchedEntityMeaning,
    //        meanings[parsedNextInput].Pattern,
    //        LinkSeverity.Strong );
    //      Console.WriteLine( "Link strengthened." );
    //      Logger.PrintPattern( updatedLink.Pattern );
    //    }
    //  }
    //}

    //Objects with more than X links
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

    //All entity pairs or a specific entity pair having mutual entities
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
