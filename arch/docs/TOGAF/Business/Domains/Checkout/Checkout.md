# Checkout – Core Domain

- Orchestrace rezervací a finálních kalkulací.  

## Klíčové procesy

- Validace košíku a položek
- Rezervace položek ve skladu
- Synchronní výpočet cen
- Synchronizace s:
    - Basket
    - Stock
    - Pricing
    - Order
    - Payments
    - Stock

## Architektonické principy

- Orchestrace bez business logiky
- Synchronous authoritative calls tam, kde je třeba okamžitá konzistence
- Event-driven update pro downstream systémy
- Auditovatelnost transakčních kroků