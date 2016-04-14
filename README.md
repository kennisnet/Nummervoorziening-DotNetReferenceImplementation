# Nummervoorziening - C#.NET Client Reference Application

## Algemeen

De Nummervoorziening - C#.NET Client Reference Application is een client implementatie in C#.NET dat middels diverse unit tests de communicatie met de Nummervoorziening applicatie binnen de Educatieve Contentketen aantoont. De applicatie is opgezet als een Visual C# Library aangevuld met een UnitTest Application en een Console Application. De C#.NET Client Reference Application implementeert globaal de volgende functionaliteiten;
 * Globale communicatie met de Nummervoorziening applicatie (PingOperation t.b.v. het vaststellen van een correcte verbinding met en werking van de Nummervoorziening applicatie).
 * Genereren van een eerste niveau hash van een PGN middels het SCrypt hashing algoritme.
 * Het opvragen van een ECK-ID bij de Nummervoorziening applicatie op basis van een gehashte PGN, een keten-id en sector-id (RetrieveEckIdOperation).
 * Het opvragen van ondersteunde ketens bij de Nummervoorziening applicatie (RetrieveChainsOperation).
 * Het opvragen van ondersteunde sectoren bij de Nummervoorziening applicatie (RetrieveSectorsOperation).
 * Opvoeren van nieuwe gehashte PGNs die dienen te worden doorverwezen naar reeds bestaande gehashte PGNs bij het creëren van ECK-IDs (ReplaceEckIdOperation).
 * Het opvoeren van een batch met gehaste PGNs (SubmitEckIdBatchOperation).
 * Het opvragen van de status en/of ophalen van een batch met ECK-IDs (RetrieveEckIdBatchOperation).

Voor alle bovenstaande functionaliteiten wordt gebruik gemaakt van SOAP Messaging tussen de verschillende actoren. Authenticatie vindt plaats middels PKI-Certificaten die over TLS worden uitgewisseld. Aanvullende informatie over Nummervoorziening kan gevonden worden op de [Edukoppeling Wiki] van [Stichting Kennisnet].  

## Version
0.1-20160414

## Gebruikte Technologiën

Bij de ontwikkeling van Nummervoorziening - C#.NET Client Reference Application zijn diverse technologiën ingezet:
 * .NET v4.5.2 - Microsoft's .NET Framework
 * Visual Studio 2015 - Ontwikkelomgeving
   * svcutil.exe - Utility voor het creëren van WCF proxy service interface op basis van een WSDL
 * Fiddler - TLS/SSL debugging
 * Wireshark - TLS/SSL debugging
 * SOAP UI - Functional Testing framework for SOAP and REST APIs

## Communicatie
De Client Reference Application communiceert met de Nummervoorziening applicatie middels het SOAP-messaging protocol. Hierbij wordt het profiel 2W-be (tweeweg, Best Effort) zoals beschreven in de Edukoppeling Transactiestandaard versie 1.2 en Digikoppeling WUS 3.0 gevolgd. Praktisch gezien houdt dit het volgende in:
 * synchrone uitwisselingen die geen faciliteiten voor betrouwbaarheid (ontvangstbevestigingen, duplicaateliminatie etc.) vereisen. Voorbeelden zijn toepassingen waar het eventueel verloren raken van sommige berichten niet problematisch is en waar snelle verwerking gewenst is.
 * gebaseerd op SOAP 1.1.
 * gebruik van de verplichte WS-Addressing 1.0 headers, waarbij de From header verplicht is, en de ReplyTo header niet wordt gebruikt.
 * gebruik van PKIoverheid certificaten voor point-to-point security.
 * geen gebruik van End-to-End beveiliging (geen WS-Security: signing en/of encryptie van de SOAP headers of body).

