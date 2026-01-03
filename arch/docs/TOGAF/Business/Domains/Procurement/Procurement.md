# Procurement – Core Domain

- Řídí nákup položek od dodavatelů a jejich příjem do skladu.  

## Klíčové procesy

- Správa objednávek u dodavatelů
- Evidence příjmů a dodacích lhůt
- Synchronizace s:
    - Stock
    - Product
    - Accounting

## Architektonické principy

- Centralizovaná logika pro nákup
- Event-driven integrace s fyzickým skladem
- Oddělení interního procesu od frontendu
- Auditovatelnost a sledovatelnost nákupů