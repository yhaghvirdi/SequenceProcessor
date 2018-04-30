using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1 {
  public static class Settings {
    public static int SequenceLength => 1024;
    public static int MinimumPatternLength => 50;
    public static int LinkMinimumSize => 48;
    public static int LinkMediumSize => 64;
    public static int LinkMaximumSize => 80;

    public static int LinkStregtheningMinimumSize => 10;
    public static int LinkStregtheningMediumSize => 13;
    public static int LinkStregtheningMaximumSize => 16;

    public static bool DebugLogs => true;
    public static bool Level2Logs => true;

    public static int Level2LogEveryNObjects => 50;

    public static int DatabaseReadPageSize => 20;


    public static int MeaningCloseThreshold => LinkMaximumSize + LinkMinimumSize / 2;
    public static int MeaningExactThreshold => MeaningCloseThreshold * 2;

    
  }
}
