# Automatizace

Automatizace není samostatná komponenta, ale schopnost architektury zabírající celý vývoj + údržbu, která se projevuje napříč doménami i procesy.
Místo statického pohledu na vrstvy je zde použit procesní pohled, který ukazuje automatizované flow. 
Tato sekce definuje rozsah povinné automatizace systému napříč celým jeho životním cyklem – 
od založení, přes změny zdrojového kódu až po provoz v produkčním prostředí.

## Průřezové principy (platí pro všechny sekce)

- Shift‑Left Automation – testy, bezpečnost a  kvalita se posouvají co nejblíže ke zdrojovému kódu.
- Policy‑as‑Code / Governance‑as‑Code – pravidla nejsou v Confluence, ale v repozitářích.
- Idempotence & Reproducibility – každý krok lze spustit opakovaně se stejným výsledkem.
- Event‑Driven Automation – automatizace reaguje na události (commit, alert, drift, incident).
- No Manual Steps Without Exception – manuální zásah musí být výslovně zdůvodněn a schválen.

--- 

1. [Change & Delivery Automation (CI/CD)](CICD/Cicd.md)
2. Environment & Infrastructure Automation
3. Runtime Operations Automation
4. Observability & Incident Automation
5. Security & Compliance Automation
