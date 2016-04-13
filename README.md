# Nummervoorziening - C#.NET Client Reference Application

### Algemeen

De Nummervoorziening - C#.NET Client Reference Application is een client implementatie in C#.NET dat als voorbeeld dient voor de communicatie met de Nummervoorziening applicatie binnen de Educatieve Contentketen. De C#.NET Client Reference Application implementeert globaal de volgende functionaliteiten;
  - Globale communicatie met de Nummervoorziening applicatie (PingOperation t.b.v. het vaststellen van een correcte verbinding met en werking van de Nummervoorziening applicatie).
  - Genereren van een eerste niveau hash van een PGN middels het SCrypt hashing algoritme.
  - Het opvragen van een ECK-ID bij de Nummervoorziening applicatie op basis van een gehashte PGN, een keten-id en sector-id (RetrieveEckIdOperation).
  - Het opvragen van ondersteunde ketens bij de Nummervoorziening applicatie (RetrieveChainsOperation).
  - Het opvragen van ondersteunde sectoren bij de Nummervoorziening applicatie (RetrieveSectorsOperation).
  - Opvoeren van nieuwe gehashte PGNs die dienen te worden doorverwezen naar reeds bestaande gehashte PGNs bij het creëren van ECK-IDs (ReplaceEckIdOperation).
  - Het opvoeren van een batch met gehaste PGNs (SubmitEckIdBatchOperation).
  - Het ophalen van een batch met ECK-IDs (RetrieveEckIdBatchOperation).

Voor alle bovenstaande functionaliteiten wordt gebruik gemaakt van SOAP Messaging tussen de verschillende actoren. Authenticatie vindt plaats middels PKI-Certificaten die over TLS worden uitgewisseld. Aanvullende informatie over Nummervoorziening kan gevonden worden op de [Edukoppeling Wiki] van [Stichting Kennisnet].  

### Version
0.1-20160412

### Gebruikte Technologiën

Bij de ontwikkeling van Nummervoorziening - C#.NET Client Reference Application zijn diverse technologiën ingezet:
* .NET v4.5.2 - Microsoft's .NET Framework
* Visual Studio 2015 - Ontwikkelomgeving
    * svcutil.exe - Utility voor het creëren van WCF proxy service interface op basis van een WSDL
* Fiddler - TLS/SSL debugging
* Wireshark - TLS/SSL debugging
* SOAP UI - Functional Testing framework for SOAP and REST APIs

### Solution Modules
 * **ConsoleApplication** Voorbeeldapplicatie om de werking van de **SchoolID** module te demonstreren
 * **CryptSharp** 3rd party library met (onder andere) de functionaliteit om op basis van het hashingalgoritme SCrypt hashes te genereren.  
 * **SchoolID** Library met de basisfunctionaliteiten om de Nummervoorziening applicatie op een juiste wijze te kunnen bevragen. 
 * **UnitTestProject** Voorbeeldcode voor het gebruik van de Nummervoorziening applicatie.

### ConsoleApplication - Structuur
 * **App.config** (.NET applicatie configuratie)

 ### CryptSharp - Structuur
 * 
 
 ### SchoolID - Structuur
 * 
 
 ### UnitTestProject - Structuur
 * **App.config** (.NET applicatie configuratie)

### Ontwikkeling

##### WSDL naar Proxy Class
Voor de ontwikkeling van de applicatie is de WSDL als uitgangspunt genomen. De WSDL wordt aangeboden via een TLS/SSL beveiligde verbinding, waar de standaard .NET utilities niet (eenvoudig) bij kunnen. Het is daarom aan te raden de WSDL eerst te downloaden en lokaal op te slaan.

Voor het omzetten van een WSDL naar gegenereerde code zijn uit de .NET toolset grofweg twee utilities beschikbaar; wsdl.exe en svcutil.exe. De utility wsdl.exe zet een WSDL om in een webservice, waarbij de nieuwe class SoapHttpClientProtocol overerft. Het resultaat kan gebruikt worden als ASMX webservice. De utility svcutil.exe zet een WSDL om in een WCF proxy class (interface), die gebruikt kan worden door webservices door de interface te implementeren en de class SoapHttpClientProtocol te overerven. Omdat het nieuwere WCF door Microsoft is aangemerkt om ASMX services te vervangen, is voor de ontwikkeling van deze applicatie gekozen om de webservices middels WCF te implementeren. De gebruikte commando voor de utility is als volgt:
```
svcutil.exe /mc /syncOnly schoolid.wsdl
```
De svcutil.exe bleek niet in staat diverse WS-Addressing policies (zoals <wsaw:UsingAddressing wsdl:required="true" />) op een juiste manier te verwerken. Mits deze regel is toegevoegd aan de WSDL, toont de svcutil.exe een warning en genereerd deze geen output.config bestand. Bij het opnieuw genereren van de proxy class is het daarom aan te raden de verwijzingen naar WS-Addressing te verwijderen uit het WSDL bestand, en de relevante inhoud van de app.config over te nemen zoals geconfigureerd in deze Client Reference Application, welke handmatig is aangepast om te voldoen aan de vereisten van Edukoppeling Transactiestandaard en de onderliggende Digikoppeling 3.0 standaard.

In de applicatie is de gegenereerde class één-op-één overgenomen: SchoolID.cs, De methodes eindigend op een 1, waaronder pingRequest1, pingResponse1 etc., zijn door de svcutil aangemaakte wrapper objecten, Zo is de pingRequest1() aangemaakt als een wrapper voor het pingRequest object, en wordt gebruikt als payload voor de pingOperation. Als antwoord van de pingOperation wordt de pingResponse1 teruggegeven, dat de pingResponse object bevat.

##### Implementatie 
De applicatie is opgezet als een Visual C# Library aangevuld met een UnitTest Application en een Console Application. 

### Installatie
De applicatie is gebouwd voor en getest onder Windows 8. Om de applicatie succes te laten draaien moeten aan een aantal randvoorwaarden worden voldaan:
* De machine dient een geldig en geregistreerd TLSv1.2 certificaat te hebben op basis van de domeinnaam
* Toegang tot de Nummervoorziening applicatie vanuit de server

### Todo's
 - Edukoppeling ondersteuning.
 - Reference to relevant components (and versioning) is addressed
 - Documentation contains list of dependencies to succesfully compile and run the client
 - Documentation contains descriptions to build and test the client
 - Documentation contains references to the configuration properties required to run the system

### Licentie
Nog vast te stellen.

### Contact
Voor meer informatie kunt u contact opnemen met [Marc Fleischeuers](mailto:M.Fleischeuers@kennisnet.nl) of [Thomas Beekman](mailto:Beekman.Thomas@kpmg.nl).

**(c) Stichting Kennisnet (2016)**

[//]: # (These are reference links used in the body of this note)
   [Edukoppeling Wiki]: <http://developers.wiki.kennisnet.nl/index.php?title=Standaarden:Edukoppeling>
   [Stichting Kennisnet]: <http://www.kennisnet.nl>