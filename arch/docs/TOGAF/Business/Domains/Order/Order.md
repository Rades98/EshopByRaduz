# Order – Core Domain

- Evidence objednávek a stavů.  

## Klíčové procesy
- Vytvoření a správa objednávky
- Sledování životního cyklu objednávky
- Synchronizace s:
    - Checkout
    - Stock
    - Payments
    - Fulfillment
    - Accounting

## Architektonické principy
- Agregát Order s historií stavu (Event-Sourcing)
- Event-driven propagace změn
- Oddělení orchestrace od core business logiky
- Auditovatelnost a traceability