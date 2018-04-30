using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGraph {
  public class MemoryFragment : Entity {
    public List<Entity> Graphs { set; get; }

    public MemoryFragment() {
      Type = EntityType.Fragment;
    }
    public MemoryFragment( string name ) : base( name, EntityType.Fragment ) { }
    public MemoryFragment( string name, string rawInput ) : base( name, EntityType.Fragment, rawInput ) { }

    public override Entity Instance( bool includeParameters = false ) {
      var newObject = (MemoryFragment)base.Instance( includeParameters );

      if ( includeParameters ) {
        newObject.Graphs = InstanceList( Graphs, true );
      }

      return newObject; 
    }
  }
}
