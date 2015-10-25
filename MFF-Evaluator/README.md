CZECH VERSION only

Zadani:

```
Napište program, který dostane na prvním řádku standardního vstupu výraz zapsaný v preorder notaci, výraz vyhodnotí a výsledek vypíše na standardní výstup. Ve výrazu se mohou vyskytovat celá kladná čísla, která se vejdou do 32-bit. integeru se znaménkem (tj. menší než 231 = 2147483648), binární operátory +, -, * a / a operátor unárního minus ~. Operátory i čísla jsou odděleny mezerami. Výsledek vypište jako celé číslo se znaménkem v dekadickém formátu.

Při vyhodnocování výrazu používejte výhradně celočíselnou aritmetiku. Všechny mezivýsledky se vejdou do 32-bit. integeru se znaménkem. Pokud by v průběhu výpočtu došlo k přetečení (mezivýsledek se nevejde do intu se znaménkem), vypište jako výsledek řetězec "Overflow Error". Pokud dojde k dělení nulou vypište chybový řetězec "Divide Error". A konečně pokud zjistíte, že zápis výrazu je z jakéhokoli důvodu neplatný (objevují se v něm neznámé tokeny, nedodržuje preorder formát apod.), vypište chybovou hlášku "Format Error".

Vyhodnocované výrazy jsou relativně krátké, takže není potřeba optimalizovat řešení na výkon. Místo toho bude kladen zvláštní důraz na kvalitu objektového návrhu a znovupoužitelnost kódu. Předpokládejte, že váš program bude sloužit jako základ knihovny pro práci s výrazy, tj. uživatelé takové knihovny budou typicky na jednom vstupním výraze provádět mnoho různých operací (opakované vyhodnocování, zjednodušování částí výrazu, apod.). Za opravdu kvalitní řešení můžete jednak dostat bonusové body navíc a jednak se vám to může vyplatit při řešení dalších domácích úkolů.


Příklady:
Vstup: + ~ 1 3
Výstup: 2

Vstup: / + - 5 2 * 2 + 3 3 ~ 2
Výstup: -7

Vstup: - - 2000000000 2100000000 2100000000
Výstup: Overflow Error

Vstup: / 100 - + 10 10 20
Výstup: Divide Error

Vstup: + 1 2 3
Výstup: Format Error

Vstup: - 2000000000 4000000000
Výstup: Format Error

Poznámka: poslední příklad vrací Format Error, protože číslo 4000000000 nevyjde jako mezivýsledek, ale je přímo zapsané ve výrazu.
```