# Nákup

Poznámky:

 - Checkout neobsahuje business logiku cen ani skladových pravidel
 - Pricing a Regulatory jsou autoritativní zdroje rozhodnutí
 - Stock provádí explicitní rezervaci až při potvrzení objednávky
 - Order vzniká na základě eventu od checkoutu


```mermaid
sequenceDiagram
    autonumber

    participant User as Zákazník
    participant BasketAPI as Košík (REST)
    participant CatalogAPI as Katalog (gRPC)
    participant CheckoutAPI as Checkout
    participant PricingAPI as Ceník (gRPC)
    participant RegulatoryAPI as Regulace (gRPC)
    participant StockAPI as Sklad (gRPC)
    participant PaymentAPI as Platby (REST)
    participant ShippingAPI as Doprava (REST)
    participant OrderAPI as Objednávky

    User->>BasketAPI: Přidat položku do košíku

    BasketAPI->>CatalogAPI: Ověřit, zda lze položku přidat (light validace)
    CatalogAPI-->>BasketAPI: OK

    User->>CheckoutAPI: Zahájit checkout
    CheckoutAPI->>BasketAPI: Načíst položky košíku
    BasketAPI-->>CheckoutAPI: Položky

    CheckoutAPI->>User: Vyžádat adresu
    User-->>CheckoutAPI: Poskytnout adresu

    CheckoutAPI->>PricingAPI: Vyžádat cenový přehled
    PricingAPI->>RegulatoryAPI: Vyžádat daňová pravidla
    RegulatoryAPI-->>PricingAPI: Daňová pravidla
    PricingAPI-->>CheckoutAPI: Cenový přehled

    CheckoutAPI->>RegulatoryAPI: Ověřit omezení
    RegulatoryAPI-->>CheckoutAPI: OK

    CheckoutAPI->>StockAPI: Ověřit dostupnost zboží
    StockAPI-->>CheckoutAPI: OK

    User->>PaymentAPI: Vybrat platební metodu
    PaymentAPI-->>User: Potvrzení výběru

    User->>CheckoutAPI: Uložit vybranou platební metodu
    CheckoutAPI->>PaymentAPI: Ověřit existenci platební metody (gRPC)
    PaymentAPI-->>CheckoutAPI: OK

    User->>ShippingAPI: Vybrat způsob dopravy
    ShippingAPI-->>User: Potvrzení výběru

    User->>CheckoutAPI: Uložit vybraný způsob dopravy
    CheckoutAPI->>ShippingAPI: Ověřit existenci způsobu dopravy (gRPC)
    ShippingAPI-->>CheckoutAPI: OK

    User->>CheckoutAPI: Potvrdit objednávku („Objednat“)

    CheckoutAPI->>StockAPI: Rezervovat položky
    StockAPI-->>CheckoutAPI: Rezervace OK

    CheckoutAPI->>OrderAPI: Vytvořit objednávku (CheckoutCompletedEvent)
    OrderAPI->>StockAPI: Převzít rezervace (order-allocation)
    StockAPI-->>OrderAPI: Převod OK

    OrderAPI-->>CheckoutAPI: OrderCreated
    CheckoutAPI-->>User: Objednávka potvrzena

```