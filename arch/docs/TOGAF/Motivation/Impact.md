# Přínosy architektury

| Problém                                               | Řešení                                                                                           |
| ------------------------------------------------------| ------------------------------------------------------------------------------------------------ |
| Nejasné vlastnictví dat                               | Rozdělení do domén, které mají své BC s agregáty                                                 |
| Náročné zavádění nových featur                        | Jasné určení odpovědnosti vede k jednoduššímu plánování zavedení feature                         |
| Složité určování vlastníka dat                        | U DDD je vlastník jasný, nikdo jiný nesmí data mutovat, ostatní mohou jen konzumovat/žádat změnu |
| Škálovatelnost                                        | Rozdělení do domén a domény do služeb (Core + BFFs) umožňuje atomičtejší škálování               |
| Zbytečně těžké dotazy                                 | Eventual consistency vs authoritative state                                                      |
| Náročné Integrace a synchronizace dat                 | Event dtiven + gRPC pro jasné stanovení odpovědnosti                                             |
| Rozjíždění dat v případě více participujících procesů | V DDD je jeden vlastník a tedy jedna autorita                                                    |
| Roztroušená pravidla po systému                       | DDD Agregát s eventami, kde lze jasně definovat obraz businesové logiky                          |
| Nejasné stavy dat/workflows                           | Každá změna emituje event pro synchronizaci ostatních služeb                                     |
| Chaos v komunikaci mezi týmy a v interpretaci práce   | Domény mají jasné hranice, nevolá se cizí metoda, ale API, případně se konzumuje event           |
| Náročná testovatelnost, větší chybovost               | Menší celky s jasnými hranicemi (event/gRPC)                                                     |
| Pomalý onboarding                                     | Domény s Eventami a Agregáty jasně definují pravidla a jednoduše se zavádí nové                  |
| Přetížení monolitu                                    | Doménový core + BFF per client (e-shop/erp etc)                                                  |
| Složitý audit a monitoring změn                       | Core služby s jasnými pravidly                                                                   |
| Náročný výpočet náročnosti                            | Menší BC v DDD jsou prediktivní - možnost rzchlejších změn a iterací, core změny dělá jeden tým  |
| Složité plánování roadmapy                            | Dá se plánovat a ověřovat po doménách a ne jako celek.                                           |
