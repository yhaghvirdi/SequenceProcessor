using System;
using System.Collections;

namespace ConsoleApp1.Link {
  public static class LinkProcessor {
    public static void Link(this BitArray host, BitArray from, LinkSeverity severity ) {
      var randomGenerator = new Random( DateTime.Now.Millisecond );
      var linkSize = GetLinkSize( randomGenerator, severity );
      var startIndex = randomGenerator.Next( Settings.SequenceLength - linkSize - 1 );
      
      for ( var i = startIndex; i < startIndex + linkSize; i++ ) {
        host.Set( i, from.Get( i ) );
      }
    }

    private static int GetLinkSize( Random r, LinkSeverity severity ) {
      switch ( severity ) {
        case LinkSeverity.Weak:
          return r.Next( Settings.LinkMinimumSize, Settings.LinkMediumsize );
        case LinkSeverity.Strong:
          return r.Next( Settings.LinkMediumsize, Settings.LinkMaximumSize );
        default:
          throw new ArgumentOutOfRangeException( nameof(severity), severity, null );
      }
    }
  }
}
