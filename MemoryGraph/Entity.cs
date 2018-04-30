using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGraph {
  public class Entity {
    public string Name { get; set; }

    public string RawInput { get; set; }

    public EntityType Type { get; set; }

    public List<Entity> Entities { get; set; }

    public List<Entity> Place { get; set; }

    public List<Entity> Time { get; set; }

    public List<Entity> Quality { get; set; }

    public List<Entity> Quantity { get; set; }

    public List<Entity> State { get; set; }

    public List<Entity> Action { get; set; }

    public List<Entity> Affection { get; set; }

    public List<Entity> Has { get; set; }

    public List<Entity> Using { get; set; }

    public Entity() {
      Type = EntityType.Entity;
    }

    public Entity( string name ) {
      Type = EntityType.Entity;
      Name = name;
    }

    public Entity( string name, EntityType type ) {
      Type = type;
      Name = name;
    }

    public Entity( string name, EntityType type, string rawInput ) {
      Type = type;
      Name = name;
      RawInput = rawInput;
    }

    public virtual Entity Instance( bool includeParameters = false ) {
      var newObject = new Entity { Name = Name, RawInput = RawInput, Type = Type };
      newObject.Entities = InstanceList( Entities, includeParameters );
      

      if ( includeParameters ) {
        newObject.Action = InstanceList( Action, true );
        newObject.Affection = InstanceList( Affection, true );
        newObject.Has = InstanceList( Has, true );
        newObject.Place = InstanceList( Place, true );
        newObject.Quality = InstanceList( Quality, true );
        newObject.Quantity = InstanceList( Quantity, true );
        newObject.State = InstanceList( State, true );
        newObject.Time = InstanceList( Time, true );
        newObject.Using = InstanceList( Using, true );
      }

      return newObject;
    }

    internal List<Entity> InstanceList(List<Entity> input, bool includeParameters ) {
      List<Entity> output = null;

      if ( input != null && input.Count > 0 ) {
        output = new List<Entity>();
        foreach ( var entity in input) {
          output.Add( entity.Instance( includeParameters ) );
        }
      }

      return output;
    }

    public void Merge( Entity entity ) {
      if ( Type != entity.Type )
        throw new Exception( "types does not match" );

      throw new NotImplementedException();
    }

    public override bool Equals( object obj ) {
      if ( obj is Entity entity
        && Name.Equals( entity.Name, StringComparison.InvariantCultureIgnoreCase )
        && Type.Equals( entity.Type ) )
        return true;

      return false;
    }

  }
}
