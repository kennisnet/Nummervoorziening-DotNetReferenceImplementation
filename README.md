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
 * **App.config** (standaard .NET classes)

 ### CryptSharp - Structuur
 * **App.config** (standaard .NET classes)
 
 ### SchoolID - Structuur
 * **App.config** (standaard .NET classes)
 
 ### UnitTestProject - Structuur
 * **App.config** (standaard .NET classes)

### Ontwikkeling

##### WSDL naar Proxy Class
Voor de ontwikkeling van de applicatie is de WSDL als uitgangspunt genomen. De WSDL wordt aangeboden via een TLS/SSL beveiligde verbinding, waar de standaard .NET utilities niet (eenvoudig) bij kunnen. Het is daarom aan te raden de WSDL eerst te downloaden en lokaal op te slaan.

Voor het omzetten van een WSDL naar gegenereerde code zijn uit de .NET toolset grofweg twee utilities beschikbaar; wsdl.exe en svcutil.exe. De utility wsdl.exe zet een WSDL om in een webservice, waarbij de nieuwe class SoapHttpClientProtocol overerft. Het resultaat kan gebruikt worden als ASMX webservice. De utility svcutil.exe zet een WSDL om in een WCF proxy class (interface), die gebruikt kan worden door webservices door de interface te implementeren en de class SoapHttpClientProtocol te overerven. Omdat het nieuwere WCF door Microsoft is aangemerkt om ASMX services te vervangen, is voor de ontwikkeling van deze applicatie gekozen om de webservices middels WCF te implementeren. De gebruikte commando voor de utility is als volgt:
```
svcutil.exe /mc /syncOnly schoolid.wsdl
```
De svcutil.exe bleek niet in staat diverse WS-Addressing policies (zoals <wsaw:UsingAddressing wsdl:required="true" />) op een juiste manier te verwerken. Mits deze regel is toegevoegd aan de WSDL, toont de svcutil.exe een warning en genereerd deze geen output.config bestand. Bij het opnieuw genereren van de proxy class is het daarom aan te raden de verwijzingen naar WS-Addressing te verwijderen uit het WSDL bestand, en de app.config of de web.config over te nemen zoals geconfigureerd in deze Client Reference Application, welke handmatig is aangepast om te voldoen aan de vereisten van Edukoppeling Transactiestandaard en de onderliggende Digikoppeling 3.0 standaard.

In de applicatie is de gegenereerde class één-op-één overgenomen: SchoolID.cs, De methodes eindigend op een 1, waaronder pingRequest1, pingResponse1 etc., zijn door de svcutil aangemaakte wrapper objecten, Zo is de pingRequest1() aangemaakt als een wrapper voor het pingRequest object, en wordt gebruikt als payload voor de pingOperation. Als antwoord van de pingOperation wordt de pingResponse1 teruggegeven, dat de pingResponse object bevat.

##### Implementatie 
De applicatie is opgezet als een Visual C# Library aangevuld met een UnitTest Application en een Console Application. De standaard opbouw van deze applicatie template is gevolgd voor de implementatie als Doelsysteem; het systeem dat bij het Traffic Center een sessie initieert, bij een Bronsysteem een dossier ophaalt, en bij het Traffic Center de sessie afsluit. De implementatie is vastgelegd in de HomeController.cs en in de Views.

Vanuit de HomeController.cs wordt de class OverstapService.cs aangeroepen voor de communicatie met het Traffic Center en het (beoogde) Bronsysteem. De OverstapService.cs implementeert de proxy class / interface IOVerstapService.cs en overerft SoapHttpClientProtocol. De OverstapService.cs biedt tevens de functionaliteiten om de OSO clientcertificaat in te lezen en te gebruiken bij de communicatie t.b.v. de authenticatie. Mede voor deze initialisatie en tevens wegens sessiebeheer richting het Traffic Center is de OverstapService.cs geïmplementeerd als een Singleton. 

Er is één WSDL beschikbaar voor alle systemen binnen het OSO domein (Traffic Center, Bronsystemen en Doelsystemen). Als Doelsysteem dient de applicatie eerst aan het Traffic Center het opvragen van een Dossier bij een Bronsysteem aan te melden alvorens het Dossier bij een Bronsysteem kan worden opgehaald. Dit betekent dat de proxy class voor twee verschillende doelen moet worden ingezet (Traffic Center en Bronsysteem). In de code is dit opgelost door een DocumentRequest aan het Bronsysteem uit te voeren met de aanvullende functie InvokeWithUrl(), dat tijdelijk een nieuw Object initialiseert op basis van de class Overstap, deze initialiseert met dezelfde certificaatgegevens maar met een ander targetUrl. De opgehaalde DocumentResponse wordt vervolgens weer teruggegeven via de Overstap Singleton aan de HomeController.

Het Doelsysteem is opgebouwd met formulieren waarbij parameters handmatig kunnen worden ingevuld. Dit biedt de vrijheid om de applicatie in te zetten voor testwerkzaamheden. De authenticatie van het Doelsysteem, gebaseerd op een OSO-certificaat, kan niet binnen de draaiende applicatie worden gewijzigd. De configuratie hiervan is vastgelegd in de Web.config, waarbij het certificaat als .pfx of .p12 in de applicatie-directory dient te worden geplaatst. De configuratie hiervan vindt zodoende plaats buiten de IIS configuratie.

##### Implementatie 
De implementatie van het Bronsysteem is gerealiseerd in de Bronservice.svc. De Bronservice class implementeert eveneens de proxy class / interface IOVerstapService.cs en ook overerft deze de class SoapHttpClientProtocol. De Bronservice implementeert alleen de functie document() t.b.v. een documentRequest. Bij een ontvangen request wordt bij het Traffic Center een sessiecontrole uitgevoerd. Daarnaast wordt het request getoetst tegen de gebruikte client certificaat. Als alles goed is verlopen, en het dossier van de bevraagde bsn beschikbaar is (061463905, 074200689, 241865542, 540258209, 690069212), wordt deze middels een DocumentResponse teruggegeven. De configuratie van de Bronservice is geheel opgenomen in de Web.config. 

### Installatie
De applicatie is gebouwd voor en getest onder Windows 8. Om de applicatie succes te laten draaien moeten aan een aantal randvoorwaarden worden voldaan:
* De machine dient een geldig en geregistreerd TLSv1.2 certificaat te hebben op basis van de domeinnaam
* Toegang tot de Nummervoorziening applicatie vanuit de server
* Voor de website dient alleen een binding te worden ingericht voor https, gekoppeld aan het domeincertificaat.


### Todo's
 - Edukoppeling ondersteuning.
 - Reference to relevant components (and versioning) is addressed
 - Documentation contains list of dependencies to succesfully compile and run the client
 - Documentation contains descriptions to build and test the client
 - Documentation contains references to the configuration properties required to run the system

### Licentie
Nog vast te stellen.

### Contact
Voor meer informatie kunt u contact opnemen met [Marc Fleischeurs](mailto:M.Fleischeurs@kennisnet.nl) of [Thomas Beekman](mailto:Beekman.Thomas@kpmg.nl).

**(c) Stichting Kennisnet (2016)**

[//]: # (These are reference links used in the body of this note)
   [Edukoppeling Wiki]: <http://developers.wiki.kennisnet.nl/index.php?title=Standaarden:Edukoppeling>
   [Stichting Kennisnet]: <http://www.kennisnet.nl>