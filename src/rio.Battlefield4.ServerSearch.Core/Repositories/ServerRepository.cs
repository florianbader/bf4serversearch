using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NLog;
using rio.Battlefield4.ServerSearch.Core.Extensions;
using rio.Battlefield4.ServerSearch.Core.Entities;

namespace rio.Battlefield4.ServerSearch.Core.Repositories
{
    public class ServerRepository
    {
        // &slots=16&slots=8
        private const string GetServersUrl =
            "http://battlelog.battlefield.com/bf4/servers/pc/?filtered=1&expand=1&settings=vgmc%2C0%2C100|osls%2C0%2C0|vffi%2C0%2C0&useLocation=1&useAdvanced=1&slots=1&slots=2&slots=4&gameSize=32&q=&serverTypes=1&serverTypes=2&mapRotation=-1&modeRotation=-1&password=-1&country=fi&country=de&country=se&country=dk&country=pl&country=ch&country=nl&country=no&country=gb&country=at&country=be&country=cz&osls=0&vvsa=-1&vffi=0&vaba=-1&vkca=-1&v3ca=-1&v3sp=-1&vmsp=-1&vrhe=-1&vhud=-1&vmin=-1&vnta=-1&vbdm-min=1&vbdm-max=300&vprt-min=1&vprt-max=300&vshe-min=1&vshe-max=300&vtkk-min=1&vtkk-max=99&vnit-min=30&vnit-max=900&vtkc-min=1&vtkc-max=99&vvsd-min=0&vvsd-max=500&settings-vgmc=1&vgmc-min=0&vgmc-max=100";

        private const string GetServerUrl = "http://battlelog.battlefield.com/bf4/servers/show/pc/{0}/?json=1";

        private const int ServerPageSize = 20;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public IEnumerable<Server> GetServers(int page = 0)
        {
            return GetServersInternal(page * ServerPageSize);
        }

        public IEnumerable<Server> GetServersInternal(int offset = 0)
        {
            using (var httpClient = new HttpClient())
            {
                InitializeHttpClient(httpClient);

                string url = string.Format("{0}&offset={1}&count={2}",
                    GetServersUrl,
                    offset,
                    ServerPageSize);

                string jsonString = httpClient.GetStringAsync(url).Result;
                dynamic json = JObject.Parse(jsonString);

                if (json == null
                    || json.globalContext == null
                    || json.globalContext.servers == null)
                {
                    Logger.Warn("GetServersInternal(): Received invalid json, no servers found");
                    yield break;
                }

                foreach (dynamic server in json.globalContext.servers)
                {
                    Guid id;
                    string idString = server.guid;

                    if (!Guid.TryParse(idString, out id))
                        continue;

                    var serverResult = GetServer(id);
                    if (serverResult != null)
                        yield return serverResult;

                    Thread.Sleep(200);
                }
            }
        }

        private static void InitializeHttpClient(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("X-AjaxNavigation", "1");
            httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        }

        public Server GetServer(Guid id)
        {
            using (var httpClient = new HttpClient())
            {
                InitializeHttpClient(httpClient);
                
                string url = string.Format(GetServerUrl, 
                    id.ToString().Replace("{", string.Empty).Replace("}", string.Empty));

                string jsonString = httpClient.GetStringAsync(url).Result;
                if (string.IsNullOrWhiteSpace(jsonString))
                {
                    Logger.Debug("GetServer(): Retreived json string is null or empty");
                    return null;
                }

                dynamic json = JObject.Parse(jsonString);
                if (json == null)
                {
                    Logger.Debug("GetServer(): Retreived json is null");
                    return null;
                }

                if (json.type != "success")
                {
                    Logger.Debug("GetServer(): No success");
                    return null;
                }
                
                var serverInfo = json.message.SERVER_INFO;

                bool settings3DSpotting = serverInfo.settings.v3sp == 1;
                bool settingsMinimapSpotting = serverInfo.settings.vmsp == 1;
                bool settingsOnlySquadleaderSpawn = serverInfo.settings.osls == 1;
                bool settingsHitIndicator = serverInfo.settings.vhit == 1;
                bool settingsFriendlyFire = serverInfo.settings.vffi == 1;
                bool settingsDisplayHud = serverInfo.settings.vhud == 1;
                bool settingsDisplayMinimap = serverInfo.settings.vmin == 1;
                bool settingsRegenerativeHealth = serverInfo.settings.vrhe == 1;

                bool hardMode = !settings3DSpotting
                                | !settingsDisplayHud
                                //| !settingsDisplayMinimap
                                | settingsFriendlyFire
                                | !settingsHitIndicator
                                //| !settingsMinimapSpotting
                                | settingsOnlySquadleaderSpawn
                                | !settingsRegenerativeHealth;

                if (hardMode)
                    return null;

                var server = new Server();
                server.Guid = id;
                server.Name = serverInfo.name;
                server.Maps = GetServerMapsInternal(serverInfo.maps.maps);

                return server;
            }
        }

        private List<Map> GetServerMapsInternal(dynamic maps)
        {
            var mapResults = new List<Map>();

            foreach (dynamic map in maps)
            {
                var mapResult = new Map();
                mapResult.Mode = (MapMode) map.mapMode;
                mapResult.Id = map.map;

                mapResults.Add(mapResult);
            }

            return mapResults;
        }
    }
}
