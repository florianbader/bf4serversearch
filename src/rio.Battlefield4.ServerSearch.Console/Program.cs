using System;
using System.Linq;
using NLog;
using rio.Battlefield4.ServerSearch.Core;
using System.Threading.Tasks;

namespace rio.Battlefield4.ServerSearch.ConsoleRunner
{
    class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            var searcher = new Searcher();
            var searchResults = searcher.Search();

            var orderedSearchResult = searchResults.OrderByDescending(r => r.Weight);

            foreach (var searchResult in orderedSearchResult)
            {
                Logger.Info("Server '{1}', weighted {2}{0}http://battlelog.battlefield.com/bf4/servers/show/pc/{3}{0}",
                    Environment.NewLine,
                    searchResult.Server,
                    searchResult.Weight,
                    searchResult.Server.Guid);
            }

            Console.ReadKey();
        }
    }
}
