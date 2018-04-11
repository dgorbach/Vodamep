# Vodamep

Vorarlberger Datenmeldung in der Pflege.


### Datenmodel 

[Definition im protobuf-Format](./proto/Hkpv/Hkpv.proto)

#### Beispiel
```
{
    "institution": {
        "id": "kpv_test",
        "name": "Testverein"
    },
    "from": "2018-03-01",
    "to": "2018-03-31",
    "staffs": [{
        "id": "2",
        "familyName": "Ilgenfritz",
        "givenName": "Lucie"
    }],
    "persons": [{
        "id": "1",
        "religion": "VAR",
        "insurance": "19",
        "nationality": "AT",
        "careAllowance": "L4",
        "postcode": "6850",
        "city": "Dornbirn",
        "gender": "female"
    }],
    "personalData": [{
        "id": "1",
        "familyName": "Radl",
        "givenName": "Elena",
        "street": "Fußenegg 21",
        "ssn": "4221-30.07.50",
        "birthday": "1950-07-30"
    }],
    "activities": [{
        "date": "2018-03-01",
        "personId": "1",
        "staffId": "2",
        "type": "LV02",
        "amount": 1
    }, {
        "date": "2018-03-01",
        "personId": "1",
        "staffId": "2",
        "type": "LV15",
        "amount": 1
    }]
}
```

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

- Thema Wochenendbetreuung: Als eigene Liste? Sind alle Stammdaten zu melden?



## Todo

- Ort des Vereines / Adresse der Person überprüfen?

