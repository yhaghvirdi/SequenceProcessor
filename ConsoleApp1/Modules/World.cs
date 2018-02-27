using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using ConsoleApp1.Models;
using ConsoleApp1.WordNetInterpreter;

namespace ConsoleApp1.Modules {
  public static class World {
    public static List<Entity> Data = new List<Entity>();

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

    public static Entity GetOrCreate( string word, int getMeaningInvardsUpToLayer = 0 ) {
      //try to get entity from world
      var entity = Data.FirstOrDefault( item => item.Name.Equals( word ) );
      if ( entity != null ) return entity;

      //create entity
      entity = new Entity( word );

      //if need to get the meaning
      if ( getMeaningInvardsUpToLayer > 0 ) {
        var meaning = WordNet.GetWordMeaning( word );

        if ( !string.IsNullOrEmpty( meaning ) ) {
          var sentenceWords = TextProcessor.GetWords( meaning );
          foreach ( var sentenceWord in sentenceWords ) {
            //get or create entity for word
            var sentenceWordEntity = GetOrCreate( sentenceWord, getMeaningInvardsUpToLayer - 1 );
            //link word to it
            sentenceWordEntity.CreateLink( entity, LinkSeverity.Weak );
          }
        }
      }

      Data.Add( entity );
      return entity;
    }
  }
}
