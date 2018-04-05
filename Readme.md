# Vodamep

Vorarlberger Datenmeldung in der Pflege.


## Build

powershell
```
.\build.ps1
```


native single-file executable :)
```
$env:vodamepnative = $true
dotnet publish -r win-x64 -c release

```

## Fragen


- Daten.xlsx: Ist Staatsangehörigkeit oder Land (Country) gemeint?
- Thema Wochenendbetreuung: Als eigene Liste? Sind alle Stammdaten zu melden?

## Todo

- Ort des Vereines / Adresse der Person.

