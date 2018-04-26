# Vodamep - Server

(dms) Daten-Meldungs-Server

[IIS-Integration](./web.config)

[Konfiguration](./appsettings.json)


```
dms.exe
```


```
Invoke-WebRequest -Uri http://localhost:5000/2018/2 -Method Put -InFile .\test.zip -Headers @{ Authorization = "Basic dGVzdDp0ZXN0"}

```

```
curl --request PUT --data-binary "@test.zip" --user test:test http://localhost:5000/2018/2
```



