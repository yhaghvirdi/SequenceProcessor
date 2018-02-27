using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.Models {
  public class Link: IComparable {
    public Entity Left { get; set; }
    public Entity Right { get; set; }
    public List<Pattern> Pattern { get; set; }

    public int CompareTo( object obj ) {
      if ( !( obj is Link link ) )
        throw new NotImplementedException();

      var currentMaxPattern = this.Pattern.OrderByDescending( item => item.Length ).FirstOrDefault();
      var nextMaxPattern = link.Pattern.OrderByDescending( item => item.Length ).FirstOrDefault();
      return currentMaxPattern?.Length.CompareTo( nextMaxPattern?.Length ) * -1 ?? 1;
    }
  }
}
