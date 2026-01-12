# Výběr platformy pro CI/CD + GitOps + AI asistenci

## Kontext

Firma má:

- cca 20 domén  
     - REST API  
     - gRPC API  
     - Workers (2–5 na doménu)  
     - Databáze: PostgreSQL, Elastic, Redis, MongoDB  
- cca 60 BE vývojářů a 25 FE vývojářů  
- 4 prostředí: DEV, TEST, STAGE, PROD  
- On-Prem Kubernetes cluster  
- Vývoj primárně v .NET + Angular  

### Potřeby

- rychlejší release s plnou observabilitou  
- self-recovery a automatický rollback  
- bezvýpadkové nasazení  
- zkrácení lead time od merge po produkci  
- možnost paralelních releasů  
- podpora canary routingu pro A/B testy  
- minimalizace manuální práce v provozu  
- snížení nákladů na incidenty  
- škálovatelnost  
- integrace s Vault / HSM  
- AI analýza provozu (AIOps)  
- AI tooling pro vývojáře (Developer Experience)  

## Zvažované varianty

=== "GitHub Enterprise + GitHub Actions + ArgoCD + GitHub Copilot"

    ## Varianta A – GitHub Enterprise + GitHub Actions + ArgoCD + GitHub Copilot
    | Oblast        | Tool / Funkce                                                   |
    |---------------|-----------------------------------------------------------------|
    | CI/CD         | GitHub Actions (cloud + self-hosted runnery)                    |
    | GitOps        | ArgoCD                                                          |
    | AI tooling    | GitHub Copilot (BE/FE), AI-assisted PR review, generování testů |
    | AI v provozu  | Integrace na externí AIOps (Datadog / Elastic / Grafana ML)     |
    | Security AI   | GitHub Advanced Security *(volitelný placený add-on)*           |
    | Hostování     | On-Prem runnery pro heavy joby, cloud pro lehké joby            |
    
    ### Výhody
    - nejlepší developer experience na trhu  
    - nejsilnější AI tooling pro vývojáře (Copilot)  
    - ArgoCD jako de facto standard GitOps  
    - snadná integrace s Vaultem (External Secrets, CSI)  
    - nízký vendor lock-in (GitHub + ArgoCD + Kubernetes)  
    
    ### Nevýhody
    - vyšší cena za uživatele  
    - pricing GitHub Actions závislý na minutách (nutná optimalizace)  
    - GitHub Advanced Security je samostatně licencovaný add-on  
    
    ### Self-hosted runnery
    - build FE/BE  
    - integrační a e2e testy  
    - image buildy  
    - v praxi snižují cloud CI náklady o 70–90 %  

=== "GitLab Premium + GitLab CI/CD + GitLab Agent"

    ## Varianta B – GitLab Premium + GitLab CI/CD + GitLab Agent
    | Oblast        | Tool / Funkce                                            |
    |---------------|----------------------------------------------------------|
    | CI/CD         | GitLab CI/CD (primárně self-hosted runnery)              |
    | GitOps        | GitLab Agent                                             |
    | AI tooling    | GitLab Duo (základní AI asistence)                       |
    | AI v provozu  | Externí AIOps: Datadog, Dynatrace, New Relic, Elastic ML |
    | Hostování     | On-Prem GitLab + On-Prem runnery                         |
    
    ### Výhody
    - jednotná platforma (Git, CI/CD, registry, security)  
    - silná podpora On-Prem scénářů  
    - dobrá integrace Git + CI/CD  
    
    ### Nevýhody
    - vyšší licenční náklady  
    - AI tooling výrazně méně vyspělý než GitHub Copilot  
    - GitOps méně flexibilní než ArgoCD  
    - vyšší provozní (ops) náročnost GitLab On-Prem  
    
    ### Self-hosted runnery
    - prakticky nutnost (jinak vysoké CI náklady)  
    - dobrá integrace, ale vyšší provozní složitost  
    

=== "Azure DevOps Server + On-Prem build agenti + FluxCD"

    ## Varianta C – Azure DevOps Server + On-Prem build agenti + FluxCD
    
    | Oblast        | Tool / Funkce                                            |
    |---------------|----------------------------------------------------------|
    | CI/CD         | Azure DevOps Pipelines (self-hosted agenti)              |
    | GitOps        | FluxCD                                                   |
    | AI tooling    | Externí (GitHub Copilot není nativní součástí ADO)       |
    | AI v provozu  | Externí AIOps: Datadog, Dynatrace, New Relic, Elastic ML |
    | Hostování     | On-Prem ADO Server + On-Prem build agenti                |
    
    ### Výhody
    - robustní enterprise governance  
    - silná integrace s Microsoft stackem (.NET, AD, HSM)  
    - nejnižší licenční náklady  
    
    ### Nevýhody
    - GitOps není nativní součást platformy  
    - slabší developer experience  
    - AI tooling oddělený a neintegrovaný  
    - vyšší komplexita celkového řešení  
    

## Pricing & Cost Governance

> **Poznámka:** Níže uvedené ceny jsou **modelový odhad** pro srovnání variant.  
> Skutečné náklady závisí na míře využití self-hosted runnerů, OS multiplierech a enterprise smlouvách.

### Předpoklady

- 85 vývojářů  
- 20 domén  
- 35 buildů / den / doména  
- 6 minut průměrný běh  
- 30 dní / měsíc  
- cca **126 000 minut CI běhů / měsíc**  


