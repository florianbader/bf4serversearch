using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rio.Battlefield4.ServerSearch.Core.Entities
{
    public class SearchResults : List<SearchResult>
    {
        public void Add(int weight, Server server)
        {
            this.Add(new SearchResult(weight, server));
        }
    }
}
