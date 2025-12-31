# Založení Itemu

## Účel
Zajistit, že pro každý produkt, který je uvedený do nabídky, existuje odpovídající položka zásob (Stock Item), která reprezentuje jeho fyzickou nebo disponibilní existenci v systému.
Stock Item je business entita, která umožňuje:

- sledovat dostupnost,
- provádět rezervace,
- plánovat výdej,
- řídit doplňování.

---

## Spouštěč
Produkt je zalistován v Product doméně

---

## Kroky procesu

1. Produkt je zalistován
     - Produkt má definované SKU, varianty, parametry.

2. Stock doména reaguje na informaci o produktu.
     - Ověří, zda pro daný SKU/Variant existuje odpovídající položka zásob.
     - Stock Item reprezentuje možnost držet, rezervovat a vydávat daný produkt.
     -  Business pravidla:
         - Každý aktivní produkt musí mít svůj Stock Item.
         - Stock Item vzniká i tehdy, když je počáteční množství 0 (např. pre‑sale, backorder).

3. Pokud položka zásob neexistuje, vzniká nový Stock Item.

     - Vytvoří se entita reprezentující zásobu produktu.
     - informace potřebné pro budoucí rezervace a dostupnost

4. Stock Item je zařazen do procesů dostupnosti.

     - Lze jej zahrnout do výpočtů dostupnosti.
     - Lze jej rezervovat.
     - Lze jej plánovat pro výdej.

5. Informace o existenci Stock Itemu je zpřístupněna ostatním doménám.

     - Checkout může dotazovat dostupnost.
     - Fulfillment může plánovat výdej.
     - Pricing může pracovat s dostupností.
     - Catalog staví snapshot pro budoucí indexaci

Výsledek:

- Produkt je založen a připraven pro integraci do skladového a prodejního procesu.
- Systém umí:
     - zobrazit dostupnost,
     - přijímat rezervace,
     - plánovat výdej,
     - řídit replenishment.

---