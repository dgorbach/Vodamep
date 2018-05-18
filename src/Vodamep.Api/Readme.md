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

## Basic-Authentication mit Proxy:
Die Authentifizierung wird an einen anderen Webservice weitergeleitet. 

### Beispiel:

Als Basic-Auth-Server wird dms im BasicAuthentication-Testmodus "UsernameEqualsPassword" gestartet.
```
SET BasicAuthentication__MODE=UsernameEqualsPassword 
SET ASPNETCORE_URLS=http://*:5001
dms.exe
---
    Vodamep.Api.Startup[0]                        
    Using UsernameEqualsPasswordCredentialVerifier
    ...
    Now listening on: http://[::]:5001
```

Eine weitere Instanz im Proxy-Modus verwendet nun den obigen Service.


Basic-Auth-Proxy:
```
SET BasicAuthentication__MODE=Proxy 
SET BasicAuthentication__PROXY=http://localhost:5001
dms.exe
---
    info: Vodamep.Api.Startup[0]
      Using ProxyAuthentication: http://localhost:5001
    ...    
    Now listening on: http://localhost:5000
```


```
curl http://localhost:5000 --head --user test:test
HTTP/1.1 200 OK
```

```
curl http://localhost:5000 --head
HTTP/1.1 401 Unauthorized
```