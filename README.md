Tervezési döntések
Domain modell
-------------
A Barcode, Price és Quantity mezőket opcionálissá tettem.

Ennek célja, hogy a termék egy kezdetleges állapotban is rögzíthető legyen, majd a hiányzó adatok később kitölthetők legyenek. Egy komplexebb rendszerben érdemes lehet egy ProductStatus mező bevezetése is, amely jelzi a termék adatainak teljességét.

Validáció
---------
A validáció elsődlegesen a szervizrétegben történik mentéskor.

Az alkalmazás jelenleg az alábbi ellenőrzéseket végzi:
- a Name mező kitöltése kötelező
- a Name mező hossza 3 és 100 karakter között lehet
- a Name alapján nem hozható létre duplikált termék
- a Barcode mező opcionális
- amennyiben megadásra kerül, hossza 3 és 50 karakter között lehet
- a Barcode alapján nem hozható létre duplikált termék
- a Price mező opcionális, de negatív érték nem menthető
- a Quantity mező opcionális, de negatív érték nem menthető

Érvénytelen adatok esetén a felhasználó értesítést kap a termék adatlapján.

Adatkezelés
-----------
Az alkalmazás az adatkezelést Entity Framework Core segítségével valósítja meg.

A választás oka:
- gyors fejlesztési lehetőség
- egyszerű adatkezelési igények
- adatbázis-független architektúra
- későbbi technológiai váltás egyszerűsége

A repository és service réteg megfelelő alapot biztosít arra, hogy az alkalmazás később REST API alapú háttérrendszerhez kapcsolódjon, például MSSQL adatbázissal.

Lehetséges továbbfejlesztések
-----------------------------
- naplózás (logging) bevezetése
- hitelesítés és jogosultságkezelés
- role alapú láthatóság és módosíthatóság
- fejlettebb listakezelés:
	- oszlop szerinti rendezés
	- több feltételes szűrés
	- dupla kattintásos szerkesztés
- automatizált tesztek készítése
- újrahasznosítható UI komponensek kialakítása

Tesztelés
---------
Az alkalmazást Windows és Android platformokon teszteltem.
A két platform több esetben eltérően viselkedik, különösen:

- beviteli vezérlők működése
- layout kezelés
- egyes natív UI elemek megjelenése

A tesztelés során az alkalmazás mindkét platformon megfelelően működött.