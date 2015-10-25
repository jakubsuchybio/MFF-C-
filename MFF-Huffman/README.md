CZECH VERSION only

Zadani Huffman I:

```
Napište program, který dostane jako argument jméno vstupního souboru. Přečte všechna data ze vstupního souboru, na jejich
základě postaví Huffmanův strom a ten vypíše v prefixovém tvaru na standardní výstup. Pokud nedostane právě jeden argument,
vypište na std. výstup řetězec "Argument Error". Pokud vstupní soubor nejde z jakéhokoli důvodu otevřít nebo z něj nelze
číst, vypište na std. výstup řetězec "File Error". Vstupní soubor může mít libovolný formát (tedy může být klidně binární).
Pro upřesnění: Znakem od této chvíle dál myslíme nějaký symbol reprezentující 1 byte (dívejte se tedy na data spíše jako na
posloupnost bytů než na text).

Huffmanův strom je binární strom, v jehož listech se nachází všechny znaky, které se vyskytly ve vstupním souboru alespoň
jednou. Váhou znaku pak rozumíme počet výskytů daného znaku ve vstupním souboru. Například vstupní data "xxyz" obsahují znaky
"x", "y" a "z", přičemž "x" má váhu 2 a "y" i "z" mají váhu 1. Vnitřní uzly stromu neobsahují žádné znaky a jejich váha je
rovna součtu vah obou jejich synů (každý vnitřní uzel má vždy oba syny).

Při stavbě stromu postupujeme následovně. Nejprve si připravíme všechny listy na základě analýzy vstupních dat a připravíme
si z nich les jednoprvkových stromů. Dokud je v lese více než jeden strom, vezmeme vždy dva nejlehčí stromy z lesa, spojíme
je pod nově vytvořený kořen (jehož váhu spočítáme jako součet vah listů) a vložíme jej zpět do lesa. Při spojování je uzel s
menší váhou vždy vlevo. Pokud při výběru uzlů má více uzlů stejnou váhu, aplikují se následující pravidla (v pořadí, ve
kterém jsou napsána). Tato pravidla také rozhodují, který uzel by měl být připojen vlevo a který vpravo, pokud mají oba
stejnou váhu.

listy jsou lehčí než vnitřní uzly
mezi listy mají přednost ty, které mají menší hodnotu v nich uloženého znaku
mezi vnitřními uzly mají přednost ty, které vznikly dříve v průběhu algoritmu
Vzniklý strom vypište na standardní výstup v prefixovém tvaru. Listy budou vypsány ve formátu *SYMBOL:VAHA, kde SYMBOL je kód
znaku zapsaný decimálně (číslo od 0 do 255) a váha je počet výskytů daného znaku ve vstupním souboru (rovněž zapsaný
decimálně). Vnitřní uzly vypište ve tvaru VAHA LEVY PRAVY, přičemž váha je opět celé číslo zapsané decimálně a LEVY resp.
PRAVY je rekurzivní výpis levého resp. pravého podstromu a mezi jednotlivými částmi je právě jedna mezera. Pokud je délka
vstupního souboru 0 bytů, tak program na standardní výstup nevypíše nic.

Důležitá poznámka: Váš program by měl být bez problémů schopen pracovat minimálně s 1 TB (přesněji tedy 1 TiB) vstupními
soubory. K dispozici máte ovšem pouze O(1) paměti.


Příklad:
$>program.exe simple.in
simple.in
    aaabbc
std. výstup
    6 *97:3 3 *99:1 *98:2
Testovací sadu vstupních a vzorových výstupních dat naleznete zde.

Poslední změna: 1.11.2010 (18:43)
```


