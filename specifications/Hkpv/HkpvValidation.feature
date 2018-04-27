#language: de-DE
Funktionalität: Validation

Szenario: Korrekt befüllt
	Angenommen eine Meldung ist korrekt befüllt
	Dann enthält das Validierungsergebnis keine Fehler
	Und es enthält keine Warnungen

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
	Angenommen die Eigenschaft '<name>' ist nicht gesetzt
	Dann enthält das Validierungsergebnis genau einen Fehler
	Und die Fehlermeldung lautet: ''<bezeichnung>' darf nicht leer sein.'
Beispiele: 
	| name | bezeichnung |
	| From | Von         |
	| To   | Bis         |
	| Institution   | Einrichtung         |
	



