### Endpoint pro načítání dat

Tato aplikace poskytuje endpoint pro načítání dat buď z externí API, nebo pokud není API dostupné, z lokálního CSV souboru.

#### Endpoint

**GET /data**

Tento endpoint se pokouší načíst data z externí API na adrese `http://wea.nti.tul.cz:1337/data`. Pokud není API dostupné nebo vrátí stavový kód 404, služba načte data z CSV souboru, jehož cesta je nastavena v konfiguraci aplikace.

#### Postup:
1. Aplikace se pokusí načíst data z API pomocí služby `LoadFromApiService`.
2. Pokud API vrátí 404 nebo není dostupné, zkontroluje, zda databáze obsahuje knihy.
3. Pokud databáze neobsahuje žádné knihy, zavolá se `LoadFromCSVService`, která načte data z CSV souboru.
4. Data se uloží do tabulky `Books` v `DatabaseContext`.

#### Příklad odpovědi

Status: 200 OK
