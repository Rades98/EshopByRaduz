# Výkon & metriky

## 1) LCP - konverze při rychlosti načítání

Web Vitals představují měřitelné ukazatele kvality UX. V EA kontextu fungují jako **KPI pro digitální kanály**.

### 1.1 Konverze vs. rychlost načtení (LCP)

LCP je čas vykreslení největšího prvku v aktuálním viewpointu. Nejčastěji se jedná o blokové prvky, ve kterých je 
hlavní obsah. Nejčastěji se jedná o text a obrázky. V našem případě to může být landing page s menu a listem nejvíce atraktivních produktů, 
či detail produktu, kde dominuje náhledové foto s galerií a základní informace o produktu.

> Dobré LCP by dle Google mělo proběhnout maximálně do 2.5 sekundy.

| Hodnota LCP       | Mobile  | Desktop |
|-------------------|---------|---------|
| Dobrá             | ≤ 2.5 s | ≤ 1.2s  |
| Vyžaduje zlepšení |	≤ 4,0 s | ≤ 2,4s  |
| Špatná	          | > 4,0 s | > 2,4s  |

#### Konverze vs rychlost načtení (LCP)

```vegalite

{
  "description": "Konverze vs rychlost načtení (LCP)",
  "data": {
    "values": [
      {"time": "1 s", "conversion": 32},
      {"time": "2 s", "conversion": 25},
      {"time": "3 s", "conversion": 18},
      {"time": "4 s", "conversion": 10}
    ]
  },
  "mark": {"type": "line", "point": true, "tooltip": true},
  "encoding": {
    "x": {"field": "time", "type": "nominal"},
    "y": {"field": "conversion", "type": "quantitative"}
  }
}

```

### Interpretace
- LCP je **leading indicator** pro revenue.
- Každá +1s = **–7 až –10 % konverzí**.
- U e‑shopu s obratem 100M Kč/rok znamená zpomalení z 2s → 3s ztrátu **7–10M Kč ročně**.
- V architektuře to ovlivňuje:
     - volbu CDN,
     - caching strategii,
     - image pipeline,
     - edge rendering.

---

## 2) Stabilita (CLS) – dopad na důvěru a UX

CLS je metrika, která měří největší skóre posunu layoutu (layout shift) pro každý neočekávaný posun prvků, ke kterému dojde během celého životního cyklu stránky.
Layout shift nastane kdykoli, když viditelný prvek změní svou pozici mezi dvěma po sobě jdoucími renderovanými snímky (frames).

| Hodnota CLS       | Mobile  | Desktop |
|-------------------|---------|---------|
| Dobrá             | ≤ 0,1   | ≤ 0,1   |
| Vyžaduje zlepšení | ≤ 0,25  | ≤ 0,25  |
| Špatná            | > 0,25  | > 0,25  |

| CLS skóre | Interpretace                                             |
| --------- | -------------------------------------------------------- |
| 0         | Žádný posun layoutu – ideální UX                         |
| 0,05      | Malé posuny – prakticky nezaznamenatelné                 |
| 0,1       | Maximum pro „dobré“ UX (doporučuje Google)               |
| 0,25      | Výrazné posuny – vyžaduje zlepšení                       |
| >0,25     | Špatná stabilita layoutu – negativní UX a vyšší opuštění |

### CLS vs bounce rate na landing page
```vegalite
{
  "description": "CLS vs bounce rate na landing page",
  "data": {
    "values": [
      {"cls": "0.05", "bounce": 32},
      {"cls": "0.1", "bounce": 40},
      {"cls": "0.25", "bounce": 58}
    ]
  },
  "mark": {"type": "bar", "tooltip": true},
  "encoding": {
    "x": {"field": "cls", "type": "nominal"},
    "y": {"field": "bounce", "type": "quantitative"}
  }
}

```

### Interpretace
- CLS je metrika **perceived quality**.
- Při CLS > 0.1 roste opuštění landing page o **+6 až +17 %**.
- Dopad na architekturu:
    - řízení layoutu,
    - lazy loading strategie,
    - governance nad komponentami UI,
    - design system maturity.

---

## 3) Interaktivita (INP/FID) – dopad na NPS a loajalitu

```vegalite
{
  "description": "INP vs NPS",
  "data": {
    "values": [
      {"inp": "100ms", "nps": 62},
      {"inp": "200ms", "nps": 55},
      {"inp": "300ms", "nps": 41},
      {"inp": "500ms", "nps": 28}
    ]
  },
  "mark": {"type": "line", "point": true, "tooltip": true},
  "encoding": {
    "x": {"field": "inp", "type": "nominal"},
    "y": {"field": "nps", "type": "quantitative"}
  }
}
```

### Interpretace
- INP je metrika **responsiveness** – klíčová pro vnímanou kvalitu.
- Při INP 500ms klesá NPS o **50 %**.
- Dopad na architekturu:
     - volba frameworku,
     - granularita komponent,
     - client/server rendering,
     - prioritizace interakcí.

---

## 4)  Provozní metriky – dopad na náklady

### 4.1 Error rate → náklady na support

```vegalite
{
  "description": "Error rate vs náklady na support",
  "data": {
    "values": [
      {"error": "0.5%", "cost": 300000},
      {"error": "1%", "cost": 450000},
      {"error": "2%", "cost": 700000}
    ]
  },
  "mark": {"type": "bar", "tooltip": true},
  "encoding": {
    "x": {"field": "error", "type": "nominal"},
    "y": {"field": "cost", "type": "quantitative"}
  }
}
```

### interpretace
- Každé +1 % error rate = **+150–250k Kč měsíčně**.
- Dopad na architekturu:
     - resilience patterns (retry, circuit breaker),
     - observability maturity,
     - SLA/SLO definice,
     - incident governance.

---

## 5) Metriky jako řídicí mechanismus

| Oblast        | Metrika       | Business dopad           | EA dopad                       |
|---------------|---------------|--------------------------|--------------------------------|
| Rychlost      | LCP           | – 7–10 % konverzí / +1s  | CDN, caching, edge rendering   |
| Stabilita     | CLS           | + 6–17 % opuštění košíku | UI governance, design system   |
| Interaktivita | INP           | – 50 % NPS při 500ms     | Framework, rendering strategie |
| Spolehlivost  | Error rate    | + 150–250k Kč/měsíc      | Resilience, SLO, monitoring    |
| Výkon API     | Response time | + 20 % cloud náklady     | Optimalizace, škálování        |

---

## 6) Vazba na ADR a TOGAF

### 6.1 ADR
Výkonové metriky jsou vstupem pro rozhodnutí typu:

- [ADR_00023](../../ADR/ADR_00023): Volba CDN -> snížení LCP o 30–50 %
- [ADR_00024](../../ADR/ADR_00024): Prevence degradace služeb -> snížení error rate
- [ADR_00025](../../ADR/ADR_00025): Přechod na edge rendering -> zlepšení INP o 20–40 %

### 6.2 TOGAF
- **Business Architecture**: [Web Vitals jako KPI digitálních kanálů](../../TOGAF/Business/WebVitals)  
- **Application Architecture**: volba frameworků, rendering strategií  
- **Technology Architecture**: CDN, caching, observability stack  

---

## 7) Governance & reporting

Doporučené dashboardy:

   - Web Vitals (LCP, CLS, INP)
   - API performance (p95, p99)
   - Error rate & incidenty
   - Business dopady (konverze, NPS, revenue leakage)

Metriky se reportují:

   - týdně (operativa),
   - měsíčně (střední management),
   - kvartálně (board).

