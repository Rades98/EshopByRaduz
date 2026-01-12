# Testování

## Záměr 
Testování je nedílnou součástí architektury a zajišťuje, že systém je stabilní, predikovatelný a odolný vůči změnám. 
Cílem je vytvořit jednotný rámec, který definuje, co se testuje, kdy, jak a s jakým dopadem na release proces - jak před, tak po nasazení.

## Garance 

- včasné odhalení chyb
- konzistentní kvalita napříč týmy 
- ověřitelnost architektonických pravidel 
- vašší provozní stabilita před nasazením 
- vyšší provozní stabilita po nasazení
- snížení nákladů na incidenty a rework 

## Dopad 

- rychlejší a bezpečnější releasy 
- méně incidentů v produkci 
- vyšší důvěra v provedené změny 
- lepší prediktivita chování systému 
- snížení nákladů na QA a provoz

## Vývoj

### Arch testy

Testy ověřující architektonická pravidla. Kromě code analyzérů se pravidla dají vynucovat také těmito testy, 
které jsou součástí pull requestů a zabraňují postupnému rozpadu architektury.
Zaměřují se na:

- Strukturu (Dodržení principů solution architektury - závislosti vrstev etc.)
- Naming konvence
- Kontrola registrací závislostí

### Unit testy

Testy ověřující logiku na úrovni unit, kde jako unitu považujeme třídu, či metodu.

- rychlé
- deterministické
- bez externích závislostí
- pokrývají pouze logiku na základě mockovaných vstupů
- musí pokrýt jak pozitivní, tak negativní scénáře

### Integrační testy

Testy ověřující spolupráci komponent.

Tyto testy globálně pokrývají:

- Kontejnerizovanou databázi odpovídající realitě s minimálním data-setem
- Messaging s kontejnerizovaným brokerem a mockovanými vstupy
- Volání API - jak REST, tak gRPC
- Vedlejší efekty (naplnění outboxu, či odeslání zprávy)
- Správnost všech kontraktů

Asertace zde musí probíhat jak na úrovni kontraktu vráceného API, tak kontrolou změn v databázi.

Tyto testy běží v izolovaném prostředí (sandbox) a nesmí záviset na žádné fyzické části infrastruktury (data/služby/messaging/...).

> Integrační testy jsou povinné pro všechny služby vystavující, či konzumující kontrakt na jakékoliv úrovni (sync/async)

## QA

QA testy ověřují chování systému z pohledu uživatele a business procesů. Jsou nadstavba k vývojovým testům, 
kde pro dokončení plnohodnotného scénáře chybí další komponenty.

### Manuální

Manuální testy jsou základ pro automatizaci. Primárně se focusují na:

- UX validace
- edge cases
- exploratory testing 
- případné testování scénářů, které nelze zautomatizovat

### Automatizované

Automatizované testy pokrývají business scénáře, které jsou opakovatelné a deterministické.

#### BDD

Behavioral driven testy jsou testy, které mají definovaný vlastní jazyk srozumitelný člověku, jenž se dá převést na vykonávaný kód na základě anotací.

- ideální využití nastává v okamžiku, kdy je výsledný proces popsán v manuální části spustitelný v BDD

### E2E

E2E testy ověřují kompletní business flow napříč službami. Mohou být jak manuální, tak automatizované. Tyto testy se používají pouze pro klíčové 
scénáře s vysokým dopadem. Nejsou náhradou unit nebo integračních testů.


---

## Provozní testy

Tyto testy slouží k ověření chování po nasazení. Jejich cílem je monitoring toho, že jsou služby stabilní, odolné a připravené k provozu.

### Smoke testy

Smoke testy běží bezprostředně po deployi a ověřují základní funkčnost (health), je to první validace rolloutu.

### Synthetic monitoring

Tyto testy slouží k pravidelnému volání klíčových endpointů a pro validaci dostupnosti/odezvy pro monitoring dodržování SLI

---

## A/B testy (Experimentální testování)

A/B testy slouží k ověřování business hypotéz v reálném prostředí a jsou cíleny na zákazníky.
Nejde o testování kvality, ale o řízený experiment nad reálným provozem.

A/B testy vyžadují:

- jasně definovanou hypotézu a metriky úspěchu
- řízený routing uživatelů (feature flags, canary routing)
- izolaci dopadu na business (konverze, chování, výkon)
- sběr a vyhodnocení dat v reálném čase
- možnost okamžitého vypnutí

## Procesní diagram

> Pokud view nenačítá, refreshujte, prosím, obrazovku

![](tests.drawio)