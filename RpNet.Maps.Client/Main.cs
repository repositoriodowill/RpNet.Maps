using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RpNet.Maps.Client
{
    public class Main : BaseScript
    {
        /// Arquivo de configuração do Mapa.
        private static string _mapsList = "maps/_maps.txt";
        private static string _resourceName = API.GetCurrentResourceName();

        private string _lastLoadedMapName;

        /// <summary>
        /// Variável de controle. Indica se está testando o resource.<br></br>
        /// Se vc deletar a #region DeleteAoRebuildar, delete isso também!
        /// </summary>
        private bool _isTesting = false;

        ///Queue que contém todos os blipHandles criados.
        private Queue<Blip> BlipsHandles = new Queue<Blip>();

        ///Array de string que contém os nomes dos arquivos em _mapsList
        private string[] MapsNames;

        ///Dictionary que contém os nomes 
        private Dictionary<string, Map> MapsLoaded = new Dictionary<string, Map>();

        public Main()
        {
            MapsNames = API.LoadResourceFile(_resourceName, _mapsList).Trim().Split('\n'); ///Carrega os nomes dos mapas no arquivo maps/_maps.txt
                                                                                           ///Arrumando map Names...string.Trim() 
            for (int i = 0; i < MapsNames.Length; i++)
            {
                MapsNames[i] = $"maps/{MapsNames[i].Trim()}";
            }

            EventHandlers["onClientResourceStart"] += new Action<string>(OnStart); ///Evento disparado quando o resource iniciar
        }

        private Map GetFromJson(string jsonMapFile)
        {
            try
            {
                ///Converte o map em json para o objeto e itera sobre todos os blips, criando eles no map.
                return JsonConvert.DeserializeObject<Map>(jsonMapFile);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Sending Exception...");
                TriggerServerEvent("RpNet.Exception", ex.Message, ex.StackTrace); ///Envia a exception para o server.
                return null;
            }
        }

        private void LoadMaps()
        {
            foreach (var name in MapsNames)
            {
                MapsLoaded.Add(name, GetFromJson(API.LoadResourceFile(_resourceName, name)));
            }
        }

        ///Método que recebe o map em json e cria os blips no map.
        private async void SetBlips(Map map)
        {
            if (map != null)
            {
                try
                {
                    ClearBlips();
                    foreach (var item in map.Blips)
                    {
                        BuildSimpleBlip(item);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Sending Exception...");
                    TriggerServerEvent("RpNet.Exception", ex.Message, ex.StackTrace);
                }
            }
            else
            {
                Debug.WriteLine("Mapa vazio :(");
            }
        }

        ///Método que limpa os blips do map.
        private async void ClearBlips()
        {
            ///Se a Queue BlipsHandles tiver algum item...
            if (BlipsHandles.Count > 0)
            {
                ///Enquanto a Queue BlipsHandles tiver algum item...
                while (BlipsHandles.Count > 0)
                {
                    ///O método Dequeue() retorna o último item do Queue
                    ///E o retira da lista. Dessa forma a cada iteração do while o BlipsHandles.Count diminui 1...
                    ///Quando BlipHandles.Count=0, encerra o while.
                    BlipsHandles.Dequeue().Delete();
                }
            }
        }

        ///Método que cria um blip no map. E também adiciona a referência do bliphandle no Queue BlipsHandles. 
        private void BuildSimpleBlip(MapBlip mapBlip, bool setRoute = false)
        {
            try
            {
                int i = 1;
                foreach (var item in mapBlip.Coords)
                {
                    Blip blipHandler = new Blip(API.AddBlipForCoord(item.X, item.Y, item.Z));
                    blipHandler.Sprite = (BlipSprite)mapBlip.MarkerId;
                    blipHandler.Scale = mapBlip.MarkerSize;
                    blipHandler.Color = (BlipColor)mapBlip.MarkerColor;
                    API.BeginTextCommandSetBlipName("STRING");
                    API.AddTextComponentString(mapBlip.Grouped ? mapBlip.Name : $"{mapBlip.Name} {i}");
                    API.EndTextCommandSetBlipName(blipHandler.Handle);
                    blipHandler.IsShortRange = mapBlip.IsShortRange; ///true=só mostra no minimap quando estiver perto | false=sempre mostra no minimap
                    blipHandler.ShowRoute = false;
                    BlipsHandles.Enqueue(blipHandler); ///Adiciona o BlipHandle na Queue BlipsHandles
                    i++;
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine("Sending Exception...");
                TriggerServerEvent("RpNet.Exception", ex.Message, ex.StackTrace);
            }
        }

        ///Método Executado quando o resource iniciar...onClientResourceStart
        private void OnStart(string resourceName)
        {
            if (!_resourceName.Equals(resourceName)) return;

            ///Carrega todos os Mapas na Memória;
            LoadMaps();
            ///Define o Map inicial como sendo o default.json
            ///A lógica do método { MapsLoaded.FirstOrDefault(x => x.Key.Contains("default")) } é fazer uma busca nos mapas carregados (LoadMaps()) trazendo o primeiro Map que CONTENHA a key "default"...
            ///supondo que há um arquivo "default.json", o método retornará o nome do arquivo "default.json"...
            SetBlips(MapsLoaded.FirstOrDefault(x => x.Key.Contains("default")).Value);

            #region DeleteAoRebuildar
            /// O intuíto desses métotos é testar o consumo de memória do resource
            /// e se o resource é pesado para executar.

            ///Registra o comando para testar todos os mapas disponíveis...
            API.RegisterCommand("testmap", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                int time = 3000;
                if (args.Any() && args != null)
                {
                    try
                    {
                        int num = Convert.ToInt32(args[0]);
                        time = num <= 3 ? 3000 : num * 1000;
                    }
                    catch
                    {
                        time = 3000;
                    }
                }

                ///A primeira vez que você executa o comando /testmap, ele seta a variável de controle _isTesting como true, para iniciar o while
                _isTesting = true;
                ///Enquanto estiver testando (_isTesting=true)...
                while (_isTesting)
                {
                    ///Percorre todos os maps em MapsLoaded
                    foreach (var item in MapsLoaded.Values)
                    {
                        ///! If recursivo antes de cada iteração
                        ///Se não tiver esse if, o teste só vai realmente parar quando o foreach terminar as iterações, e não quando vc executar o /stoptest.
                        ///quando vc executa o /stoptest, ele seta a variável de controle _isTesting como false.
                        ///dessa forma, a cada iteração é checado se  _isTesting=false e se sim, encerra as iterações do foreach com o break.
                        if (!_isTesting) break; //! Se NÃO estiver mais testando, encerra o foreach...

                        ///Seta os blips no map e espera 2 segundos para a próxima iteração.
                        SetBlips(item);
                        await Delay(time);
                    }
                }


            }), false);

            ///Registra o comando "stoptest" e seta a variável de controle _isTesting = false.
            API.RegisterCommand("stoptest", new Action(() => _isTesting = false), false);

            //Adicionando ajuda aos comandos
            TriggerEvent("chat:addSuggestion", "/stoptest", "Para o teste de mapas.");
            TriggerEvent("chat:addSuggestion", "/testmap", "Inicia o teste de mapas.", new[]
            {
                new{name="[int]Tempo de espera para próximo mapa", help="Tempo em segundos. Min/Default 3 segundos."}
            });
            #endregion

            ///Registra o comando /map
            ///Esse método precisa de um parâmetro. E ele é obrigatorio.
            ///Caso não queira isso, apague o else. Assim ele não irá disparar o evento "chat:addMessage".
            API.RegisterCommand("map", new Action<int, List<object>, string>((source, args, raw) =>
            {
                ///Converte a lista de argumentos( List<object> ) para uma lista de string ( List<string> )
                var argList = args.Select(o => o.ToString()).ToList();
                if (argList.Any()) ///Se a lista de argumentos tiver algum ítem...
                {
                    /// "/map clear" limpa os blips do map.
                    if ("clear".Equals(argList[0]))
                    {
                        ClearBlips();
                        return;
                    }
                    var mapName = argList[0]; ///atribui o primeiro argumento da lista...
                                              ///Procura na lista de nomes de mapas o argumento passado...Exemplo "/map default"
                                              /// vai procurar e retornar o primeiro nome do map que contenha "default"...
                                              /// nesse caso irá retornar default.json.
                                              ///Se não localizar algum mapa, retorna string.Empty...
                    _lastLoadedMapName = MapsNames.FirstOrDefault(x => x.Contains($"{mapName}"));

                    if (!string.IsNullOrEmpty(_lastLoadedMapName)) ///Se localizar algum mapa
                    {

                        ///var mapFile = $"maps/{_lastLoadedMapName}"; //trata o nome do map e cria o caminho do resource...
                        if (MapsLoaded.ContainsKey(_lastLoadedMapName))
                        {
                            SetBlips(MapsLoaded[_lastLoadedMapName]);
                            return;
                        }
                    }
                }
                else ///Se o comando não tiver nenhum argumento...
                {
                    ///Supondo que há um arquivo default.json...
                    try
                    {
                        SetBlips(MapsLoaded["default"]);
                    }
                    catch
                    {
                        if (MapsLoaded.Count > 0 && MapsLoaded != null)
                            SetBlips(MapsLoaded.First().Value);
                    }
                    return;
                }
            }), false);

            //Adicionando ajuda aos comandos
            TriggerEvent("chat:addSuggestion", "/map", "Carrega um mapa.", new[]
            {
                new{name="[string]Nome do mapa. '/map clear' limpa os blips do mapa.", help="Nome do mapa que será carregado."}
            });

        }


    }
}
