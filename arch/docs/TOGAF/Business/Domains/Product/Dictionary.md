# Doménový slovník

| Termín             | Definice                                                                 | Klíčové vlastnosti / Poznámky                                                           |
|------------------- |--------------------------------------------------------------------------|-----------------------------------------------------------------------------------------|
| **Product**        | Reprezentace produktu - SKU, Obrázek, metadata, lifecycle                | Identita SKU, drží informace o produku, základ pro product view                         |
| **ProductVariant** | Reprezentace varianty - lifecycle, metadata                              | Reprezentuje variantu produktu                                                          |

## Eventy

| Název                     | Definice                                                                 |
|-------------------------- |--------------------------------------------------------------------------|
| **ProductCreated**        | Založen nový produkt - veškerá data pro snapshot                         |
| **ProductUpdated**        | Produkt upraven - Definice fieldu a hodnota (jak data, tak lifecycle)    |
| **ProductVariantCreated** | Založena nová varianta - veškerá data pro snapshot                       |
| **ProductVariantUpdated** | Varianta upravena - Definice fieldu a hodnota (jak data, tak lifecycle)  |