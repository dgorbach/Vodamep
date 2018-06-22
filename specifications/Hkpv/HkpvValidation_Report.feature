#language: de-DE
Funktionalität: Validierung der Datenmeldung

Szenario: Korrekt befüllt
    Angenommen eine Meldung ist korrekt befüllt
    Dann enthält das Validierungsergebnis keine Fehler
    Und es enthält keine Warnungen
    
Szenario: Von-Datum muss der erste Tag des Monats sein.
    Angenommen die Eigenschaft 'from' von 'HkpvReport' ist auf '2018-04-04' gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''Von' muss der erste Tag des Monats sein.'

Szenario: Bis-Datum muss der letzte Tag des Monats sein.
    Angenommen die Eigenschaft 'to' von 'HkpvReport' ist auf '2018-04-04' gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''Bis' muss der letzte Tag des Monats sein.'

Szenario: Die Meldung muss genau einen Monat beinhalten.
    Angenommen die Eigenschaft 'from' von 'HkpvReport' ist auf '2018-03-01' gesetzt
        Und die Eigenschaft 'to' von 'HkpvReport' ist auf '2018-04-30' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Die Meldung muss genau einen Monat beinhalten.'

Szenario: Die Meldung darf nicht die Zukunft betreffen.
    Angenommen die Eigenschaft 'to' von 'HkpvReport' ist auf '2058-04-30' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Der Wert von 'Bis' muss kleiner oder gleich .*'

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
    Angenommen die Eigenschaft '<Name>' von 'HkpvReport' ist nicht gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: ''<Bezeichnung>' darf nicht leer sein.'
Beispiele:
    | Name        | Bezeichnung |
    | from        | Von         |
    | to          | Bis         |
    | institution | Einrichtung |

Szenariogrundriss: Die Datumsfelder dürfen keine Zeit enthalten
    Angenommen die Datums-Eigenschaft '<Name>' von 'HkpvReport' hat eine Uhrzeit gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' darf keine Uhrzeit beinhalten.'
Beispiele:
    | Name     | Bezeichnung |
    | from     | Von         |
    | to       | Bis         |
    