# Stock
Typ:

Core Domain

Bounded Context:

Stock BC

Proč:

řeší dostupnost

serializaci

rezervace

alokaci

je kritický pro business

Architektura:

DDD

Aggregates

Domain Events

Outbox

Event publisher

Hlavní agregáty:

StockItemAggregate

StockUnit

✔ master dostupnosti
✔ source of truth pro fyzickou existenci zboží