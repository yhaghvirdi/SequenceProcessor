using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Sequence {
  public class Pattern {
    public int StartIndex { get; set; }
    public int EndIndex { get; set; }
    public int Length => EndIndex - StartIndex + 1;
  }
}
