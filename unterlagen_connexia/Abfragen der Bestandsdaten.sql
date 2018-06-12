
select distinct
  L.Verein, 
  A.A_Name, 
  A.Name_1 
from Zeit Z, 
     HKP_Betreuung_Leistungen L inner join TB_HKP_Verein V on V.Verein = L.Verein 
                                inner join Adressen A on A.Adressnummer = V.Adressnummer 
where L.Datum between Z.dVon and Z.dBis 
order by 
  L.Verein



select
--  A.A_Name,
  L.Verein ID_Verein,
  L.Pfleger ID_PflegerIn,
  L.Adressnummer ID_Klient,
  L.Leistung,
  L.Anzahl,
  L.Datum 
from Zeit Z,
     HKP_Betreuung_Leistungen L inner join Adressen A on A.Adressnummer = L.Adressnummer 
where L.Datum between Z.dVon and Z.dBis 
order by 
  L.Verein,
  L.Adressnummer,
  L.Datum 



select
  U.Verein cID_Verein,
  U.von dVon,
  U.bis dBis,
  V.Wert VZAE,  
  U.Wert ID_PflegerIn,
  U.Adressnummer ID_Klient,
  V.Funktion BerufsTitel, 
  A.A_Name Name_PflegerIn  

from Zeit Z,
     TB_HKP_Verein_Funktion U inner join TB_HKP_Verein_Funktion V 
                                         on V.Adressnummer=U.Adressnummer 
                                         and V.von=U.von 
                                         and coalesce(V.bis,V.von) = coalesce(U.bis,V.von)
                              inner join Adressen A on A.Adressnummer = U.Adressnummer 
where U.Funktion = 'PFL' 
  and not V.Funktion = 'PFL' 
  and U.von <= Z.dBis 
  and coalesce(U.bis,Z.dBis) >= Z.dBis 
order by 
  U.Verein,
  A.A_Name,
  U.von 



select distinct 
  L.Verein ID_Verein,
  L.Adressnummer ID_Klient,
  D.Detail, 
  T.Doku_Text PflegeGeld 

from Zeit Z,
     HKP_Betreuung_Leistungen L inner join Doku D on D.Adressnummer = L.Adressnummer and D.Heim = L.Verein 
                                inner join Doku_Tabelle T on T.Projekt='HKP' and T.Gruppe='21' and T.Detail = D.Detail 
where L.Datum between Z.dVon and Z.dBis 
  and D.Gruppe = '21'
order by 
  L.Verein,
  L.Adressnummer 




select distinct 
  L.Verein ID_Verein, 
  A.Adressnummer ID_Klient,
  A.A_Name Name_Klient,
  A.Geschlecht,
  A.Geburtsdatum,
  A.Staatsbuergerschaft,
  A.Postleitzahl,
  O.Ort, 
  A.Versicherungsnummer SVNR,
  A.Versicherung,
  V.Versicherung_Text 
from Zeit Z, 
     Adressen A inner join HKP_Betreuung_Leistungen L on L.Adressnummer=A.Adressnummer 
                left join TB_Versicherung V on V.Versicherung=A.Versicherung 
                left join TB_Orte O on O.Postleitzahl = A.Postleitzahl 
where L.Datum between Z.dVon and Z.dBis 
   and L.Leistung not in ('31','32','33','34') 
order by 
  L.Verein, 
  A.A_Name 



select distinct 
  L.Verein ID_Verein, 
  A.Adressnummer ID_Klient,
  A.A_Name Name_Klient,
  A.Geschlecht,
  A.Geburtsdatum,
  A.Staatsbuergerschaft,
  A.Postleitzahl,
  O.Ort, 
  A.Versicherungsnummer SVNR,
  A.Versicherung,
  V.Versicherung_Text 
from Zeit Z, 
     Adressen A inner join HKP_Betreuung_Leistungen L on L.Adressnummer=A.Adressnummer 
                left join TB_Versicherung V on V.Versicherung=A.Versicherung 
                left join TB_Orte O on O.Postleitzahl = A.Postleitzahl 
where L.Datum between Z.dVon and Z.dBis 
   and L.Leistung in ('31','32','33','34') 
order by 
  L.Verein, 
  A.A_Name 


select
--  A.A_Name,
  L.Verein ID_Verein,
  L.Pfleger ID_PflegerIn,
  L.Adressnummer ID_Klient,
  L.Leistung,
  L.Anzahl,
  L.Datum 
from Zeit Z,
     HKP_Betreuung_Leistungen L inner join Adressen A on A.Adressnummer = L.Adressnummer 
where L.Datum between Z.dVon and Z.dBis 
order by 
  L.Verein,
  L.Adressnummer,
  L.Datum 



Land	Postleitzahl	Ort
A	670053	Lorüns
A	670084	Stallehr
A	675140	Innerbraz
A	682221	Düns
A	682222	Dünserberg
A	682267	Röns
A	682275	Schnifis
A	683048	Laterns
A	683268	Röthis
A	683285	Sulz
A	683328	Fraxern
A	685807	Bildstein
A	687006	Reuthe
A	690060	Möggers
A	691124	Eichenberg
A	695281	Sibratsgfäll
A	696014	Buch