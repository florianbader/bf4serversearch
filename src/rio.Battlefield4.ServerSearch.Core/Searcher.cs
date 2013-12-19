using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Net.Http.Headers;
using rio.Battlefield4.ServerSearch.Core.Extensions;
using rio.Battlefield4.ServerSearch.Core.Repositories;
using rio.Battlefield4.ServerSearch.Core.Entities;
using NLog;

namespace rio.Battlefield4.ServerSearch.Core
{
    public class Searcher
    {
        private delegate int WeightServerDelegate(Server server);

        private readonly ServerRepository _serverRepository;

        private readonly List<WeightServerDelegate> _weightServerDelegates;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Searcher()
        {
            _weightServerDelegates = new List<WeightServerDelegate>();
            _serverRepository = new ServerRepository();

            InitializeWeights();
        }

        private void InitializeWeights()
        {
            _weightServerDelegates.Add(WeightServerByMaps);
        }

        public int WeightServerByMaps(Server server)
        {
            int weight = 0;

            var countMapModes = Enum.GetValues(typeof (MapMode))
                                    .Cast<MapMode>()
                                    .ToDictionary(mapMode => mapMode, mapMode => 0);

            foreach (var map in server.Maps)
            {
                countMapModes[map.Mode]++;
            }

            int countConquest = countMapModes[MapMode.Conquest] 
                                + countMapModes[MapMode.ConquestLarge];
            if (countConquest > 1)
                weight += 5;

            int countRush = countMapModes[MapMode.Rush];
            if (countRush > 1)
                weight += 5;

            int countDomination = countMapModes[MapMode.Domination];
            if (countDomination > 1)
                weight += 4;

            int countDeathmatch = countMapModes[MapMode.SquadDeathmatch] 
                                    + countMapModes[MapMode.TeamDeathmatch];
            if (countDeathmatch > 1)
                weight += 2;

            int countOtherMapModes = countMapModes[MapMode.AirSuperiority]
                                     + countMapModes[MapMode.Defuse];
            if (countOtherMapModes > 1)
                weight += 1;
            
            return weight;
        }

        public SearchResults Search()
        {
            var results = new SearchResults();

            try
            {
                var servers = _serverRepository.GetServers()
                    .Union(_serverRepository.GetServers(1))
                    .Union(_serverRepository.GetServers(2))
                    .Union(_serverRepository.GetServers(3))
                    .Union(_serverRepository.GetServers(4))
                    .Union(_serverRepository.GetServers(5))
                    .Distinct()
                    .ToList();

                Logger.Debug("Search found {0} servers", servers.Count);

                if (!servers.Any()) 
                    return results;

                foreach (var server in servers)
                {
                    int weight = WeightServer(server);

                    Logger.Debug("Server '{0}' rated {1}", server, weight);

                    if (weight > 0)
                        results.Add(weight, server);
                }

                return results;
            }
            catch (Exception exception)
            {
                Logger.ErrorException("Search()", exception);
                return results;
            }
        }

        private int WeightServer(Server server)
        {
            return _weightServerDelegates.Sum(d => d(server));
        }
    }
}
