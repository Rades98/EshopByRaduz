# Order
Typ:

Core Domain

Bounded Context:

Order BC

Architektura:

DDD

Event Sourcing (velmi dobrá volba)

Proč:

právní dokument

historie

audit

změny stavu

Aggregate:

Order

📌 jediný master objednávky

## ORDER FLOW

z checkout prijde checkoutReference
zalozi se order s id
provola se stock pro assign s checkout reference a order reference - vrati se units 
  - pokud assign selze, order vola checkout refund s checkout reference a konci s chybou