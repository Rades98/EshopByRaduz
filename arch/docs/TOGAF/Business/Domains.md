# Přehled domén

Tento pohled představuje celkovou mapu domén a klíčových procesů napříč systémem.  
Slouží jako orientační vrstva, která propojuje business logiku s technickými doménami — od marketingu přes objednávky až po reklamace, platby a reporting.

Každý proces (např. „Vytvoření objednávky“, „Platba“, „Reklamace“) je navázán na konkrétní doménu, která ho umožňuje realizovat.  
Domény jsou rozděleny na **core**, **supporting** a **generic**, podle jejich významu v architektuře.

> Cílem tohoto pohledu není detailní specifikace, ale **navigační přehled**, který ukazuje, jak jednotlivé části systému spolupracují na doručení hodnoty.

Tento pohled tvoří základ pro další, konkrétnější schémata per doména, kde jsou procesy rozpracovány do větší hloubky (např. Stock).

## Architektonické principy
- Domény nejsou jen technické silo — jsou nositeli business významu  
- Procesy jsou explicitně mapovány na domény, ne na technické vrstvy  
- Každá doména má jasně definovanou odpovědnost a hranice  
- Tento pohled slouží jako výchozí bod pro orientaci v systému

---

## Archimate view

<div class="iframe-wrapper">
  <iframe 
    src="../../Archi/id-a684dda0a38c46dd8ec32300560e4317/views/id-300711c3effb4db1bbf4b914e740661c.html" loading="lazy">
  </iframe>
</div>
