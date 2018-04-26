# Schnittstellenbeschreibung

## Datenmodel 

[Für die Hauskrankenpflege](./Hkpv/Hkpv.proto) (im protobuf-Format)

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
## Web-Api 

[Definition](./WebApi/swagger.yaml) (im Swagger-Format)

[Aufruf](./WebApi/Vodamep.postman_collection.json) (als Postman-Collection)