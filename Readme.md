# Vodamep

Vorarlberger Datenmeldung in der Pflege.


## Build

powershell
```
.\build.ps1
```


native single-file executable :)
```
cd src\Vodamep.Client

$env:vodamepnative = $true

dotnet publish -r win-x64 -c release

```

## Fragen


- Daten.xlsx: Ist Staatsangehörigkeit oder Land (Country) gemeint?
- Pflegegeld: ein feld oder zwei?

- Thema Wochenendbetreuung: Als eigene Liste? Sind alle Stammdaten zu melden?

Möglichst Person statt PersonalData (=Personenbezug):
- Braucht es das Geburtsdatum? wäre vielleicht nicht Alter genug?
- Ort eventuell nicht Personenbezogen.

- Stammdaten des Vereins

## Todo

- Ort des Vereines / Adresse der Person überprüfen?

