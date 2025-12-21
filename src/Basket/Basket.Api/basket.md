# BASKET FLOW
FastApi - zadna architektura

- POST   users/{userId}/basket              → vytvori kosik
- PUT    users/{userId}/basket/{basketId}   → upravuje polozky
- GET    /basket/{userId}                   → nacteni celeho kosiku

DB je redis

pri kazde zmene se emituje event BasketChanged(UserId)

checkout je pullbased.. takze asi gRPC si dotahne sam pri nejakem create


