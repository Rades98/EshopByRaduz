# Environment & Infrastructure Automation


> Jakákoliv změna (prostředí, infrastruktura, provozní konfigurace), která není reprezentována v Git repozitáři 
a aplikována automatizovaným procesem, je považována za akci v rozporu s těmito pravidly.

## Záměr

Automatizace prostředí a infrastruktury zajišťuje, že prostředí, infrastruktura a provozní konfigurace jsou řízeny 
stejným způsobem jako aplikace: deklarativně, auditovatelně, konzistentně a bez manuálních zásahů.

### Garance

Architektura formou automatizace prostředí zajišťuje:

- jednotné prostředí napříč lifecycle
- deklarativní řízení infrastruktury (IaC)
- jednotnou správu konfigurací a secrets
- GitOps jako jediný mechanismus aplikace změn
- audit změn
- provozní stabilitu díky standardizované observabilitě

### Dopad

- Eliminace manuálních zásahů a s tím spojených rizik
- Rychlejší a jasně ohraničený provisioning + údržba prostředí
- Vyšší bezpečnost díky centralizované správě secrets (Vault, Hashicorp, ...)
- Jednotné chování napříč prostředími (DEV → TEST → Stage → Prod)
- Snížení provozních nákladů díky automatizaci a jednoduchému přepoužití
- Lepší reakce na incidenty díky standardizovaným provozním signálům (OTEL sidecar + monitoring)

## Jednotný standard práce

### Infrastructure as Code (IaC)  

Veškerá infrastruktura je deklarativně definovaná a verzována v Gitu. Veškeré změny procházejí klasickým procesem obsahujícím pull request, schvalování a případně i testy.

### GitOps jako runtime governance 

Git je jediný zdroj pravdy pro konfiguraci prostředí.
GitOps controller je jediný executor změn v runtime.

### Centralizovaná správa secrets

Secrets jsou spravovány v dedikovaném systému a roubování do aplikací probíhá až v momentě nasazení, nikoliv během vývojového procesu, či v CI/CD.

Secrets se nikdy nevyskytují v:

- `Git repozitářích`
- `CI/CD definicích`
- `aplikačních konfiguračních souborech` (appsettings.json/.env etc)
- `build artefaktech`

### Standardizovaná observabilita

Logy, metriky a tracing jsou povinné a jednotné napříč službami.
Provozní signály jsou využívány pro validaci rolloutů i incident management.

### Provozní governance  

Každé prostředí má definovaná pravidla přístupu, promotion a provozní readiness.
Manuální zásahy do prostředí jsou zakázány.

Každé prostředí má:

- definovaná přístupová práva (RBAC)
- jasně definovaný proces promotion (DEV → TEST → STAGE → PROD)
- explicitní kritéria provozní readiness (health, SLI/SLO, observabilita)

## Zakázané postupy

- přímé zásahy do runtime prostředí
- změny infrastruktury mimo IaC
- vytváření prostředí nebo zdrojů mimo IaC
- úpravy konfigurace mimo Git
- manuální zásahy do GitOps repozitáře
- deployment mimo CI/CD proces
- ukládání secrets do repozitářů, CI/CD nebo konfiguračních souborů
- obcházení validačních kroků (force‑merge, bypass pipeline)

> Všechny výše popsané zakázané postupy platí primárně pro běžný provoz, výjimky musí být konzultovány a trackovány.