CZECH VERSION only

Zadani: 

```
Napište program, který dostane jako argumenty příkazové řádky tři parametry: vstupní soubor, výstupní soubor a maximální šířku textu. Všechen text ze vstupního souboru přeformátuje do výstupního tak, aby každý řádek měl právě maximální šířku textu, je-li to možné.

Pokud program obdrží špatné množství argumentů nebo třetí argument není platné číslo větší než 0, program vypíše na standardní výstup řetězec "Argument Error". V případě, že vstupní nebo výstupní soubor nejde otevřít (vstupní soubor neexistuje, špatné znaky v názvu, nedostatečná práva, apod.), vypíše program řetězec "File Error".

Při formátování je potřeba dodržet následující pravidla:

Zalomení řádku, tabulátor a mezera (tzn. '\n', '\t' a ' ') považujeme za bílé znaky, vše ostatní za znaky tisknutelné. Pro jednoduchost máte zaručeno, že znak '\r' v textu není.
Za slovo považujeme libovolnou sekvenci nebílých znaků, která je z obou stran oddělena alespoň jedním bílým znakem nebo začátkem/koncem souboru. Měnit smíte pouze bílé znaky, všechna slova musí zůstat přesně tak, jak jsou ve vstupním souboru.
Prázný řádek, řádek obsahující pouze bílé znaky a více takových řádků za sebou představuje oddělovač odstavců. Každý odstavec se pak formátuje zvlášť přičemž ve výstupu se odstavce oddělují jedním prázdným řádkem. Poslední řádek posledního odstavce v souboru musí být také zakončen (musí obsahovat zalomení řádku), avšak za ním již žádné prázdné řádky nesmí být.
Odstavec je sázen tak, že na každý řádek vysázíme největší možný počet slov (při zachování jejich pořadí) tak, aby nebyla překročena maximální šířka řádku. Mezi jednotlivými slovy musí být alespoň jedna mezera. Pokud na konci řádku zbývá nějaké volné místo (do maximální šířky), rozdělí se toto místo rovnoměrně mezi slova doplněním znaků mezer. V případě, že nejde mezery rozdělit zcela rovnoměrně, budou mezery přidělovány do proluk v pořadí zleva. Tzn. širší proluky jsou více vlevo než užší. Poslední řádek každého odstavce je zarovnaný vlevo, tj. mezi všemi slovy na tomto řádku je právě jedna mezera.
Ve výstupním souboru nesmí být na žádném řádku mezi posledním znakem posledního slova a znakem konce řádku nikdy žádné bílé znaky.
Pokud se v textu nachází slovo, které je delší než maximální počet znaků na řádku, bude toto slovo bez zalomení vysázeno na samostatném řádku. Nachází-li se na jednom řádku samotné slovo, je toto slovo zarovnáno vlevo.
Výše uvedená pravidla je třeba dodržet velmi důsledně, protože výstup vašeho programu bude porovnán se vzorovým výstupem znak po znaku. Rovněž neděletje žádné předpoklady o velikosti vstupu. Celý vstup a dokonce ani jedniný jeho řádek se nemusí vejít do paměti.


Příklad 1:
$>program.exe plain.txt format.txt 17
Vstupní soubor plain.txt
If a train station is where the train stops, what is a work station?
Výstupní soubor format.txt
If     a    train
station  is where
the  train stops,
what  is  a  work
station?
Příklad 2:
$>program.exe plain.txt format.txt abc
std. výstup
Argument Error
Příklad 3:
Až budete potřebovat nějaký delší text, na kterém byste mohli otestovat funčnost vaší implementace, tak můžete využít např. nějaký generátor tzv. Lorem ipsum textu (např. zde http://generator.lorem-ipsum.info/, nebo jakýkoliv jiný který vygooglujete). Pro ukázku si můžete stáhnout soubor LoremIpsum.txt (Unix konce řádků, tj. jen "\n") s předgenerovaným Lorem Ipsum, a k němu ukázkový výstup této úlohy při nastavení šířky na 40 znaků: LoremIpsum_Aligned.txt (Unix konce řádků). 

Pozor: pokud budete vaše řešení spouštět na Windows, tak ve vlastnosti Environment.NewLine je uložený dvojznak Windows konce řádků (tj. "\r\n"), tedy všechny výpisy metod TextWriter.WriteLine, apod. vám budou také vypisovat Windows konce řádků. Podobně výše uvedený generátor vrací textové soubory s Windows konci řádků. 

Hint: pokud byste potřebovali překódovat nějaký textový soubor z Windows do Unix konců řádků nebo obráceně, tak na to můžete využít např. editor Visual Studia. Pokud ve VS otevřete textový soubor (VS automaticky detektuje "formát" souboru, tj. typ konců řádků, pokud jsou použity konzistentně), tak pak vyberte v položku menu File - Save * as..., v dialogu klikněte na šipku v tlačítku Save, vyberte Save with Encoding..., a poté si v Line endings můžete vybrat cílové kódování konců řádků.
Poslední změna: 13.10.2013 (11:47)
```