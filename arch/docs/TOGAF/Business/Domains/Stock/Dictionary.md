# Doménový slovník

| Termín           | Definice                                                                 | Klíčové vlastnosti / Poznámky                                                           |
|----------------- |--------------------------------------------------------------------------|-----------------------------------------------------------------------------------------|
| **StockItem**    | Reprezentace produktu ve skladové doméně.                                | Identita SKU/Variant; drží celkovou zásobu; základ pro dostupnost.                      |
| **StockUnit**    | Nejmenší jednotka zásoby, fyzická nebo logická.                          | Může reprezentovat kus, balení, rezervaci nebo blokaci.                                 |
| **CostSnapshot** | Záznam o nákladové hodnotě zásoby v konkrétním čase.                     | Nemění se zpětně; používá se pro výpočty marže, účetnictví, reporting.                  |
