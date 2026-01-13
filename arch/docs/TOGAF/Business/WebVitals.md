# Web Vitals jako KPI digitálních kanálů

**Účel:**  
Web Vitals definují měřitelné ukazatele kvality digitálního kanálu.  
Slouží jako **KPI pro business**, **požadavky pro architekturu** a **cílové hodnoty pro delivery týmy**.

---

## Definice metrik

| Metrika | Název                      | Co měří                                     | Vazba na KPI                      | Cílová hodnota                        |
|---------|----------------------------|---------------------------------------------|-----------------------------------|---------------------------------------|
| LCP     | Largest Contentful Paint   | Rychlost načtení hlavního obsahu stránky    | Rychlost → konverze → revenue     | ≤ 2.5 s (mobile)<br>≤ 1.2 s (desktop) |
| CLS     | Cumulative Layout Shift    | Stabilitu layoutu a vizuální důvěru         | Stabilita → bounce rate → důvěra  | ≤ 0.1                                 |
| INP     | Interaction to Next Paint  | Rychlost reakce UI na uživatelské interakce | Interaktivita → NPS → loajalita   | ≤ 200 ms                              |


---

## Business dopady

| Metrika | Business dopad                            |
|---------|-------------------------------------------|
| **LCP** | –7 až –10 % konverzí za každou +1 s       |
| **CLS** | +6 až +17 % opuštění košíku při CLS > 0.1 |
| **INP** | –50 % NPS při INP 500 ms                  |

---

## Dopad na architekturu

| Metrika | Dopad na architekturu                                 |
|---------|-------------------------------------------------------|
| **LCP** | CDN, caching, image pipeline, edge rendering          |
| **CLS** | UI governance, design system, lazy loading            |
| **INP** | Framework, granularita komponent, hydration strategie |

---

## Cílové hodnoty (Target KPIs)

| Metrika       | Target   | Threshold | Alert  |
|---------------|----------|-----------|--------|
| **LCP (p75)** | ≤ 2.5 s  | 3.5 s     | 4.0 s  |
| **CLS (p75)** | ≤ 0.1    | 0.15      | 0.25   |
| **INP (p75)** | ≤ 200 ms | 300 ms    | 500 ms |

---

# Shrnutí
Web Vitals jsou **hlavní KPI digitálních kanálů**.  
Jsou závazné pro business, architekturu i delivery.  
Jsou měřitelné, auditovatelné a přímo navázané na revenue, NPS a náklady.

