# Slovník pojmů
| Pojem                     | Definice                                                                                                            |
|---------------------------|---------------------------------------------------------------------------------------------------------------------|
| **Stakeholder**           | Osoba nebo skupina, která má zájem na výsledku domény nebo procesu. Ovlivňuje požadavky, priority a očekávání.      |
| **Doména**                | Ucelená oblast odpovědnosti s vlastním jazykem, pravidly a cíli. Řeší konkrétní část podnikové reality.             |
| **Core doména**           | Klíčová oblast, která vytváří hlavní hodnotu firmy. Obsahuje unikátní know‑how a rozhodující pravidla.              |
| **Supporting doména**     | Podpůrná oblast, která umožňuje core doménám fungovat. Není unikátní, ale je specifická pro daný kontext.           |
| **Generic doména**        | Obecná, standardizovatelná oblast bez unikátních pravidel. Kompozice, agregace, prezentace                          |
| **Channel**               | Způsob, jakým se zákazník nebo systém dostává k funkcionalitě. Reprezentuje vstupní bod interakce - vstupní bod     |
| **Snapshot**              | Časově konzistentní výřez dat z jedné nebo více domén, určený pro čtení nebo validaci.                              |
| **Read model**            | Optimalizovaná struktura dat pro čtení, složená ze snapshotů. Nemá vlastní logiku ani autoritu.                     |
| **Authoritative source**  | Doména nebo systém, který je jediným zdrojem pravdy pro konkrétní typ dat nebo rozhodnutí.                          |
| **Identity**              | Jednoznačný identifikátor entity v rámci domény. Může být interní nebo externí.                                     |
| **Event**                 | Zaznamenaná událost, která popisuje změnu stavu nebo významný okamžik v doméně.                                     |
| **Command**               | Požadavek na změnu stavu domény. Vychází z rozhodnutí uživatele nebo systému.                                       |
| **Aggregate**             | Jednotka konzistence v doméně. Obsahuje pravidla, stav a rozhodovací logiku.                                        |
| **Bounded Context**       | Logicky ohraničený prostor, kde má jazyk a pravidla domény jednoznačný význam.                                      |
| **Inbox / Outbox**        | Pattern pro spolehlivou asynchronní komunikaci mezi doménami.                                                       |
| **Choreografie**          | Styl integrace, kde domény reagují na události bez centrálního řízení.                                              |
| **Orchestrace**           | Styl integrace, kde centrální komponenta řídí tok mezi doménami.                                                    |
| **Eventual consistency**  | Stav, kdy se data mezi doménami synchronizují časem, ne okamžitě.                                                   |
| **Audit log**             | Nezpochybnitelný záznam o událostech, rozhodnutích a změnách v doméně.                                              |
| **Contract**              | Dohoda mezi doménami o struktuře a významu dat (např. event schema).                                                |
| **Replay**                | Opětovné zpracování historických událostí pro rebuild nebo analýzu.                                                 |
| **Dead Letter Queue**     | Místo, kam se ukládají nezpracovatelné zprávy nebo eventy.                                                          |
| **Idempotence**           | Vlastnost operace, která má stejný výsledek při opakovaném provedení.                                               |
| **TCO**                   | Total Cost of Ownership - celkové náklady vlastnictví                                                               |
| **KPI**                   | Key Performance Indicator - klíčový ukazatel výkonnosti.                                                            |
