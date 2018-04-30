#language: de-DE
Funktionalität: Validation

Szenario: Korrekt befüllt
	Angenommen eine Meldung ist korrekt befüllt
	Dann enthält das Validierungsergebnis keine Fehler
	Und es enthält keine Warnungen

Szenariogrundriss: Eine Eigenschaft vom HkpvReport ist nicht gesetzt
	Angenommen die Eigenschaft '<Name>' von '<Art>' ist nicht gesetzt
	Dann enthält das Validierungsergebnis genau einen Fehler
	Und die Fehlermeldung lautet: ''<Bezeichnung>' darf nicht leer sein.'
Beispiele: 
	| Name        | Bezeichnung         | Art           |
	| from        | Von                 | HkpvReport   |
	| to          | Bis                 | HkpvReport   |
	| institution | Einrichtung         | HkpvReport   |
	| ssn         | Versicherungsnummer | PersonalData |
	| birthday    | Geburtsdatum        | PersonalData |
	| family_name | Familienname        | PersonalData |
	| given_name  | Vorname             | PersonalData |
	| street      | Anschrift           | PersonalData |
	| religion    | Religion            | Person       |
	| insurance   | Versicherung        | Person       |
	| nationality | Staatsangehörigkeit | Person       |
	| postcode    | Plz                 | Person       |
	| city        | Ort                 | Person       |
	| gender      | Geschlecht          | Person       |
	| family_name | Familienname        | Staff        |
	| given_name  | Vorname             | Staff        |
	

	

	
	

