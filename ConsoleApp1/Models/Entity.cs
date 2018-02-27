using System;
using System.Collections;

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

    public Link CreateLink( Entity to, LinkSeverity severity ) {
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

    private static int GetLinkSize( Random r, LinkSeverity severity ) {
      switch ( severity ) {
        case LinkSeverity.Weak:
          return r.Next( Settings.LinkMinimumSize, Settings.LinkMediumsize );
        case LinkSeverity.Strong:
          return r.Next( Settings.LinkMediumsize, Settings.LinkMaximumSize );
        default:
          throw new ArgumentOutOfRangeException( nameof( severity ), severity, null );
      }
    }
  }
}
