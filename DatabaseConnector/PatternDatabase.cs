using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnector {
  public class PatternDatabase {
    private static PatternsTest1Entities db;

    public static void Initialize() {
      db = new PatternsTest1Entities();
    }

    public static List<Pattern> ReadAll( int pageNumber, int pageSize ) {
      var list = from b in db.Patterns
        orderby b.Id
        select b;

      return list.Skip( (pageNumber - 1) * pageSize ).Take( pageSize ).ToList();
    }

    public static List<Pattern> Read( string paternName ) {
      var list = from b in db.Patterns
                 where b.Name == paternName
                 orderby b.Name
                 select b;

      return list.ToList();
    }

    public static void Insert( Pattern pattern ) {
      pattern.CreateDate = DateTime.Now;
      pattern.LastUpdateDate = DateTime.Now;
      db.Patterns.Add( pattern );
      db.SaveChanges();
    }

    public static void Update( Pattern pattern ) {
      var oldOne = Read( pattern.Name ).FirstOrDefault();
      if ( oldOne == null ) return;
      oldOne.LastUpdateDate = DateTime.Now;
      oldOne.Pattern1 = pattern.Pattern1;
      db.Patterns.AddOrUpdate( oldOne );
      db.SaveChanges();
    }

    public static void AddOrUpdate( Pattern pattern ) {
      var oldOne = Read( pattern.Name ).FirstOrDefault();
      if ( oldOne != null ) {
        pattern.Id = oldOne.Id;
        pattern.CreateDate = oldOne.CreateDate;
        pattern.LastUpdateDate = DateTime.Now;
      }
      else {
        pattern.CreateDate = DateTime.Now;
        pattern.LastUpdateDate = DateTime.Now;
      }

      db.Patterns.AddOrUpdate( pattern );
      db.SaveChanges();
    }
  }
}
