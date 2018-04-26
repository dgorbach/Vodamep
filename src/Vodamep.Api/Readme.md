# Vodamep - Server

(dms) Daten-Meldungs-Server

## Konfiguration

Die Konfiguration erfolgt durch Anpassung von [appsettings.json](./appsettings.json) oder durch setzten von Umgebungsvariablen (z.B. SqlServerEngine__ConnectionString=Server=localhost;Database=xxx;User Id=yyy;Password=zzz; 

Für die [IIS-Integration](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?view=aspnetcore-2.1&tabs=aspnetcore2x#configuration-of-iis-with-webconfig) befindet sich eine passende web.confg im Release.


```
dms.exe
```


```
Invoke-WebRequest -Uri http://localhost:5000/2018/2 -Method Put -InFile .\test.zip -Headers @{ Authorization = "Basic dGVzdDp0ZXN0"}

```

```
curl --request PUT --data-binary "@test.zip" --user test:test http://localhost:5000/2018/2
```



