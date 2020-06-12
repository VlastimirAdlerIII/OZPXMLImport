# OZPXMLImport
Ukázkový projekt

Vytvoření DB:  
`sqlcmd -S [název SQL server/instance] -i SQLScript.sql`
(za předpokladu, že stojím v adresáři, kde se nachází script)

Import dat z excelu:  
`OZPXMLImport import Data.xlsx`

Import dat z xml:  
`OZPXMLImport import XMLSerializerOutput.xml`

Výpis aktuálních smluv:  
`OZPXMLImport list [datum ve formátu rrrr-mm-dd]`

Projekt není ve finální (produkční) kvalitě, ale nechtěl jsem jeho předání dále odsouvat.  
Chybí především:  
- transakce
- unit testy
- ošetření vstupů
- ošetření výjimek
- logování činnosti do souboru/na konzoli

Kontrola překrývání intervalů by potřebovala upřesnit zadání, vyrobil jsem ji podle vlastního uvážení.  
Nevhodnou tvorbou vstupních dat se člověk může dostat do potíží, i tady by bylo potřeba vyjasnit si zadání.  
Do repository jsem uložil úplně všechno - i to, co sem nepatří :-)