Zadani Huffman II:
```
Upozornění: Tato úloha navazuje na úlohu Huffman I. Vřele doporučujeme se nejprve seznámit s jejím zadáním a vyzkoušet si ji vyřešit. Formát vstupu a pravidla pro stavbu stromu zůstávají nadále stejná. Pokud jste s nimi důkladně seznámeni, můžete je přeskočit.

Formát vstupu a stavba stromu (stejné jako u Huffman I)
Napište program, který dostane jako argument jméno vstupního souboru. Přečte všechna data ze vstupního souboru, na jejich základě postaví Huffmanův strom a s jeho pomocí provede kompresi vstupních dat. Výsledek je vypsán v binárním formátu do výstupního souboru, jehož jméno dostaneme tak, že k názvu vstupního souboru přidáme příponu ".huff". Pokud nedostane právě jeden argument, vypište na std. výstup řetězec "Argument Error". Pokud vstupní nebo výstupní soubor nejde z jakéhokoli důvodu otevřít nebo z něj nelze číst (resp. do něj zapisovat), vypište na std. výstup řetězec "File Error". Vstupní soubor může mít libovolný formát (tedy může být klidně binární). Pro upřesnění: Znakem od této chvíle dál myslíme nějaký symbol reprezentující 1 byte (dívejte se tedy na data spíše jako na posloupnost bytů než na text).

Huffmanův strom je binární strom, v jehož listech se nachází všechny znaky, které se vyskytly ve vstupním souboru alespoň jednou. Váhou znaku pak rozumíme počet výskytů daného znaku ve vstupním souboru. Například vstupní data "xxyz" obsahují znaky "x", "y" a "z", přičemž "x" má váhu 2 a "y" i "z" mají váhu 1. Vnitřní uzly stromu neobsahují žádné znaky a jejich váha je rovna součtu vah obou jejich synů (každý vnitřní uzel má vždy oba syny).

Při stavbě stromu postupujeme následovně. Nejprve si připravíme všechny listy na základě analýzy vstupních dat a připravíme si z nich les jednoprvkových stromů. Dokud je v lese více než jeden strom, vezmeme vždy dva nejlehčí stromy z lesa, spojíme je pod nově vytvořený kořen (jehož váhu spočítáme jako součet vah listů) a vložíme jej zpět do lesa. Při spojování je uzel s menší váhou vždy vlevo. Pokud při výběru uzlů má více uzlů stejnou váhu, aplikují se následující pravidla (v pořadí, ve kterém jsou napsána). Tato pravidla také rozhodují, který uzel by měl být připojen vlevo a který vpravo, pokud mají oba stejnou váhu.

listy jsou lehčí než vnitřní uzly
mezi listy mají přednost ty, které mají menší hodnotu v nich uloženého znaku
mezi vnitřními uzly mají přednost ty, které vznikly dříve v průběhu algoritmu
Formát výstupu (nové pro Huffman II)
Výstupní soubor má následující formát: hlavička, kódovací strom a zakódovaná data. Hlavička obsahuje 8 bytů s následujícími hodnotami:

0x7B 0x68 0x75 0x7C 0x6D 0x7D 0x66 0x66
Strom je zakódován v prefixové notaci, kde každý uzel je kódován jako 64-bitové číslo zakódované ve formátu Little Endian (pořadí bytů použité např. na platformě IA-32/x86). Každý uzel tedy zabírá právě 8B. Popis stromu je zakončen speciální sekvencí obsahující 8 nulových bytů (tedy 64-bitová 0), která nekóduje žádný uzel (slouží pouze jako ukončovací zarážka).

Vnitřní uzly mají následující formát:

bit 0:  obsahuje hodnotu 0, která indikuje, že jde o vnitřní uzel
bity 1-55:  obsahují spodních 55 bitů váhy daného uzlu
bity 56-63: jsou nastaveny na 0
A listy jsou formátovány takto:

bit 0:  obsahuje hodnotu 1, která indikuje, že jde o list
bity 1-55:  obsahují spodních 55 bitů váhy daného uzlu
bity 56-63: 8-bitová hodnota znaku uloženého v daném listu
Kódování probíhá následovně. Každý znak ze vstupního souboru je zakódován do sekvence bitů, která odpovídá cestě z kořene Huffmanova stromu do listu obsahující tentýž znak. Hrany směřující k levým synům jsou označeny symbolem 0 a hrany směřující vpravo symbolem 1. Data se kódují jako bitový proud, neboť různé symboly mohou být kódovány různě dlouhými posloupnostmi bitů (klidně i sekvencí, která výrazně přesahuje délku 8). Sekvence pro jednotlivé vstupní znaky se do výstupního bitového proudu skládají hned za sebe (ve stejném pořadí, v jakém byly ve vstupním souboru). Vzhledem k tomu, že do souboru lze zapisovat pouze po bytech, musí být celý bitový proud doplněn nulovými bity na nejbližší možný násobek osmi. Při kódování postupujte tak, že 0. bit proudu bude uložen v 0. (t.j. nejnižším) bitu prvního byte, atd. až do 7. bitu. Dále 8. bit proudu bude v 0. bitu druhého byte, 16. bit bude v 0. bitu třetího byte, atd.

Například sekvence 1101 0010 0001 1010 111 (mezery v sekvenci slouží pouze k vyšší přehlednosti) bude kódována do tří bytů: 0x4B 0x58 0x07.

Důležitá poznámka: Váš program by měl být bez problémů schopen pracovat minimálně s 1 TB (přesněji tedy 1 TiB) vstupními soubory. K dispozici máte ovšem pouze O(1) paměti.


Příklady
Předpokládejme, že máme vytvořený Huffmanův strom, který pro znaky A, B, C, D generuje následující bitové sekvence (z důvodu přehlednosti příkladu tvar stromu neodpovídá skutečným četnostem znaků v následujících příkladech):

A: 0
B: 11
C: 100
D: 101

Pak bude vstupní sekvence znaků (vstupní soubor s následujícím obsahem): BDAACB
převedena do sekvence bitů (bitového proudu): 1110 1001 0011
která bude zakódována do sekvence bytů: 0x97 0x0C
tj. velikost vstupního souboru bude 6 bytů, velikost zakódvaných dat ve výstupním souboru bude 2 byty.

Pozor: následující odlišná sekvence znaků: BDAACBAA
bude sice převedena do jiné sekvence bitů: 1110 1001 0011 00
ale kvůli doplnění nulovými bity bude výsledná sekvence bytů stejná jako předchozím příkladě: 0x97 0x0C

Testovací data
Testovací sadu vstupních a vzorových výstupních dat naleznete zde.
```


