# Vodamep

Vorarlberger Datenmeldung in der Pflege.

Inhalte:
- [Spezifikation](./specifications/Readme.md)
- [Meldungsclient](./src/Vodamep.Client/Readme.md)
- [Meldungsserver](./src/Vodamep.Api/Readme.md)
- [Meldungsbibliothek](./src/Vodamep/Readme.md) zur Einbindung in .net-Projekte.


Das aktuelle Release ist im [releases-Verzeichnis](.//releases) zu finden.


## Build

Für ein Build ist die Installation von [.Net Core SDK](https://www.microsoft.com/net/download/windows) erforderlich.
```
.\build.ps1
```

Für einen Build mit [native Compilation](https://github.com/dotnet/corert/blob/master/Documentation/intro-to-corert.md) sind die Visual Studio 2017 "Desktopentwicklung mit C++" erforderlich.
```
.\build.ps1 -Target Appveyor
```