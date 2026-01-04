# Stock under stress

## Service Level Indicator

- dostupnost Stock API
- latence odpovědi
- úspěšnost rezervací

## Service Level Objective & Error budget

- Stock availability SLO: 99.0 % / měsíc
- Max tolerovaná latence Stock API: 200 ms
- Při vyčerpání error budgetu:
     - naškálování systému
     - přechod na soft rezervace
     - ochrana databáze před write stormem

## Problémy

> Jde o teoretické scénáře a teoretická řešení. Provozní realita si, [stejně jako voda](https://www.youtube.com/watch?v=I4qv3HeVAzc), často najde vlastní cestu, takže skutečné incidenty mohou přinést jiné — někdy jednodušší, jindy mnohem komplexnější varianty.

### Stock under stress

#### Scénář

- Extrémní zájem o jeden produkt (např. nový iPhone).
- Dochází k vysoké konkurenci na agregátu StockItem.
- Outbox generuje velké množství událostí → hrozí backlog.
- Databáze je zatížena čtením i zápisem.
- Škálování nemusí být řešení, jelikož bottleneck může být databáze

#### Dopad na systém

- Checkout nedokáže garantovat dostupnost položky.
- Rezervace selhávají kvůli kolizím (optimistic concurrency).
- Latence Stock API roste → hrozí timeouts v Checkoutu.

##### Trade‑offs

- Dočasné znemožnění dokončení nákupu s hláškou uživateli (vyprodáno/nelze ověřit dostupnost)
- Přechod na soft rezervaci, kde je třeba počítat s variantou, že položka bude skutečně vyprodána a na uživatele tak nezbyde kus a bude muset čekat na naskladnění
- Zvýšit timeouty na checkoutu

#### Řešení

- Optimistic concurrency + retry s exponential backoff a max retry count
- Circuit breaker mezi Stockem a ostaními doménami
- Škálovatelný outbox processor
- Idempotentní změna agregátu
     - Vazba na CheckoutId
- Periodické čištění ghost rezervací pomocí BG jobu
- Degradace systému - pending rezervace v checkoutu/soft rezervace s dokončením objednávky a hláškou o ověření po potvrzení objednávky
- Sekundární databáze (failover, nikoli multi-writer)

#### Disaster recovery

- Obnovení standardního režimu
- Automatické obnovení Outboxu + Validace DLQ
- Přepnutí na primární DB

### Stock outage

#### Scénář

- Výpadek databáze
- Přetížení instance
- Nasazení verze s chybou
- Síťové problémy

#### Dopad na systém

- Checkout nemůže validovat ani rezervovat položky.
- Order nemůže převzít položky z Checkoutu.
- Fulfillment nemůže expedovat.

##### Trade‑offs

- Dočasné znemožnění dokončení nákupu s hláškou uživateli (vyprodáno/nelze ověřit dostupnost)
- Přechod na soft rezervaci, kde je třeba počítat s variantou, že položka bude skutečně vyprodána a na uživatele tak nezbyde kus a bude muset čekat na naskladnění
- Pozastavení naskladňování nových položek
- Zpoždění synchronizace do Catalogu.

#### Řešení

- Circuit breaker mezi Stockem a ostaními doménami
- Degradace systému - pending rezervace v checkoutu/soft rezervace s dokončením objednávky
- Health checky + auto recovery
- Observabilita - Monitoring deployů + běhu

#### Disaster recovery

- Obnovení standardního režimu
- Automatické obnovení Inboxu + Validace DLQ


### Kafka down scenario

#### Scénář

- Výpadek brokerů
- Plné disky

#### Dopad na systém

- Nesynchronizují se repliky dat pro catalog, Basket pak může špatně vyhodnocovat light validaci
- Nepřicházejí nové produkty z Product domény, tedy je nelze naskladnit
- Přeplňuje se outbox + nevede do DLQ -> hrozí backlog

##### Trade‑offs

- Catalog přejde do režimu last known state, kdy nemusí zobrazovat validní data o skladu
- Vypnutí light validace vůči katalogovému snapshotu v košíku

#### Řešení

- Outbox se nemaže, pokud nedošlo k odeslání eventu
- Možnost zastavit outbox processor
- Monitoring kafky (consumer lag etc)
- Failover cluster

#### Disaster recovery

- Dohnání consumer lagu
- Automatické obnovení Outboxu 