Zadani Huffman III:
```
Upozornění: Tato úloha navazuje na úlohy Huffman I a Huffman II. Vřele doporučujeme se nejprve seznámit s jejich zadáním a vyzkoušet si je vyřešit. Formát kódovaného souboru a pravidla pro stavbu stromu zůstávají nadále stejná. Pokud jste s nimi důkladně seznámeni, můžete je přeskočit.

Napište program, který dostane jako argument jméno komprimovaného souboru (s příponou ".huff". Tento soubor je kódován způsobem popsaným v úloze Huffman II (viz též níže). Vašim úkolem je načíst z tohoto souboru Huffmanův strom a na jeho základě provést dekompresi dat, která za ním následují. Dekomprimovaná data uložte do výstupního souboru, jehož jméno získáte odtržením přípony ".huff" od názvu vstupního souboru. Pokud nedostane právě jeden argument nebo tento argument není platný (nemá správnou příponu) vypište na std. výstup řetězec "Argument Error". Pokud vstupní nebo výstupní soubor nejde z jakéhokoli důvodu otevřít, nelze číst (resp. do něj zapisovat) nebo vstupní soubor nemá správný formát, vypište na std. výstup řetězec "File Error". Pokud dojde k detekci chyby formátu v průběhu čtení vstupního souboru, je povoleno ponechat rozpracovaný výstupní soubor na disku.

Formát vstupu (stejný jako formát výstupu u úlohy Huffman II)
Vstupní soubor má následující formát: hlavička, kódovací strom a zakódovaná data. Hlavička obsahuje 8 bytů s následujícími hodnotami:

0x7B 0x68 0x75 0x7C 0x6D 0x7D 0x66 0x66
Strom je zakódován v prefixové notaci, kde každý uzel je kódován jako 64-bitové číslo zakódované ve formátu Little Endian (pořadí bitů použité např. na platformě IA-32/x86). Každý uzel tedy zabírá právě 8B. Popis stromu je zakončen speciální sekvencí obsahující 8 nulových bytů (tedy 64-bitová 0), která nekóduje žádný uzel (slouží pouze jako ukončovací zarážka).

Vnitřní uzly mají následující formát:

bit 0:  obsahuje hodnotu 0, která indikuje, že jde o vnitřní uzel
bity 1-55:  obsahují spodních 55 bitů váhy daného uzlu
bity 56-63: jsou nastaveny na 0
A listy jsou formátovány takto:

bit 0:  obsahuje hodnotu 1, která indikuje, že jde o list
bity 1-55:  obsahují spodních 55 bitů váhy daného uzlu
bity 56-63: 8-bitová hodnota znaku uloženého v daném listu
Data jsou kódována následovně. Každý znak je zakódován do sekvence bitů, která odpovídá cestě z kořene Huffmanova stromu do listu obsahující tentýž znak. Hrany směřující k levým synům jsou označeny symbolem 0 a hrany směřující vpravo symbolem 1. Data jsou kódována jako bitový proud, neboť různé symboly mohou být kódovány různě dlouhými posloupnostmi bitů (klidně i sekvencí, která výrazně přesahuje délku 8). Sekvence pro jednotlivé vstupní znaky jsou v bitovém proudu poskládány hned za sebe. Vzhledem k tomu, že do souboru lze zapisovat pouze po bytech, je celý bitový proud doplněn nulovými bity na nejbližší možný násobek osmi. Mapování bitů na byty je tvořeno tak, že 0. bit proudu je uložen v 0. (t.j. nejnižším) bitu prvního byte, atd. až do 7. bitu. Dále 8. bit proudu je v 0. bitu druhého byte, 16. bit je v 0. bitu třetího byte, atd.

Například byty 0x4B 0x58 0x07 kódují sekvenci 1101 0010 0001 1010 1110 0000 (mezery slouží pouze k vyšší přehlednosti). Posledních pět nul nemusí být součástí bitového proudu, ale může se jednat pouze o vycpávku (záleží na tom, kolik znaků je potřeba dekódovat).

Důležitá poznámka: Váš program by měl být bez problémů schopen pracovat minimálně s 1 TB (přesněji tedy 1 TiB) soubory. K dispozici máte ovšem pouze O(1) paměti. Jinými slovy vstup ani výstup se nevejdou do paměti.


Příklady
Předpokládejme, že ze vstupního souboru načteme Huffmanův strom, který pro znaky A, B, C, D generuje následující bitové sekvence. V závorkách jsou uvedeny frekvence výskytů znaků.

A (2): 0
B (2): 11
C (1): 100
D (1): 101

Dále mějme zakódovanou sekvenci 0x97 0x0C, která reprezentuje bitový proud 1110 1001 0011 0000. Dekódováním dostaneme posloupnost znaků BDAACB.

V dalším příkladu použijeme stejný strom, jen upravíme frekvence výskytů znaků A (4), B (2), C (1), D (1). Všimněte si, že kódy jednotlivých znaků zůstanou v tomto případě nezměněny. Pokud nyní dekódujeme stejnou sekvenci 0x97 0x0C, dostaneme výsledek BDAACBAA.

Testovací data
Testovací sadu vstupních a vzorových výstupních dat naleznete zde.
```