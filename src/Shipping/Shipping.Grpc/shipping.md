# Shipping

Typ:


Doména

Bounded context s komplexními invarianty

Role:

Checkout support / validátor výběru doručení

UX facilitátor – poskytuje možnosti doručení

Charakteristika:

Seznam možností doručení (ShippingOptions + Carriers + PickupPoints)

Dočasné cache (Redis) pro rychlý lookup

Žádné složité business procesy ani agregáty

Eventy: nepovinné, můžeš logovat změny možností doručení, ale není nutné pro funkci

SHIPPING FLOW

gRPC service – žádné REST endpointy

Metody:

GetShippingOptions(cart) → vrací seznam možností pro daný košík

Každá možnost obsahuje: carrier, delivery_type (adresa, výdejní místo), delivery_time, price

Pokud typ doručení je “pobočka”, vrátí i seznam dostupných pickup points

ValidateShippingOption(cart, shippingOptionId, pickupPointId?) → ověří, jestli je výběr platný

Data storage:

ShippingOptions (tabulka)

Carriers (tabulka)

PickupPoints (tabulka)

Volitelně cache v Redis pro rychlé dotazy

Integrace s checkout:

Checkout si pull-based dotahuje možnosti přes gRPC

Validace probíhá při potvrzení výběru