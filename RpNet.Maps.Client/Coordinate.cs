using Newtonsoft.Json;
using System.Globalization;
using System.Linq;

namespace RpNet.Maps.Client
{
    public class Coordinate
    {

        CultureInfo _cultureInfo = new CultureInfo("en-us");


        [JsonProperty("X")]
        public float X { get; private set; }

        [JsonProperty("Y")]
        public float Y { get; private set; }

        [JsonProperty("Z")]
        public float Z { get; private set; }


        private void DefaultSetup() { X = 0f; Y = 0f; Z = 0f; }

        #region --Constructors

        private Coordinate() { DefaultSetup(); }
        public Coordinate(CultureInfo cultureInfo) : this()
        {
            _cultureInfo = cultureInfo;
        }

        public Coordinate(float x, float y, float z) { X = x; Y = y; Z = z; }

        public Coordinate(double x, double y, double z) { X = (float)x; Y = (float)y; Z = (float)z; }

        public Coordinate(string coordinate, char delimiter = ',') { GetCoordinateFrom(coordinate, delimiter); }

        public Coordinate(string[] coordinates) { GetCoordinateFrom(coordinates); }

        public Coordinate(float[] coordinates) { GetCoordinateFrom(coordinates); }
        #endregion

        /// <summary>
        /// Retorna o valor de X, Y ou Z. Baseado em Índice 0 ZERO.<br></br>
        /// Índice 0 = X, Índice 1 = Y, Índice 2 = Z, fora do intervalo = 0
        /// </summary>
        /// <param name="xyz">O índice da coordenada, onde:<br></br>
        /// 0 = X, 1 = Y, 2 = Z, fora do intervalo = 0</param>
        /// <returns></returns>
        public float this[int xyz]
        {
            get
            {
                switch (xyz)
                {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    default: return 0f;
                }
            }
            set
            {
                switch (xyz)
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    case 2: Z = value; break;
                    default: break;
                }
            }
        }

        #region Private Methods
        /// <summary>
        /// Preenche as coordenadas X, Y e Z com uma string de coordenadas, separada por delimitador.<br></br>
        /// Exemplos: <br></br>
        /// string home1 = "-866.68 , 457.38 , 88.28"; // string delimitada por "," <br></br>
        /// string home2 = "-866.68 ; 457.38 ; 88.28"; // string delimitada por ";" <br></br>
        /// </summary>
        /// <param name="coordinates">string que contém as coordenadas X, Y e Z.</param>
        /// /// <param name="delimiter">Delimitador utilizado para separar(split) as coordenadas.</param>
        private void GetCoordinateFrom(string coordinate, char delimiter = ',')
        {
            string[] cds = coordinate.Split(delimiter);
            GetCoordinateFrom(cds);
        }

        /// <summary>
        /// Preenche as coordenadas X, Y e Z com os valores de um array de string contendo coordenadas.<br></br>
        /// Exemplo.: string[] arrayCoords = new string[]{"-866.68","457.38","88.28"};
        /// </summary>
        /// <param name="coordinates">Array de string que contém as coordenadas X, Y e Z.</param>
        private void GetCoordinateFrom(string[] coordinates)
        {
            if (coordinates.Any() && coordinates.Length >= 3)
            {
                float x = 0.0f;
                float y = 0.0f;
                float z = 0.0f;

                x = float.Parse(coordinates[0], _cultureInfo);
                y = float.Parse(coordinates[1], _cultureInfo);
                z = float.Parse(coordinates[2], _cultureInfo);

                X = x;
                Y = y;
                Z = z;
            }
        }

        /// <summary>
        /// Preenche as coordenadas X, Y e Z com um array float de coordenadas.<br></br>
        /// Exemplo.: float[] arrayCoords = new float[]{-866.68,457.38,88.28};
        /// </summary>
        /// <param name="coordinates">Array de float que contém as coordenadas X, Y e Z.</param>
        private void GetCoordinateFrom(float[] coordinates)
        {
            switch (coordinates.Length)
            {
                case 0:
                    DefaultSetup();
                    break;
                case 1:
                    X = coordinates[0];
                    Y = 0;
                    Z = 0;
                    break;
                case 2:
                    X = coordinates[0];
                    Y = coordinates[1];
                    Z = 0;
                    break;
                case 3:
                default:
                    X = coordinates[0];
                    Y = coordinates[1];
                    Z = coordinates[2];
                    break;
            }
        }
        #endregion
    }
}
