using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using ConsoleApp1.Models;
using ConsoleApp1.WordBaseInterpreter;
using ConsoleApp1.WordsAndMeanings;

using DatabaseConnector;

namespace ConsoleApp1.Modules {
  public static class World {
    public static List<Entity> Data = new List<Entity>();
    public static List<Entity> Wisdom = new List<Entity>();

    public static IWordsMaster _wordsMaster;
    public static List<List<Link>> LinksDumpList { get; set; }

    public static void Initialize( IWordsMaster wordsMaster ) {
      _wordsMaster = wordsMaster;
    }

    public static List<Link> FindLinks( Entity entity ) {
      var result = new List<Link>();
      foreach ( var worldEntity in Data ) {
        if ( worldEntity.Name.Equals( entity.Name ) ) continue;
        var patterns = SequenceProcessor.CalculateSimillarPatterns( entity.Sequence, worldEntity.Sequence, Settings.MinimumPatternLength );
        if ( !patterns.Any() ) continue;
        result.Add( new Link { Left = entity, Right = worldEntity, Pattern = patterns } );
      }
      result.Sort();
      return result;
    }

    public static List<Meaning> FindMeanings( Entity entity ) {
      var result = new List<Meaning>();
      foreach ( var worldEntity in Wisdom ) {
        var patterns = SequenceProcessor.CalculateSimillarPatterns( entity.Sequence, worldEntity.Sequence, Settings.MinimumPatternLength );
        if ( !patterns.Any() ) continue;
        result.Add( new Meaning { Left = entity, Right = worldEntity, Pattern = patterns } );
      }
      result.Sort();
      return result;
    }

    public static Meaning FindEntityForMeaning( Entity meaning ) {
      var result = new List<Meaning>();
      foreach ( var worldEntity in Data ) {
        var patterns = SequenceProcessor.CalculateSimillarPatterns( meaning.Sequence, worldEntity.Sequence, Settings.MinimumPatternLength );
        if ( !patterns.Any() ) continue;
        result.Add( new Meaning { Left = worldEntity, Right = meaning, Pattern = patterns } );
      }
      result.Sort();
      return result.Any() ? result.First() : null;
    }

    public static Entity GetOrCreate( string word, int getDefinitionInvardsUpToLayer = 0, bool addWisdom = false, bool refreshMeaning = false ) {
      //try to get entity from world
      var entity = Data.FirstOrDefault( item => item.Name.Equals( word ) );
      if ( entity != null && !refreshMeaning ) return entity;

      //create entity if not existed
      if ( entity == null ) {
        entity = new Entity( word );
        Data.Add( entity );
      }

      if ( Data.Count % Settings.Level2LogEveryNObjects == 0 ) {
        Logger.Level2Log( $"Number of entities reached {Data.Count}" );
      }

      //if need to get the definition
      if ( getDefinitionInvardsUpToLayer > 0 ) {
        var definition = string.Empty;

        try {
          definition = _wordsMaster.GetWordDefinition( word );
        }
        catch ( NullReferenceException ) {
          Logger.Log( "World not initialized." );
        }

        if ( !string.IsNullOrEmpty( definition ) )
          AddDefinition( entity, definition, getDefinitionInvardsUpToLayer - 1, LinkSeverity.Weak, addWisdom );
      }

      var relatedWords = _wordsMaster.GetRelatedWords( word );
      if(relatedWords != null && relatedWords.Definitions.Any() ) {
        ConsumeWordBase( relatedWords );
      }

      return entity;
    }

    public static void AddDefinition( Entity entity, string definition, int getDefinitionInvardsUpToLayer = 0, LinkSeverity severity = LinkSeverity.Weak, bool shouldAddAsWisdom = false ) {
      var sentenceWords = TextProcessor.GetWords( definition );
      foreach ( var sentenceWord in sentenceWords ) {
        if ( string.IsNullOrEmpty( sentenceWord ) ) continue;

        //get or create entity for word
        var sentenceWordEntity = GetOrCreate( sentenceWord, getDefinitionInvardsUpToLayer - 1 );

        //calculate if a link between entities already exists
        var currentPatterns = SequenceProcessor.CalculateSimillarPatterns( entity.Sequence, sentenceWordEntity.Sequence, Settings.MinimumPatternLength );
        //if link already exists, strengthen 
        if ( currentPatterns.Any() )
          sentenceWordEntity.StrengthenLink( entity, currentPatterns );
        //else link word to it
        else
          sentenceWordEntity.CreateLink( entity, severity );
      }

      if ( shouldAddAsWisdom )
        Wisdom.Add( new Entity( definition, false ) { Sequence = (BitArray) entity.Sequence.Clone() } );
    }