## Solution Modules
 * **ConsoleApplication**: Voorbeeldapplicatie om de werking van de *schoolID* module te demonstreren
 * **CryptSharp**: 3rd party library genaamd [CryptSharp] met (onder andere) de functionaliteit om op basis van het hashingalgoritme SCrypt hashes te genereren.  
 * **SchoolID**: Library met de basisfunctionaliteiten om de Nummervoorziening applicatie op een juiste wijze te kunnen bevragen. 
 * **UnitTestProject**: Voorbeeldcode voor het gebruik van de Nummervoorziening applicatie, tevens gebruikmakend van de *SchoolID* module en daaronder liggende *CryptSharp* module.

### ConsoleApplication - Structuur
 * **App.config**: Bevat de .NET applicatie configuratie. Dit configuratiebestand bevat de applicatiespecifieke instellingen (*appSettings*) voor het instellen van de identificatie van de client middels de OIN en bijbehorende client certificaat en de configuratie van de SOAP verbinding met de Nummervoorziening applicatie (*system.serviceModel*).
 * **Program.cs**: Simpele class met een Main hook. Deze class wordt als Console applicatie gebruikt om een subset van specifieke functionaliteiten en bijbehorende operaties te demonstreren.  
 
### SchoolID - Structuur
 * **Operations**: Bevat de SchoolID specifieke operaties voor het uitwisselen van data. Uitgangspunt bij deze implementatie is het gebruik van "standaard" classes als parameters, om zodoende niet SchoolID specifieke objecten door de gehele client applicatie te hoeven gebruiken. Voor elke beschikbare operatie is een separate Class opgenomen in deze Library.
  * *PingOperation.cs*
  * *ReplaceEckIdOperation.cs*
  * *RetrieveChainsOperation.cs*
  * *RetrieveEckIdBatchOperation.cs*
  * *RetrieveEckIdOperation.cs*
  * *RetrieveSectorsOperation.cs*
  * *SubmitEckIdBatchOperation.cs*
  * **Resources**: Aanvullende bestanden ter ondersteuning van de Solution.
    * *schoolid.wsdl*: De WSDL welke is gebruikt als input voor de *svcutil.exe* utility.
 * **SCrypter**: Bevat de logica ter aansturing van de CryptSharp library.
    * *Constants.cs* De SCrypt constanten zoals vastgesteld.
    * *ScryptUtil.cs* Bevat de *GenerateHash()* functie die in de rest van de Library wordt gebruikt om de eerste niveau hash te berekenen.  
 * **SchoolID.cs**: De door *svcutil.exe* gegenereerde WCF interface op basis van de *schoolid.wsdl*.
 * **SchoolIDBatch.cs**: Collectie class voor de opslag en verwerking van opgehaalde batches uit de Nummervoorziening applicatie gebruikmakend van standaard C# objecten.  
 * **SchoolIDServiceUtil.cs**: Singleton service util class voor centrale initializatie van de verbinding met de Nummervoorziening applicatie (certificaten & WS-Adressing) en het uitvoeren van operaties.
 
