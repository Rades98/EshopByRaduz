# Change & Delivery Automation (CI/CD)

Každá změna v repozitáři musí projít konzistentním, auditovatelným a automatizovaným procesem. 
Tento proces je jediná cesta, jak dostat změny do produkčního prostředí.

> Jakýkoliv manuální zásah je v rozporu s architektonickými pravidly definovanými v tomto dokumentu

## Záměr

Change & Delivery Automation (CI/CD) je základní schopnost architektury zajišťující konzistentní a bezpečné chování v systému.
Tento mechanismus zajišťuje, že všechny změny jsou sledovatelné, reprodukovatelné a řízené napříč celým životním cyklem systému.
Vztváří auditní stopu rozhodnutí a pokusů o změny, tím je chráněna stabilita provozu a kontinuita nasazování.

### Garance

Architektura formou CI/CD zajišťuje:

- konzistentní a reprodukovatelné buildy
- jednotnou validaci změn
- řízenou propagaci mezi prostředími
- automatické rollbacky do stabilního stavu
- audit změn a pokusů

### Dopad

- Konzistentní a předvídatelný release
- Rychlejší reakce na incidenty
- Zjednodušení provozu díky jednotnému a deklarativnímu stylu řízení
- Transparentní audit změn
- Snížení nákladů

## Jednotný standard práce se zdrojovým kódem

### Řízení změn (Gitflow)

- `develop` je živý kód, nasazuje se pouze na DEV prostředí
- `main` je kód který má lifecycle přes TEST->Stage->Prod

![](gitflow.drawio)

### Povolené typy větví a target větve

- Do `main` se smí mergovat pouze
     - `develop` 
         - standradní release 
     - `hotfix/<space>_<ticketId>-<short_name>`
         - založeno z `main`    

- Do `develop` se smí mergovat
     - `main`
         - cherrypick změn vůči `develop` v případě hotfixu do mainu
     - `feature/<space>_<ticketId>-<short_name>`
         - založeno z `develop`
     - `fix/<space>_<ticketId>-<short_name>`
         - založeno z `develop`, v případě opravy chyby v `develop`
     - `chore/<space>_<ticketId>-<short_name>`
         - založeno z `develop`, v případě úpravy pipeline etc

> Nepovolené větve jsou odmítnuty validačním procesem.

### Výčet pipelines a jejich zodpovědností

- `pr` pipeline
     - validace názvu větve
     - validace target větve vůči názvu
     - validace commit message 
     - zpráva do kanálu daného spacu, že vznikl pull request

- `ci` pipeline
     - build
     - analýza kódu
     - testy
         - Arch
         - Unit
         - Integration
     - build dokumentace
     - docker build
     - docker publish   
    
- `cd` pipeline
     - stažení docker image
     - update values
     - helm pack
     - helm publish
     - commit do GitOps repozitáře
     - zpráva do kanálu daného spacu, že vznikla nová konfigurace pro deploy (mimo canary scénář)

### Deploy

- `cd` pipeline vytvoří PR s konfigurací
     - pokud běží v kontextu pull requestu → vytvoří canary konfiguraci (např. rollout.strategy=canary, weight=10 %)
     - pokud běží v kontextu merge do main → vytvoří standardní production konfiguraci
- GitOps controller provede rollout podle deklarované strategie
     - canary rollout (PR build)
     - standardní rollout (release build)
- Validace metrik
     - Health probes: readiness, liveness, startup
     - Log‑based alerts: detekce chybových patternů
     - Runtime synthetic checks: jednoduché sanity testy (např. /health)
- Automatický merge nebo rollback na základě validace
- Zpráva do kanálu daného spacu, že bylo nasazeno 
     - Musí obsahovat: prostředí, commitId, v případě nasazení na DEV prostředí i odkaz na task

### Promotion

Promotion je řízený proces přesunu verze mezi prostředími (TEST → Stage → Prod).
Probíhá výhradně formou změny deklarativní konfigurace v GitOps repozitáři a podléhá definovaným oprávněním a schvalovacím pravidlům.

- Promotion je možná pouze přes Git (PR nebo merge).
- Každé prostředí má vlastní schvalovací politiku.
- Promotion nesmí obejít CI/CD ani GitOps controller.
- Odpovědnost za promotion je definována v kapitole Environment & Infrastructure Automation.
- Promotion je řízená změna konfigurace, nikoliv technický zásah do běžícího prostředí.

## Zakázané postupy

- přímé zásahy do runtime
- deployment mimo CI/CD
- změny konfigurace mimo Git