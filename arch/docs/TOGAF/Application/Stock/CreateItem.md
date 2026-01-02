# CreateStockItem

- EventConsumer přijme externí událost ProductCreatedEvent a uloží ji do tabulky Inbox v databázi StockDB.
- InboxProcessor pravidelně čte nové záznamy z Inboxu, transformuje událost na příkaz CreateStockItemCommand a předává ho handleru.
- CreateStockItemHandler:
     - Zkontroluje, zda položka již existuje.
     - Pokud ne, vytvoří pomocí `StockItemAggregate` metodou `Create` položku na základě doménových pravidel definovaných agregátem.
          - SKU a Variant nesmí nabývat prázdných hodnot
          - SKU musí odpovídat regexu 
             ``` regex
             (^[a-zA-Z]{3}-\d{3,8}$)
             ```
          - Variant musí mít maximální délku 50 znaků
     - V rámci jedné transakce uloží novou položku do tabulky `StockItems` a zároveň vygeneruje událost `StockItemCreatedEvent`, kterou uloží do tabulky `Outbox`.
- OutboxProcessor čte nové události z Outboxu a publikuje je do event systému

## View
![](StockItemCreation.drawio)