# Pricing
Typ:

Supporting Subdomain

Bounded Context:

Pricing BC

Proč:

ceny mají vlastní pravidla

akce, slevy, DPH, měna

časovou platnost

často mění pravidla

Architektura:

Clean / Domain Lite

event-driven read model

žádné vazby na stock

Zodpovědnosti:

price calculation

promotion rules

tax rules

📌 Catalog nikdy nepočítá cenu
📌 Catalog maximálně drží PriceSnapshot pro listing