### UnitTestProject - Structuur
 * **AbstractUnitTest.cs**: Basis Class voor het initializeren van de SchoolIDServiceUtil Singleton instance en het uitschakelen van aanvullende SSL beveiliging om self-signed certificaten te kunnen ondersteunen. Alle UnitTest classes erven over van de AbstractUnitTest class.  
 * **App.config**: Bevat de .NET applicatie configuratie. Dit configuratiebestand bevat de applicatiespecifieke instellingen (*appSettings*) voor het instellen van de identificatie van de client middels de OIN en bijbehorende client certificaat en de configuratie van de SOAP verbinding met de Nummervoorziening applicatie (*system.serviceModel*).
 * **BatchOperationUnitTest.cs**: Voorbeeldcode voor het uitvoeren van een Batch Operation: het submitten van een batch, controleren van de status van de verwerking en het ophalen van het resultaat. 
 * **PingOperationUnitTest.cs**: Voorbeeldcode voor het uitvoeren van een Ping Operation: het uitlezen van de status van de Nummervoorziening applicatie.
 * **ReplaceEckIdOperationUnitTest.cs**: Voorbeeldcode voor het uitvoeren van een Replace ECK ID Operation: het vervangen van een nieuwe HPGN door een reeds bestaande HPGN om zodoende het reeds uitgegeven ECK ID te kunnen blijven gebruiken in de keten bij een PGN/BSN wijziging.
 * **RetrieveChainsOperationUnitTest.cs**: Voorbeeldcode voor het uitvoeren van een Retrieve Chains Operation: het ophalen van ondersteunde ketens in de Nummervoorziening applicatie.
 * **RetrieveEckIdOperationUnitTest.cs**: Voorbeeldcode voor het uitvoeren van een Retrieve Eck ID Operation: het ophalen van een enkele ECK ID in de Nummervoorziening applicatie op basis van een eerste niveau hash, keten id en sector id.
 * **RetrieveSectorsOperationUnitTest.cs**: Voorbeeldcode voor het uitvoeren van een Retrieve Sectors Operation: het ophalen van ondersteunde sectoren in de Nummervoorziening applicatie.
 * **ScryptUtilUnitTest.cs**: Voorbeeldcode voor het genereren van een eerste niveau hash op basis van een PGN.

## Applicatie Configuratie
De applicatie configuratie (*App.config*) is opgenomen in de twee applicatie modules: *ConsoleApplication* en *UnitTestProject* en zijn identiek. De configuratie is op te delen in twee secties: de globale configuratie in *appSettings* en de WS/SOAP binding configuratie in de *system.serviceModel* sectie.

Het service model is ingericht om de configuratie van de SOAP endpoint binding te laten voldoen aan de eisen gesteld door de Edukoppeling Transactiestandaard en onderliggende Digikoppeling standaard. Het endpoint heeft zodoende een custom binding om de ondersteuning van WS-Adressing te activeren en een endpoint behavior om het gebruik van client certificaten te vereisen.

### appSettings
 * **instanceOIN**: de op de BRIN4 gebaseerde OIN van de School.
 * **certificateDnsIdentity**: de DNS identity behorend bij het client certificaat. 
 * **certificateFileName**: De bestandsnaam van het client certificaat. Het certificaat, een pfx/p12 bestand, wordt verwacht geplaatst te zijn in dezelfde directory als de *App.config*, en beveiligd te zijn met een wachtwoord.
 * **certificatePassword**: Het wachtwoord behorend bij het client certificaat.

### system.serviceModel
 * **client.endpoint**
  * *address*: De url van de Nummervoorziening applicatie
  * *binding*: De gebruikte binding voor deze endpoint (default: *customBinding*)
  * *bindingConfiguration*: De gebruikte configuratie voor de binding (default: *SchoolIdSoapBinding*)
  * *contract*: verwijzing naar de WCF interface class (default: *SchoolID*)
  * *behaviorConfiguration*: De gebruikte behavior voor deze endpoint (default: *SchoolIdBehavior*)
  * *name*: Naam van het endpoint (default: *SchoolIDSoap10*)
 * **bindings.customBinding.binding**: Configuratie van de custom binding (default name: *SchoolIDSoapBinding*)
  * *textMessageEncoding*: Activeert WS-Addressing volgens een gedefinieerd profiel (default messageVersion: *Soap11WSAddressing10*)
  * *httpsTransport*: Definieert de vereisten aan de transportlaag; het gebruik van https. Default waarden zijn maxReceivedMessageSize="524288000", requireClientCertificate="true", authenticationScheme="Anonymous".
 * **behaviors.endpointBehaviors.behavior**: Configuratie van de custom behavior (default name: *SchoolIDBehavior*).
  * *clientCredentials.clientCredentials*: Activeert het gebruik van client certificaten als een vereiste voor het uitvoeren van de SOAP operaties.
   
## Ontwikkeling

