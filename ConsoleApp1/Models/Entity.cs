using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ConsoleApp1.Modules;

namespace ConsoleApp1.Models {
  public class Entity {
    public BitArray Sequence { get; set; }
    public string Name { get; set; }

    public Entity( string name, bool generateRandomSequence = true ) {
      Name = name;
      Sequence = generateRandomSequence
        ? SequenceProcessor.PopulateBitArray( Settings.SequenceLength )
        : new BitArray( Settings.SequenceLength );
    }

    public Link CreateLink( Entity to, LinkSeverity severity = LinkSeverity.Weak ) {
      var randomGenerator = new Random( DateTime.Now.Millisecond );
      var linkSize = GetLinkSize( randomGenerator, severity );
      var startIndex = randomGenerator.Next( Settings.SequenceLength - linkSize - 1 );

      for ( var i = startIndex; i < startIndex + linkSize; i++ ) {
        Sequence.Set( i, to.Sequence.Get( i ) );
      }

      return new Link {
        Left = this,
        Right = to,
        Pattern = SequenceProcessor.CalculateSimillarPatterns( this.Sequence, to.Sequence, Settings.MinimumPatternLength )
      };
    }

    public Link StrengthenLink( Entity entity, List<Pattern> currentPatterns, LinkSeverity severity = LinkSeverity.Weak ) {
      var strongestPattern = currentPatterns.OrderByDescending( item => item.Length ).FirstOrDefault();
      if ( strongestPattern == null ) {
        return CreateLink( entity, severity );
      }

      var randomGenerator = new Random( DateTime.Now.Millisecond );
      var linkSize = GetLinkSize( randomGenerator, severity, LinkApplyType.Strengthen );
      var startIndex = strongestPattern.EndIndex + linkSize <= Settings.SequenceLength - 1 ? strongestPattern.EndIndex + 1 : strongestPattern.StartIndex - linkSize;

      for ( var i = startIndex; i < startIndex + linkSize; i++ ) {
        Sequence.Set( i, entity.Sequence.Get( i ) );
      }

      return new Link {
        Left = this,
        Right = entity,
        Pattern = SequenceProcessor.CalculateSimillarPatterns( this.Sequence, entity.Sequence, Settings.MinimumPatternLength )
      };
    }

    private static int GetLinkSize( Random r, LinkSeverity severity, LinkApplyType applyType = LinkApplyType.FirstTime ) {
      var minSize = applyType == LinkApplyType.FirstTime ? Settings.LinkMinimumSize : Settings.LinkStregtheningMinimumSize;
      var medSize = applyType == LinkApplyType.FirstTime ? Settings.LinkMediumSize : Settings.LinkStregtheningMediumSize;
      var maxSize = applyType == LinkApplyType.FirstTime ? Settings.LinkMaximumSize : Settings.LinkStregtheningMaximumSize;

      switch ( severity ) {
        case LinkSeverity.Weak:
          return r.Next( minSize, medSize );
        case LinkSeverity.Strong:
          return r.Next( medSize, maxSize );
        default:
          throw new ArgumentOutOfRangeException( nameof( severity ), severity, null );
      }
    }

    public List<Link> GetStrongLinks( List<Link> list, int minStrength ) {
      return World.FindLinks( this ).Where( item => item.Strength > minStrength ).ToList();
    }

    internal void LoadFromDatabase() {
      var response = DatabaseConnector.PatternDatabase.Read( Name );
      if ( response.Any() ) {
        this.Sequence = new BitArray( response.First().Pattern1 );
      }
    }

    internal void SaveToDatabase() {
      DatabaseConnector.PatternDatabase.AddOrUpdate( new DatabaseConnector.Pattern( Name, Sequence ) );
    }

  }
}
