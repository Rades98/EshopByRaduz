# Catalog – Supporting Domain

- Konsoliduje snapshoty z Product, Stock, Pricing a Regulatory.  
- Slouží pro prezentaci produktů.

## Klíčové procesy

- Konsolidace dat produktů
- Publikace snapshotů pro frontend a BFF
- Synchronizace s:
    - Product
    - Pricing
    - Stock
    - Regulatory
- Light validace košíku, kde ještě není nutná authoritive konzistence dat

## Architektonické principy

- Read-only model
- Event-driven aktualizace snapshotů
- Oddělení prezentace dat od zdroje
- Udržování konzistence mezi read-modelem a authoritative zdroji