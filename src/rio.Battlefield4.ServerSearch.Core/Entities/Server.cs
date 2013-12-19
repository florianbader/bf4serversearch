using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rio.Battlefield4.ServerSearch.Core.Entities
{
    public class Server
    {
        public string Name { get; set; }

        public Guid Guid { get; set; }

        public List<Map> Maps { get; set; }

        public Server()
        {
            Maps = new List<Map>();
        }

        public override string ToString()
        {
            return string.Format("{0}, {1} maps",
                Name,
                Maps.Count);
        }

        protected bool Equals(Server other)
        {
            return Guid.Equals(other.Guid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Server) obj);
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        public static bool operator ==(Server left, Server right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Server left, Server right)
        {
            return !Equals(left, right);
        }
    }
}
