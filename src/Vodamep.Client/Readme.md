# Vodamep - Client

(dmc) Daten-Meldungs-Client:

```
dmc.exe

Usage -  <action>

Actions

  Send <File> -options - Absenden der Meldung.

    Option          Description
    File* (-F)
    Address* (-A)
    User (-U)
    Password (-P)

  Validate <File>  - Prüfung der Meldung.

    Option       Description
    File* (-F)

  PackFile <File> -options - Meldung neu verpacken.

    Option               Description
    File* (-F)
    Json (-J)            Save as JSON.
    NoCompression (-N)   [Default='False']

  PackRandom -options - Meldung mit Testdaten erzeugen.

    Option               Description
    Month (-M)
    Year (-Y)
    Persons (-P)         [Default='100']
    Staffs (-S)          [Default='5']
    AddActivities (-A)   [Default='True']
    Json (-J)            Save as JSON.
    NoCompression (-N)   [Default='False']

  List <Source>  - Listet erlaubte Werte.

    Option         Description
    Source* (-S)
                   Insurances
                   Religions
                   CountryCodes
                   Postcode_City

```