# SecureLogin_ASPNet_MVC_EF_CSharp

Sicheres Login mit doppeltem Hashing des Passworts – nach Verwendung von Identity

---

## Über dieses Projekt

Dieses Projekt zeigt eine Beispielimplementierung eines sicheren Login-Systems in ASP.NET MVC unter Verwendung von Entity Framework (EF) und zusätzlichem doppeltem Hashing des Passworts. Dabei wird nach dem üblichen Identity-System ein weitergehender Schutzmechanismus eingesetzt.

---

## Funktionen

> + Verwendung von ASP.NET Core Identity zur Benutzerverwaltung (Registrierung, Login usw.)
> + Zusätzliche Hash-Ebene für Passwörter: zwei unterschiedliche Hash-Methoden werden nacheinander angewendet
> + Nutzung von Entity Framework Core zur Persistierung von Benutzerdaten in einer Datenbank
> + Strukturierter Architekturaufbau mit klaren Verzeichnissen für Controller, Data, Models, Utility, ViewModels, Views
> + Konfigurierbare Einstellungen über appsettings.json bzw. appsettings.Development.json
> + Projekt aufgebaut in C# (.NET) – mit Ziel, gute Praxis beim sicheren Login darzustellen

---

## Projektstruktur

```
PasswordSolution

```
## Voraussetzungen

+ .NET SDK (z. B. .NET 9 oder .NET 10)
+ Visual Studio, IntelliJ Rider oder ein alternativer C#-Entwicklungseditor
+ SQL-Datenbank (z. B. SQL Server, SQLite oder eine andere von EF unterstützte Datenbank)
+ Kenntnis in ASP.NET MVC, Entity Framework und Identity wird empfohlen

## Installation & Einrichtung

### Repository klonen oder herunterladen
```
 git clone https://github.com/Siandolor/SecureLogin_ASPNet_MVC_EF_CSharp.git
```

### Zusätzliche Libraries herunterladen
```
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design

dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI

dotnet add package SHA3.Net
dotnet add package Konscious.Security.Cryptography.Argon2

```

### Build
```bash
  dotnet build
```

### Run
```bash
  dotnet run
```

### **Projekt in Ihrer Entwicklungsumgebung öffnen (z. B. Visual Studio).**

In den appsettings.json bzw. appsettings.Development.json Datenbank-Verbindungszeichenfolge (Connection String) anpassen.  
Bei Bedarf Datenbankmigrationen ausführen (z. B. mit dotnet ef migrations bzw. über Visual Studio) und Datenbank aktualisieren.  

```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Projekt bauen und starten. Beim ersten Start werden Benutzer‐Registrierung und Login getestet.

---

## Architekturhinweise

+ **Controllers**: Hier befinden sich die Controller‐Klassen für Registrierung, Login, Benutzerverwaltung etc.
+ **Data**: Datenzugriffs-Schicht einschließlich Datenbankkontext (DbContext) und Initialisierung.
+ **Models**: Domänenklassen wie „User“, „Role“ etc.
+ **ViewModels**: Modelle, die speziell für Views (Registrierung, Login, Profil) verwendet werden.
+ **Utility**: Hilfsklassen zum Beispiel für das Hashing, Sicherheitsfunktionen etc.
+ **Views**: Razor-Views für die UI.
+ **wwwroot**: Statische Dateien wie CSS, JavaScript, Bilder.

## Sicherheit

Besonderer Schwerpunkt liegt auf dem doppelten Hashing von Passwörtern:  
> Zuerst wird das Passwort über die Standard-Identity-Mechanismen (in diesem Fall *SHA3 512* mit Fallback auf *SHA2 512*) verarbeitet.  
> Danach wird eine zusätzliche Hash-Methode angewendet (in diesem Fall *Argon2Id*), sodass ein zweiter Schutzlayer entsteht.  
> Damit wird ein wesentlich höheres Sicherheitsniveau gegenüber einer „nur Identity“ Lösung erreicht.

### Achtung:
```
Dieses Beispiel stellt kein fertiges Produktionssystem dar.
```
Es soll als Vorlage und Lernprojekt dienen. In einer produktiven Umgebung müssen weitere Aspekte wie z. B. Multi-Factor-Authentication (MFA), sichere Schlüsselverwaltung, Überwachung, Logging, Schutz gegen Brute-Force, Passwort-Richtlinien usw. implementiert werden.

---

## Anpassung & Erweiterung

- Wechseln Sie die zweite Hash-Methode (im Utility‐Verzeichnis) nach Bedarf.
- Passen Sie die Identity-Konfiguration an (z. B. Rollen, Ansprüche, Passwort-Richtlinien).
- Erweitern Sie den Login um z. B. Zwei-Faktor-Authentifizierung, Captcha, Account-Sperrung.
- Optimieren Sie das UI/UX (Styles, Layout, Responsivität).
- Fügen Sie Tests hinzu – Unit-Tests für die Hashing-Mechanismen, Integrationstests für das Login.

---

## Autor

**Daniel Fitz, MBA, MSc, BSc**  
Vienna, Austria  
Developer & Security Technologist — *Post-Quantum Cryptography, Blockchain/Digital Ledger & Simulation*  
C/C++ · C# · Java · Python · Visual Basic · ABAP · JavaScript/TypeScript

International Accounting · Macroeconomics & International Relations · Physiotherapy · Computer Sciences  
Former Officer of the German Federal Armed Forces

---

## Lizenz

Dieses Projekt steht unter der MIT License. Siehe Datei LICENSE für Einzelheiten.

---

> "Entropy is not chaos.  
> It’s the silence between two collisions of probability."  
> — Daniel Fitz, 2025
> 
