# Checkout
Typ:

Process Manager / Saga

Bounded Context:

Checkout BC

Role:

řízení nákupního procesu

orchestrace

koordinace BC

Architektura:

event-driven

state machine

žádné agregáty

📌 Checkout nemodeluje realitu
📌 Checkout řídí realitu

## CHECKOUT FLOW

z basket prijde list unit

vnikne zaznam s checkout reference a basket reference

- pokud se stane, ze se kosik zmeni behem aktivniho checkoutu, prijde eventa a checkout se smaze.. 
  - pokud najde zaznam v tabuli s BasketReference, tak ho proste znici

se stock se overi, ze jsou produkty stale dostupne, pokud ne, vrati se chyba

z catalog se spocita cena

do zaznamu se pridaji ceny

pred platbou se vola stock reserve s checkout reference a listem unit - vrati se rezervace
  - pokud reserve selze, vola se checkout cancel s checkout reference a konci s chybou
 
pokud lock prosel, okamzite se dokonci platba pres payments(nebude) a pinguje se order s checkout reference
  - pokud platba selze, vola se checkout cancel s checkout reference a konci s chybou   - ale payments asi nebudu delat.. 