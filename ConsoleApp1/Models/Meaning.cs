using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.Models {
  public class Meaning: Link {

    public Meaningtype GetMeaningType() {
      if ( Pattern == null || !Pattern.Any() ) return Meaningtype.NotSure;
      var lengthSum = Pattern.Sum( item => item.Length );
      if ( lengthSum > Settings.MeaningExactThreshold ) return Meaningtype.Exact;
      if ( lengthSum > Settings.MeaningCloseThreshold ) return Meaningtype.Close;
      return Meaningtype.Far;
    }
  }

  public enum Meaningtype {
    NotSure,
    Far,
    Close,
    Exact
  }
}
