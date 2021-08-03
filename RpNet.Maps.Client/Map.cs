using System.Collections.Generic;

namespace RpNet.Maps.Client
{
    public class Map
    {
        public List<MapBlip> Blips { get; set; }
        public int BlipsCount => Blips.Count;

        public Map()
        {
            Blips = new List<MapBlip>();
        }

        public Map(MapBlip mapBlip) : this()
        {
            Blips.Add(mapBlip);
        }

    }
}
