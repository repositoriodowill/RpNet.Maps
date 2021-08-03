fx_version 'bodacious'
games {'gta5'}

-- Resource data
name 'RpNet.Maps'
description 'Crie blips no mapa do FiveM. Configure os blips do seu mapa em arquivos json'
version 'v1.0.0'
author 'William Pacifico'
url 'https://github.com/repositoriodowill/RpNet.Maps/'

-- Scripts
client_scripts {
    'RpNet.Maps.Client.net.dll',
} 
server_scripts {
    'RpNet.Maps.Server.net.dll'
} 

-- Files
files {
    'Newtonsoft.Json.dll',
	--Maps
	'maps/_maps.txt', 
	'maps/default.json',
	'maps/homes.json'
}