### WSDL naar Proxy Class
Voor de ontwikkeling van de applicatie is de WSDL als uitgangspunt genomen. De WSDL wordt aangeboden via een TLS/SSL beveiligde verbinding, waar de standaard .NET utilities niet (eenvoudig) bij kunnen. Het is daarom aan te raden de WSDL eerst te downloaden en lokaal op te slaan.

Voor het omzetten van een WSDL naar gegenereerde code zijn uit de .NET toolset grofweg twee utilities beschikbaar; *wsdl.exe* en *svcutil.exe*. De utility wsdl.exe zet een WSDL om in een webservice, waarbij de nieuwe class SoapHttpClientProtocol overerft. Het resultaat kan gebruikt worden als ASMX webservice. De utility *svcutil.exe* zet een WSDL om in een WCF proxy class (interface), die gebruikt kan worden door webservices door de interface te implementeren en de class SoapHttpClientProtocol te overerven. Omdat het nieuwere WCF door Microsoft is aangemerkt om ASMX services te vervangen, is voor de ontwikkeling van deze applicatie gekozen om de webservices middels WCF te implementeren. De gebruikte commando voor de utility is als volgt:
```
svcutil.exe /mc /syncOnly schoolid.wsdl
```
De *svcutil.exe* bleek niet in staat diverse WS-Addressing policies (zoals ```<wsaw:UsingAddressing wsdl:required="true" />```) op een juiste manier te verwerken. Mits deze regel is toegevoegd aan de WSDL, toont de *svcutil.exe* een warning en genereerd deze geen output.config bestand. Bij het opnieuw genereren van de proxy class is het daarom aan te raden de verwijzingen naar WS-Addressing te verwijderen uit het WSDL bestand, en de relevante inhoud van de app.config over te nemen zoals geconfigureerd in deze Client Reference Application, welke handmatig is aangepast om te voldoen aan de vereisten van Edukoppeling Transactiestandaard en de onderliggende Digikoppeling 3.0 standaard.

In de applicatie is de gegenereerde class één-op-één overgenomen: SchoolID.cs, De methodes eindigend op een 1, waaronder pingRequest1, pingResponse1 etc., zijn door de svcutil aangemaakte wrapper objecten, Zo is de pingRequest1() aangemaakt als een wrapper voor het pingRequest object, en wordt gebruikt als payload voor de pingOperation. Als antwoord van de pingOperation wordt de pingResponse1 teruggegeven, dat de pingResponse object bevat.

## Installatie
De applicatie is gebouwd voor en getest onder Windows 8. Om de applicatie succesvol te laten draaien moet aan een aantal randvoorwaarden worden voldaan:
 * De machine dient een geldig en geregistreerd TLS client certificaat te hebben waarmee geidentificeerd kan worden bij de Nummervoorziening applicatie
 * Het certificaat is in de juiste directory geplaatst (naast de *App.config*)
 * App.config aangepast op basis van certificaat gegevens
 * De broncode is geladen in Visual Studio 2015. 
 * Er zijn geen aanvullende plugins en/of libraries benodigd.
 * Na een build worden de Unit Tests getoond in de Test Explorer, en kunnen deze worden uitgevoerd.

## Licenties
 * **Nummervoorziening - C#.NET Client Reference Application**: Nog vast te stellen.
 * **CryptSharp**: Copyright (c) 2010, 2013 James F. Bellinger <http://www.zer7.com/software/cryptsharp>

## Contact
Voor meer informatie kunt u contact opnemen met [Marc Fleischeuers](mailto:M.Fleischeuers@kennisnet.nl).

** Copyright(c) 2016 [Stichting Kennisnet]**

[//]: # (These are reference links used in the body of this note)
   [Edukoppeling Wiki]: <http://developers.wiki.kennisnet.nl/index.php?title=Standaarden:Edukoppeling>
   [Stichting Kennisnet]: <http://www.kennisnet.nl>
   [CryptSharp]: <http://www.zer7.com/software/cryptsharp>