    public static void ConsumeWordBase( WordBase wordbase, int getDefinitionInvardsUpToLayer = 0, bool addAsWisdom = false ) {
      foreach ( var word in wordbase.Words ) {
        if ( Program.EscapePressed || ( Console.KeyAvailable && Console.ReadKey( true ).Key == ConsoleKey.Escape ) ) {
          Program.EscapePressed = true;
          break;
        }

        foreach ( var separatedWord in word.Split( ' ' ) ) {
          if ( string.IsNullOrEmpty( separatedWord ) ) continue;
          var wordEntity = GetOrCreate( separatedWord, getDefinitionInvardsUpToLayer, addAsWisdom );

          if ( wordbase.Definitions == null ) continue;
          foreach ( var definition in wordbase.Definitions ) {
            AddDefinition( wordEntity, definition, 0, LinkSeverity.Strong );
          }
        }
      }
    }

    public static void ConsumeDatabase() {
      var index = 1;
      List<DatabaseConnector.Pattern> list;
      do {
        list = PatternDatabase.ReadAll( index, Settings.DatabaseReadPageSize );
        Data.AddRange( list.Select( dbEntity => new Entity( dbEntity.Name, false ) { Sequence = new BitArray( dbEntity.Pattern1 ) } ) );
        index++;
      } while ( list.Count > 0 );
    }

    public static List<List<Link>> GetLinkDumpList( bool reset = false ) {
      if ( reset || LinksDumpList == null ) {
        var allLinks = new List<List<Link>>();
        foreach ( var entity in Data ) {
          if ( Program.EscapePressed || ( Console.KeyAvailable && Console.ReadKey( true ).Key == ConsoleKey.Escape ) ) {
            Program.EscapePressed = true;
            break;
          }

          allLinks.Add( World.FindLinks( entity ) );
        }
        return LinksDumpList = allLinks;
      }
      return LinksDumpList;
    }

    public static List<Entity> AllEntitiesWithMoreThanXLinks( int minNumberOfLinks ) {
      var linkList = GetLinkDumpList();
      var result = linkList.Where( item => item.Count > minNumberOfLinks );
      return result.Select( item => item?.FirstOrDefault()?.Left ).ToList();
    }

    public static List<Tuple<Entity, Entity>> ExtractAllEntityPairsWithMutualLinks( int minNumberOfMutualLinks ) {
      var allLinks = GetLinkDumpList();

      List<Tuple<Entity, Entity>> result = new List<Tuple<Entity, Entity>>();
      for ( var i = 0; i < allLinks.Count - 1; i++ ) {
        for ( int j = i + 1; j < allLinks.Count; j++ ) {
          var mutualLinks = ExtractMutualLinks( allLinks[i], allLinks[j] );
          if ( mutualLinks.Count >= minNumberOfMutualLinks )
            result.Add( new Tuple<Entity, Entity>( allLinks[i]?.First()?.Left, allLinks[j]?.First()?.Left ) );
        }
      }
      return result;
    }

    public static List<Link> ExtractMutualLinks( Entity entity, List<Link> list2 ) {
      return ExtractMutualLinks( World.FindLinks( entity ), list2 );
    }

    public static List<Link> ExtractMutualLinks( this Entity entity1, Entity entity2 ) {
      return ExtractMutualLinks( World.FindLinks( entity1 ), World.FindLinks( entity2 ) );
    }

    private static List<Link> ExtractMutualLinks( List<Link> list1, List<Link> list2 ) {
      var result = new List<Link>();
      foreach ( var link in list1 ) {
        result.AddRange( list2.Where( item => item.Right.Name == link.Right.Name ) );
      }
      return result;
    }

  }
}
