namespace ConsoleApp1.Models {
  public class Pattern {
    public int StartIndex { get; set; }
    public int EndIndex { get; set; }
    public int Length => EndIndex - StartIndex + 1;
  }
}
