using System;

using ConsoleApp1.WordBaseInterpreter;

namespace ConsoleApp1.WordsAndMeanings {
  public interface IWordsMaster {
    string GetWordDefinition( string word );
    WordBase GetRelatedWords( string word );
  }
}
