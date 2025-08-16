# Introduction 
Zadavatel Ostravská Univerzita (OSU) v rámci studijních oborů bude studentům nabízet / požadovat firemní praxe, které si v rámci dané katedry / oboru mohou vybrat. Tyto praxe musí být dokumentovány, reportované a kontrolované pověřenými osobami, nebo učiteli v rámci informačního systému.

## Zainteresované strany
* Ostravská univerzita (KCH a KIP, administrátoři, učitelé, studenti)
* Firmy (jako zadavatelé praxí, pověřené osoby za firmy)
* Vývojář (Lukáš Vachtarčík)

# Prerequisities
* .NET 8
* Installed podman with superuser behaviour or Docker
* MS SQL Server
* Installed certificates for both backend services and frontend. Otherwise NGINX will not establish secured connection.

# Nasazení
Pro spuštění projektu a jeho úspešné kontejnerizaci, je potřeba upravit v docker-compose hodnotu proměnné `sqlhost` na ip adresu hosta pro správný běh a napojení na DB.
Compose je kompatibilní jak pro kontejnerizaci v Dockeru tak v Podmanu, avšak z důvodů některých nekompatibilit a inkonzistence chování Podmana je adresa hostitele řešena takto.
