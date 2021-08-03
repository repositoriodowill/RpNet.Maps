using Newtonsoft.Json;
using System.Collections.Generic;

namespace RpNet.Maps.Client
{
    public class MapBlip
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("MarkerId")]
        public int MarkerId { get; set; }
        [JsonProperty("MarkerColor")]
        public int MarkerColor { get; set; }
        [JsonProperty("MarkerSize")]
        public float MarkerSize { get; set; }
        [JsonProperty("Grouped")]
        public bool Grouped { get; set; }

        /// <summary>
        /// true = só mostra no minimap quando estiver perto<br></br>
        /// false = sempre mostra no minimap
        /// </summary>
        [JsonProperty("IsShortRange")]
        public bool IsShortRange { get; set; }
        /// <summary>
        /// Lista de Coordenadas
        /// </summary>
        [JsonProperty("Coords")]
        public List<Coordinate> Coords { get; set; }

        public MapBlip()
        {
            Coords = new List<Coordinate>();
            MarkerColor = 0;
            MarkerSize = 1.0f;
            Grouped = true;
            IsShortRange = false;
        }

        public MapBlip(string name) : this()
        {
            Name = name;
        }

        /// <summary>
        /// Cria uma instância de Blip com uma coordenada. <br></br>
        /// Preferível utilizar quando o <see cref="Map.Blip"/> tiver apenas uma coordenada.
        /// </summary>
        /// <param name="coordinate">"x,y,z" => "-212.0,-1378.0,31.0"</param>
        public MapBlip(string name, string coordinate) : this()
        {
            Name = name;
            AddCoord(coordinate);
        }

        /// <summary>
        /// Cria uma instância de Blip e preenche a <see cref="Coords"/> com um array de coordenadas
        /// </summary>
        /// <param name="coordinates">Array de coordenadas:<br></br>
        /// "x,y,z" => "-212.0,-1378.0,31.0"
        /// </param>
        /// <param name="delimiter">Delimitador , . |</param> 
        public MapBlip(string name, string[] coordinates, char delimiter = ',') : this()
        {
            Name = name;
            foreach (var item in coordinates)
            {
                AddCoord(item, delimiter);
            }
        }

        /// <summary>
        /// Adiciona um Blip na <see cref="Coords"/> com as coordenadas
        /// </summary>
        /// <param name="coordinate">"x,y,z" => "-212.0,-1378.0,31.0"</param>
        /// <param name="delimiter">Delimitador , . |</param>
        private void AddCoord(string coordinate, char delimiter = ',')
        {

            Coordinate c = new Coordinate(coordinate, delimiter);
            Coords.Add(c);
        }

    }

}