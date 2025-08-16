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

Nastavení proměnných pro správný běch aplikace:

* `SERVER_NAME` = doména serveru shodná s doménou certifikátu. Př. praxe.osu.cz, praxe.cz apod.
* `SERVER_IP` = IP adresa hostitele (tam, kde je spuštěný Podman / Docker)
* `SQL_HOST` = alias, pomocná proměnná, může být cokoli. Př. sqlhost..
* `SQL_HOST_IP` = adresa DB serveru
* `SSL_PATH` = cesta k certifikátu pro backed
* `SSL_PASSWORD` = passphrase certifikátu
