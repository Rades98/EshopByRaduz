# Stock Supporting Domain

- Správa položek, rezervací a dostupnosti pro objednávky a fulfillment.  

## Klíčové procesy
- Založení položky a její metadata  
- Rezervace a validace dostupnosti  
- Přiřazení položky k objednávce  
- Expedice a zpětný příjem  
- Synchronizace dalšími doménami:
    - Product
    - Procurement
    - Accounting
    - Checkout
    - Order
    - Fulfillment

## Architektonické principy
- Položka jako jednotka s vlastním stavem a historií  
- Explicitní rezervace s referencí na checkout  
- Oddělení synchronizačních toků od core logiky  
- CostSnapshotBuffer pro synchronizaci mezi Accounting a fyzickou realitou skladu

> Tento pohled slouží jako základ pro pochopení, jak systém pracuje s fyzickými položkami a jejich dostupností v reálném čase.

---

## Archimate view

<div class="iframe-wrapper">
  <iframe 
    src="../../../../Archi/id-a684dda0a38c46dd8ec32300560e4317/views/id-9d271ea16f4f4bd69d43dc69b6f64b15.html" loading="lazy">
  </iframe>
</div>
