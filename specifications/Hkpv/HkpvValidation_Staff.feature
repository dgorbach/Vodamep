#language: de-DE
Funktionalität: Validierung der gemeldeten Mitarbeiterinnen Datenmeldung

Szenario: StaffId ist nicht eindeutig.
    Angenommen der Id einer Mitarbeiterin ist nicht eindeutig
    Dann enthält das Validierungsergebnis den Fehler 'Der Id ist nicht eindeutig.'

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
    Angenommen die Eigenschaft '<Name>' von '<Art>' ist nicht gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: ''<Bezeichnung>' darf nicht leer sein.'
Beispiele:
    | Name        | Bezeichnung         | Art          |    
    | family_name | Familienname        | Staff        |
    | given_name  | Vorname             | Staff        |

Szenariogrundriss: Der Name einer Person enthält ein ungültiges Zeichen
    Angenommen die Eigenschaft '<Name>' von '<Art>' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: ''<Bezeichnung>' weist ein ungültiges Format auf.'
Beispiele: 
    | Name        | Bezeichnung         | Art    | Wert |    
    | family_name | Familienname        | Staff  | t@st |
    | given_name  | Vorname             | Staff  | t@st |