=== "GitHub – odhad nákladů"

    | Položka                     | Cena / měsíc (USD)       | Poznámka                                                                  |
    |-----------------------------|--------------------------|---------------------------------------------------------------------------|
    | GitHub Enterprise           | 1 785                    | 85 × 21 USD / user                                                        |
    | GitHub Actions (CI/CD)      | 504 – 1 500              | mix cloud + self-hosted; závisí na využití runnerů                        |
    | GitHub Copilot (Dev AI)     | 1 615                    | 85 × 19 USD / user                                                        |
    | GitHub Advanced Security*   | 900 – 1 500              | volitelný add-on pro SAST, secret scanning, dependency review             |
    | Externí AIOps / monitoring  | 3 000 – 8 000            | např. Datadog / Dynatrace / Elastic ML; anomaly detection + RCA           |
    | **Celkem (orientačně)**     | **7 804 – 14 395**       |                                                                           |
    
    > GitHub Advanced Security je volitelný a není zahrnutý v základní licenci.



=== "GitLab Premium – odhad nákladů"


    | Položka                 | Cena / měsíc (USD)       | Poznámka                                              |
    |-------------------------|--------------------------|-------------------------------------------------------|
    | GitLab Premium          | 2 465                    | 85 × 29 USD                                           |
    | CI minutes add-on       | 200                      | průměrný OnPrem provoz (self-hosted)                  |
    | Externí AIOps ML        | 2 500 – 5 000            | Elastic ML / Grafana ML mid-tier                      |
    | **Celkem (orientačně)** | **5 165 – 7 665**        |                                                       |




=== "Azure DevOps – odhad nákladů"

    | Položka                 | Cena / měsíc (USD)       | Poznámka                                      |
    |-------------------------|--------------------------|-----------------------------------------------|
    | ADO Basic licence       | 510                      | 85 × 6 USD                                    |
    | Pipelines               | 400                      | 10 paralelních jobů                           |
    | Externí AIOps ML        | 2 500 – 5 000            | Elastic ML / Grafana ML mid-tier              |
    | **Celkem (orientačně)** | **3 410 – 5 910**        |                                               |


## AI Tooling – přehled

### AI pro vývojáře

| Nástroj        | Schopnosti                                     | Cena            |
|----------------|------------------------------------------------|-----------------|
| GitHub Copilot | kód, testy, refactoring, AI-assisted PR review | 19 USD / user   |
| GitLab Duo     | základní AI asistence                          | v ceně / add-on |
| Externí LLM    | dokumentace, testy, analýza změn               | dle modelu      |

### AI pro provoz (AIOps)

| Nástroj            | Typ / Scope                   | Poznámka                                           | Orientační cena / měsíc |
|--------------------|-------------------------------|--------------------------------------------------- |-------------------------|
| Datadog AIOps      | Full-stack monitoring + AIOps | anomaly detection, RCA, enterprise                 | 3000–8000 USD           |
| Dynatrace Davis AI | Full-stack monitoring + AIOps | automatická RCA, predikce incidentů                | 4000–8000 USD           |
| Elastic ML         | Observability + ML anomalies  | statistická detekce anomálií, cloud/self-managed   | 2000–5000 USD           |
| Grafana ML         | Visualization + ML            | predikce metrik, forecasting, anomaly detection    | 500–5000 USD            |

## Zhodnocení

### GitHub
- nejlepší developer experience a moderní workflow (Actions + ArgoCD)  
- nejsilnější AI tooling pro vývojáře (Copilot + AI-assisted PR review)  
- GitOps integrace s ArgoCD jako de facto standard  
- flexibilní kombinace cloud + self-hosted runnerů  
- **Riziko / nutnost doplnit:** kompletní AIOps/observabilitu je třeba řešit externě (Datadog, Dynatrace, Elastic ML), což zvyšuje TCO  
- vyšší základní licence, ale vysoká přidaná hodnota pro rychlost vývoje a kvalitu  

### GitLab
- silná On-Prem platforma s kompletní integrací (Git + CI/CD + registry + SAST/DAST/secret scanning)  
- nižší závislost na externích add-onech pro monitoring a bezpečnost  
- větší ops náročnost a méně flexibilní GitOps (GitLab Agent vs ArgoCD)  
- cena vyšší kvůli „vše pod jednou střechou“, ale nižší integrační riziko  

### Azure DevOps
- nejnižší základní licence → výhodné pro tight budget / governance-first scénáře  
- robustní enterprise governance a integrace s Microsoft stackem (.NET, AD, HSM)  
- slabší developer experience a menší podpora GitOps  
- AI tooling a AIOps vyžadují externí řešení → vyšší TCO při plném využití  


## Závěr

Volba platformy je strategická investice do:

- rychlosti releasů a lead-time  
- kvality a stability produkce  
- developer experience a týmové produktivity  
- AI schopností pro vývojáře i provoz  
- dlouhodobého TCO a nákladové efektivity  

### Doporučení

**GitHub Enterprise + GitHub Actions + ArgoCD + GitHub Copilot**  
- nejlepší kombinace rychlosti, kvality, AI tooling a GitOps integrace  
- vhodné, pokud je priorita developer experience a agilní release management  

### Alternativní varianty
- **GitLab Premium** → pro silnou On-Prem platformu, vše v jednom a nižší integrační riziko; cena vyšší, ale méně závislá na externích add-onech  
- **Azure DevOps** → pro scénáře s důrazem na governance a nízký budget; DX a AI tooling vyžadují doplnění externě  


