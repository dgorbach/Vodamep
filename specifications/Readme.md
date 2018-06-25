# Schnittstellenbeschreibung

## Datenmodel 

### Datenaustausch

- [Datenaustausch](./Hkpv/Hkpv.proto) (im protobuf-Format)

### Validierungen 
- [Aktivitäten](./Hkpv/HkpvValidation_Activity.feature) 
- [Personen](./Hkpv/HkpvValidation_Person.feature) 
- [Mitarbeiter](./Hkpv/HkpvValidation_Staff.feature) 
- [Meldung](./Hkpv/HkpvValidation_Report.feature) 

Alle Validierungen sind im Gherkin-Format beschrieben.

#### Werte
- [Religionen](./Datasets/religions.csv)
- [Versicherungen](./Datasets/insurances.csv)
- [Ländercodes](./Datasets/german-iso-3166.csv)
- [Orte](./Datasets/postcode_cities.csv)

#### Beispiel
```
{
    "institution": {
        "id": "kpv_test",
        "name": "Testverein"
    },
    "from": "2018-03-01T00:00:00Z",
    "to": "2018-03-31T00:00:00Z",
    "staffs": [{
        "id": "2",
        "familyName": "Ilgenfritz",
        "givenName": "Lucie"
    }],
    "persons": [{
        "id": "1",
        "familyName": "Radl",
        "givenName": "Elena",
        "street": "Fußenegg 21",
        "ssn": "4221300750",
        "birthday": "1950-07-30T00:00:00Z",
        "religion": "VAR",
        "insurance": "19",
        "nationality": "AT",
        "careAllowance": "L4",
        "postcode": "6850",
        "city": "Dornbirn",
        "gender": "female"
    }],    
    "activities": [{
        "date": "2018-03-01T00:00:00Z",
        "personId": "1",
        "staffId": "2",
        "entries": ["LV02", "LV05", "LV06", "LV15"]
    }]	
}
```
## Web-Api 

[Definition](./WebApi/swagger.yaml) (im Swagger-Format)

[Aufruf](./WebApi/Vodamep.postman_collection.json) (als Postman-Collection)