# Introduction 
Zadavatel Ostravská Univerzita (OSU) v rámci studijních oborů bude studentům nabízet / požadovat firemní praxe, které si v rámci dané katedry / oboru mohou vybrat. Tyto praxe musí být dokumentovány, reportované a kontrolované pověřenými osobami, nebo učiteli v rámci informačního systému.

## Zainteresované strany
* Ostravská univerzita (KCH a KIP, administrátoři, učitelé, studenti)
* Firmy (jako zadavatelé praxí, pověřené osoby za firmy)
* Vývojář (Lukáš Vachtarčík)


# Getting Started
TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:
1.	Installation process
2.	Software dependencies
3.	Latest releases
4.	API references

# Nasazení
Pro spuštění projektu a jeho úspešné kontejnerizaci, je potřeba upravit v docker-compose hodnotu proměnné `sqlhost` na ip adresu hosta.
Compose je kompatibilní jak pro kontejnerizaci v Dockeru tak v Podmanu, avšak z důvodů některých nekompatibilit a inkonzistence chování Podmana je adresa hostitele řešena takto.
