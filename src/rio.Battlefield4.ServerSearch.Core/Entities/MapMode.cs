using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rio.Battlefield4.ServerSearch.Core.Entities
{
    public enum MapMode
    {
        [Description("Conquest")]
        Conquest = 1,

        [Description("Rush")]
        Rush = 2,

        [Description("Squad Deathmatch")]
        SquadDeathmatch = 8,

        [Description("Team Deathmatch")]
        TeamDeathmatch = 32,

        [Description("Conquest Large")]
        ConquestLarge = 64,

        [Description("Domination")]
        Domination = 1024,

        [Description("Obliteration")]
        Obliteration = 2097152,

        [Description("Air Superiority")]
        AirSuperiority = 8388608,

        [Description("Defuse")]
        Defuse = 16777216,

    }
}
