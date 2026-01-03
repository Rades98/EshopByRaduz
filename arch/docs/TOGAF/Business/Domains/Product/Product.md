# Product – Core Domain

- Definuje metadata produktů, jejich varianty a životní cyklus.  
- Zdroj a místo založení produktu

## Klíčové procesy

- Správa produktových entit a variant
- Životní cyklus produktu (draft → active → retired)
- Synchronizace s:
    - Catalog
    - Stock
    - Provisioning
    - Marketing

## Architektonické principy

- Agregát Product s historií a stavem
- Event-driven propagace změn
- Oddělení metadat a managementu od obchodní logiky
- Explicitní ownership dat