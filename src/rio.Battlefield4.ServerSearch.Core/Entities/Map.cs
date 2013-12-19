using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rio.Battlefield4.ServerSearch.Core.Extensions;

namespace rio.Battlefield4.ServerSearch.Core.Entities
{
    public class Map
    {
        private static Dictionary<string, string> _mapIdToName = new Dictionary<string, string>()
        {
            { "XP1_001", "Silk Road" },
            { "XP1_002", "Altai Range" },
            { "XP1_003", "Guilin Peaks" },
            { "XP1_004", "Dragon Pass" },
            { "MP_Tremors", "Dawnbreaker" },
            { "MP_Flooded", "Flood Zone" },
            { "MP_Journey", "Golmud Railway" },
            { "MP_Resort", "Hainan Resort" },
            { "MP_Damage", "Lancang Dam" },
            { "MP_Prison", "Operation Locker" },
            { "MP_Naval", "Paracel Storm" },
            { "MP_TheDish", "Rogue Transmission" },
            { "MP_Siege", "Siege of Shanghai" },
            { "MP_Abandoned", "Zavod 311" }
        };

        public MapMode Mode { get; set; }

        public string Id { get; set; }

        private string _name;

        public string Name
        {
            get
            {
                if (_name != null)
                    return _name;

                string name;
                if (_mapIdToName.TryGetValue(Id, out name))
                    _name = name;

                return _name;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} ({1}), {2}",
                Name,
                Id,
                Mode.GetDescription());
        }
        
        protected bool Equals(Map other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Map) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Map left, Map right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Map left, Map right)
        {
            return !Equals(left, right);
        }
    }
}
