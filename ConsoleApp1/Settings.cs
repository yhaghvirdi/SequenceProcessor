using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1 {
  public static class Settings {
    public static int SequenceLength => 512;
    public static int SequenceLengthInChar => SequenceLength / 16;
    public static int MinimumPatternLength => 32;
    public static int LinkMinimumSize => 32;
    public static int LinkMediumsize => 48;
    public static int LinkMaximumSize => 64;
  }
}
