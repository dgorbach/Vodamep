#language: de-DE
Funktionalität: Validierung der gemeldeten Personen einer Datenmeldung

Szenario: Es wurde ein ungültiger Ort angegeben.
    Angenommen die Eigenschaft 'postcode' von 'Person' ist auf '6900' gesetzt
    Und die Eigenschaft 'city' von 'Person' ist auf 'Dornbirn' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: ''6900 Dornbirn' ist kein gültiger Ort.'

Szenario: Das Geburtsdatum darf nicht in der Zukunft liegen.
    Angenommen die Eigenschaft 'birthday' von 'Person' ist auf '2058-04-30' gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''Geburtsdatum' darf nicht in der Zukunft liegen.'

Szenario: Das Geburtsdatum darf nicht vor 1900 liegen.
    Angenommen die Eigenschaft 'birthday' von 'Person' ist auf '1899-12-31' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Der Wert von 'Geburtsdatum' muss grösser oder gleich .*'

Szenario: Geburtsdatum und Datum aus der Sozialversicherungsnummer sollten übereinstimmen.
    Angenommen die Eigenschaft 'birthday' von 'Person' ist auf '1966-01-03' gesetzt
        Und die Eigenschaft 'ssn' von 'Person' ist auf '9778010366' gesetzt
    Dann enthält das Validierungsergebnis die Warnung 'Das Geburtsdatum 03.01.1966 unterscheidet sich vom Wert in der Versicherungsnummer 01.03.66.'
        Und es enthält keine Fehler

Szenario: Die Versicherungsnummer ist nicht korrekt.
    Angenommen die Eigenschaft 'ssn' von 'Person' ist auf '9999231054' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Die Versicherungsnummer 9999231054 ist nicht korrekt.'

Szenario: Versicherungsnummer ist nicht eindeutig.
    Angenommen eine Versicherungsnummer ist nicht eindeutig
    Dann enthält das Validierungsergebnis den Fehler 'Mehrere Personen haben die selbe Versicherungsnummer'

Szenario: PersonId ist nicht eindeutig.
    Angenommen der Id einer Person ist nicht eindeutig
    Dann enthält das Validierungsergebnis den Fehler 'Der Id ist nicht eindeutig.'

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
    Angenommen die Eigenschaft '<Name>' von 'Person' ist nicht gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: ''<Bezeichnung>' darf nicht leer sein.'
Beispiele:
    | Name        | Bezeichnung           |
    | ssn         | Versicherungsnummer   |
    | birthday    | Geburtsdatum          |
    | family_name | Familienname          |
    | given_name  | Vorname               |
    | religion    | Religion              |
    | insurance   | Versicherung          |
    | nationality | Staatsangehörigkeit   |
    | postcode    | Plz                   |
    | city        | Ort                   |
    | gender      | Geschlecht            |    

Szenariogrundriss: Eine Eigenschaft vom HkpvReport mit einem ungültigen Wert gesetzt.
    Angenommen die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: 'Für '<Bezeichnung>' ist '<Wert>' kein gültiger Code.'
Beispiele: 
    | Name        | Bezeichnung         | Wert |
    | religion    | Religion            | test |
    | insurance   | Versicherung        | test |
    | nationality | Staatsangehörigkeit | test |

Szenariogrundriss: Der Name einer Person enthält ein ungültiges Zeichen
    Angenommen die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: ''<Bezeichnung>' weist ein ungültiges Format auf.'
Beispiele: 
    | Name        | Bezeichnung   | Wert |
    | family_name | Familienname  | t@st |
    | given_name  | Vorname       | t@st |
