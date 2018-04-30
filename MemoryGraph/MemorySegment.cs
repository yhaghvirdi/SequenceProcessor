using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGraph {
  public class MemorySegment: Entity {
    public List<MemoryFragment> Fragments { get; set; }

    public MemorySegment() {
      Type = EntityType.Segment;
    }
    public MemorySegment( string name ) : base( name, EntityType.Segment ) { }
    public MemorySegment( string name, string rawInput ) : base( name, EntityType.Segment, rawInput ) { }

  }
}
