using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rio.Battlefield4.ServerSearch.Core.Entities
{
    public class SearchResult
    {
        public SearchResult(int weight, Server server)
        {
            Weight = weight;
            Server = server;
        }

        public int Weight { get; set; }

        public Server Server { get; set; }

        public override string ToString()
        {
            return string.Format("Weight: {0}, {1}",
                Weight,
                Server.Name);
        }
    }
}
