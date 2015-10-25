CZECH VERSION only

Zadani:

```
Napište aplikaci, která dostane vstupní soubor se zjednodušenou excelovskou tabulkou, vyhodnotí všechny vzorce a výsledek uloží opět ve formě tabulky do výstupního souboru. Názvy vstupního a výstupního souboru dostane váš program formou argumentů příkazového řádku. V případě špatného počtu argumentů musí program vypsat na standardní výstup řetězec "Argument Error". Pokud některý ze souborů nejde z jakéhokoli důvodu otevřít, váš program vypíše "File Error".

Tabulka je v souboru uložena následovně. Jeden řádek tabulky je uložen na jednom řádku souboru. Hodnoty v jednotlivých buňkách jsou odděleny jednou nebo více mezerami, každá hodnota je pak zapsána bez mezer. Počet řádků ani sloupců není dopředu znám a každý řádek může obsahovat jiný počet obsazených buněk. Řádky logicky číslujeme od 1 do nekonečna (máte zajištěno, že číslo řádku se vejde do 32 bit. integeru), sloupce značíme velkými písmeny stejně jako v Excelu (A, B, ... Z, AA, AB, ... AZ, BA, BB, ... ZZ, AAA, ...).

Obsahem jednotlivých buněk může být:

Prázdné hranaté závorky [] symbolizují prázdnou buňku. Takovéto explicitně označené prázdné buňky se ve výstupu se musí objevit opět jako prázdné hranaté závorky (jiné prázdné buňky se do výstupu nevypisují).
Celočíselná nezáporná hodnota, která se vejde do 32 bit znaménkového integeru (jako výsledek výpočtů a tedy i ve výstupním souboru se může objevit hodnota záporná).
Vzorec začínající znakem = (viz dále).
Všechny buňky, které nejsou definovány (tj. za poslední buňkou na řádku a za posledním řádkem) chápeme jako také prázdné. Pro účely výpočtů mají všechny prázdné buňky hodnotu 0. Do výstupního souboru přijde kopie celé vstupní tabulky (bez jakýchkoli strukturálních změn, buňky jsou ve výstupním souboru oddělené jednou mezerou), pouze vzorce budou nahrazeny výsledky jejich výpočtů. Pokud nějaká buňka neobsahuje žádný platný vstup (např. obsahuje řetězec), bude její obsah nahrazen řetězcem #INVVAL.

Vzorce začínají znakem =, po kterém následuje infixový zápis binární operace +, -, * nebo / jejíž operandy jsou dvě jiné buňky. Referenci na buňku píšeme ve tvaru SLOUPECřádek. Příklad jednoduchého vzorce je tedy =A1+BC2. Oba operandy musí být reference (není možné např. sčítat buňku s číslem). Všechny operace provádějte celočíselně a dejte pozor na to, že buňka použitá v operandu jednoho vzorce může také obsahovat vzorec (tj. některé buňky je potřeba spočítat dřív než jiné).

Pokud nastane chyba ve výpočtu, program nesmí spadnout, ale na chybu musí vhodně upozornit tak, že jako výsledek chybné operace vloží do příslušné buňky jeden z následujících řetězců:

#ERROR — výpočet nelze provést; nejméně jeden operand nemá platnou hodnotu nebo nejde spočítat
#DIV0 — při výpočtu došlo k dělení nulou
#CYCLE — nalezen cyklus (všechny buňky na cyklu musí mít tuto hodnotu). Výsledkem vzorců v buňkách, které nejsou součástí cyklu, ale pouze se na cyklus odkazují bude hodnota #ERROR (pokud je vzorec v takové buňce jinak platný). Můžete počítat s tím, že vstupní tabulka neobsahuje vnořené nebo jinak propletené cykly.
#MISSOP — v zápisu vzorce chybí operátor
#FORMULA — nejméně jeden z operandů je chybně zapsán nebo je ve vzorci jiná chyba než #MISSOP
Při psaní aplikace očekávejte, že máte k dispozici O(N) paměti, kde N je délka vstupního souboru. Vaše řešení by také mělo být rozumě rychlé, tzn. rozhodně byste neměli vyhodnocovat každý vzoreček víc než jednou.

Rozšíření:
Volitelně můžete naprogramovat následující rozšíření. Implementace řešení včetně rozšíření bude hodnocena 100% bodů, implementace bez rozšíření bude hodnocena 80% bodů. Při adresaci buněk ve vzorcích uvažujme také alternativní formát SESIT!SLOUPECradek, který odkazuje na buňku v jiném sešitě. Každý sešit je v separátním souboru, jehož název je identifikátor sešitu s příponou .sheet. Máme-li např. adresu List1!A2, budeme hledat první buňku na druhém řádku v souboru List1.sheet. Pokud takový soubor neexistuje, nelze operand vyhodnotit a výpočet nelze provést (výsledkem je #ERROR). Ve všech sešitech mohou být také vzorce, a to i s odkazy na jiné sešity včetně hlavního (pozor na cyklické závislosti). Externí sešity ale nemusíte vyhodnocovat celé — stačí spočítat pouze ty hodnoty, které potřebujete pro výpočet vzorců v hlavním sešitě.


Příklad:
$>program.exe sample.sheet sample.eval
sample.sheet
    [] 3 =B1*A2
    19 =C1+C2 42
    auto
    =B2/A1 =A1-B4 =C2+A4
    =chyba =A1+autobus
sample.eval
    [] 3 57
    19 99 42
    #INVVAL
    #DIV0 #CYCLE #ERROR
    #MISSOP #FORMULA


Mail 1:
Zdravim vsechny z mych cviceni,

mam pro vas k zadani ulohy Excel nekolik poznamek:
1) Uz mame za sebou dost uloh, na kterych jste videli spoustu problemu, na ktere je treba si davat pozor. Proto ma uz uloha Excel trochu pristejsi limity, a je tedy opravdu treba vyuzit vyhod objektove orientovaneho programovani, a vlastnosti C# jako jazyka, abyste vytvorili "konkurenceschopnou" aplikaci, jak ve spotrebe pameti, tak v rychlosti. Pri tvorbe vaseho reseni je zvlast dulezite si uvedomit, ze si sice v bunkach mate i "za behu" pamatovat informace o vstupnich vzorcich, ale ze je treba zvolit nejakou dostatecne uspornou reprezentaci (jen "normalne" uspornou, neocekavame od vas zadne "silenosti"), protoze tech bunek se vzorci muze byt v datech opravdu hodne.
Pro predstavu:
* CodExove limity pro testy, kde v obou testech jsou na vstupu listy s ramcove statisici bunek, ze kterych je zhruba 1/3 vzorcu:
(POZOR: ve spotrebe pameti je zahrnuta kompletni spotreba pameti beziciho procesu s resenim, tedy vcetne kodu, nahrane CLR mona [verze z CodExu], jeho datovych struktur, atd. - tj. cisla nemaji !zadny! absolutni vyznam, je treba je vnimat jen ve vztahu k vysledkum nize)
08: 1,6 sekundy + 65 MB pameti
09: 1,5 sekundy + 65 MB pameti

* moje reseni:
08: 0,848 s + 42 MB pameti
09: 0,8 s + 35 MB pameti

* jine rozumne reseni:
08: 0,992 s + 47 MB pameti
09: 0,884 s + 47 MB pameti

2) Jak si nekteri z vas vsimli, tak v rozsirene casti zadani neni uplne presne popsano, jak muze vypadat jmeno dalsich listu (pred !):

Otazka jednoho z vas:
> mohu předpokládat, že se v názvu listu nevyskytne znak operace?

Diky, to je velice dobra otazka: ano, muzete predpokladat, ze se znak operace ve validnich jmenech listu vyskytnout nemuze. Tedy napriklad bunku s
obsahem:

=Listpomlcka-2!B2+A11

muzete brat, ze je spatne (i kdyby soubor Listpomlcka-2.sheet existoval), a tedy vysledek muze byt treba #FORMULA ... Nicmene test by na to v CodExu zadny byt nemel.

3) Pro ty, co nebyli na poslednim cviceni: vsechna cviceni ze C# pristi tyden odpadaji kvuli DOD (Den otevrenych dveri), tedy dalsi ma cviceni budou az 3.12.2014. Uloha Huffman III (jelikoz uz na ni neni nic "noveho"
zasadniho na nauceni, ale je to "jen" opak Huffman II) je brana jen jako bonusova (pokud si chcete vyzkouset jeste jiny priklad na bitove operace, nebo se vam hodi 10 bodu navic), a tedy ma deadline az na konci semestru, a tedy se ji na dalsich cvicenich jiz nebudeme zabyvat. Na uloze Excel se naopak stale muzete naucit neco noveho, proto to je "ta" uloha, kterou byste meli resit - a jejimiz problemy a resenimi se budeme na dalsim cviceni zabyvat. Deadline na ulohu Excel je 14 dni, tedy do dalsiho cviceni.

Mejte se fajn,


Mail 2:
Zdravim vsechny z meho cviceni,

padla nasledujici otazka:
OTAZKA >>>
Vyhodnocují se nejdříve Errory či Cykly?

[] =A2+C1 =A1+D1 =A1+B1
=ERR
Je výsledkem pole B1 hodnota #ERROR či #CYCLE?

Za mně mi přijde na základě zadání logičtější, že má přednost #CYCLE, a to konkrétně díky formulaci:
"nalezen cyklus (všechny buňky na cyklu musí mít tuto hodnotu)", ale zdá se mi, že přesně specifikované to není.
<<< KONEC OTAZKY

Odpoved:
Ano, myslim, ze tato interpretace je nejlepsi, tedy #CYCLE zde ma prednost pred #ERROR. Viz vstupy a vystupy v priloze.

Mejte se fajn,

```