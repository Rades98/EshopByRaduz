# Basket – Supporting Domain

- Ephemeral košík, validace položek vůči Catalogu  

## Klíčové procesy

- Správa dočasného košíku
- Validace položek proti Catalogu
- Synchronizace s:
    - Catalog
    - Checkout

## Architektonické principy

- Ephemeral storage (Redis)
- Light validace přes catalog
- Oddělení transientních dat od core domén
- Podpora rychlého rollbacku a TTL