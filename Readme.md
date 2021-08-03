# Sobre
##### RpNet.Maps
Resource Client Side que serve para criar blips no mapa do FiveM. Configure os blips do seu mapa em arquivos json.

O intuito desse projeto também é mostrar alguns exemplos de utilização do C# para criação de scripts para o FiveM, como por exemplo:
* Registrar um comando `API.RegisterComand();`
* Adicionar ajuda aos comandos `API.TriggerEvent("chat:addSuggestion", "/car", "Spanwna um Carro");`
* Carregar arquivos do lado do Client `API.LoadResourceFile();`
* Tratamento de `Exception` e envio dos dados para o Server `TriggerServerEvent("RpNet.Exception", ex.Message, ex.StackTrace);`


Por isso tentei documentar ao máximo o código fonte.

## Recursos
- Crie diferentes mapas, cada um com um conjunto de blips diferentes.
- Configuração e criação dos mapas com arquivos json.
- Faz o log de erros no servidor, se algo der errado no carregamento de algum mapa.

## Utilização do Resource
O resource está disponível na pasta 'Dist'. Copie essa pasta para a pasta de resource do servidor, dê um nome de sua preferência e inicie o resource no config do servidor.

Você pode copiar algum arquivo da pasta 'maps' e usar como modelo para criar seu próprio mapa. Para mais detalhes de como adicionar o mapa, consulte "Criando seumapa.json"

##### Configurando os Blips
Para configurar os Blips, altere as propriedades.
`string Name = Texto do Blip no Mapa`
`int MarkerId = Ícone do Blip no Mapa`
`int MarkerColor = Cor do Blip no Mapa`
`float MarkerSize = Tamanho do Marcador`
`bool Grouped = Indica se os Blips serão agrupados`

[Consulte aqui](https://docs.fivem.net/docs/game-references/blips/) mais detalhes sobre a Cor e o Ícone dos Blips.

```json
{
  "Blips": [
    {
      "Name": "EA | Empório Alexandre",
      "MarkerId": 73,
      "MarkerColor": 25,
      "MarkerSize": 1,
      "Grouped": true,
      "IsShortRange": true,
      "Coords": [
        {
          "X": -4.509100,
          "Y": 6521.252930,
          "Z": 30.571024
        },
        {
          "X": 1678.057495,
          "Y": 4819.882324,
          "Z": 41.299820
        }
      ]
    }
  ]
}
```


##### Criando seumapa.json
- Crie um arquivo "seumapa.json" na pasta 'maps'
- Adicione o nome do mapa no arquivo 'maps/_maps.txt' ("seumapa.json")
`seumapa.json`
- Adicione o nome do mapa no 'fxmanifest.lua' 
```lua
files {
    'Newtonsoft.Json.dll',
	--Maps
	'maps/_maps.txt', --obrigatório
	'maps/default.json', --obrigatório
	'maps/seumapa.json', --seumapa
}
```
- Reinicie o servidor.

