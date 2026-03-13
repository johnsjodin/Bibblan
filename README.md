<center>
<img width="747" height="329" alt="Testresultat" src="https://github.com/user-attachments/assets/fc591ebf-d855-4ea8-b7d4-7f49e652ee77" />


<img width="1273" height="473" alt="Blazor-UI" src="https://github.com/user-attachments/assets/85fd6010-6903-4267-ae8e-9cda6b8d594b" />

<img width="314" height="544" alt="Blazor-UI-mobil" src="https://github.com/user-attachments/assets/3ce90279-9bdd-42a7-af46-a4191734a931" />
<img width="310" height="541" alt="Blazor-UI-mobil2" src="https://github.com/user-attachments/assets/91425b9d-5964-4534-a0f6-ccba89230542" />

</center>

# 📚 Bibblan - Bibliotekshanteringssystem

Ett komplett bibliotekshanteringssystem byggt i C# med .NET 10. Projektet består av två versioner: en konsolapplikation (Version 1) och en fullständig webbapplikation med Blazor och Entity Framework (Version 2).

---

# 📗 Version 2 - Webbapplikation (Blazor + Entity Framework)

En fullständig webbapplikation som bygger vidare på Version 1 med moderna teknologier.

## 🎯 Funktioner i Version 2

- **Blazor Server** - Modernt webbgränssnitt med responsiv design
- **Entity Framework Core** - Databashantering med SQL Server LocalDB
- **Repository Pattern** - Separerad dataåtkomst för bättre testbarhet
- **Custom Validation Attributes** - Återanvändbar validering
- **bUnit-tester** - Enhetstester för Blazor-komponenter
- **Responsiv design** - Ser bra ut på både desktop och mobil

## ✨ Funktioner (Version 2)

### 📖 Bokhantering
- Lägg till, ta bort och sök efter böcker
- Validering av ISBN (10-13 siffror), titel, författare och utgivningsår
- Formaterad ISBN-visning med bindestreck
- Detaljvy med lånehistorik

### 👥 Medlemshantering
- Registrera och hantera biblioteksmedlemmar
- Automatisk tilldelning av medlemsnummer (M001, M002, ...)
- Sök medlemmar baserat på namn, email eller medlems-ID
- Visa detaljerad medlemsinformation med aktiva lån

### 🔄 Lånehantering
- Skapa nya lån med automatisk validering
- Validering: lånedatum inte i framtiden, förfallodatum efter lånedatum
- Returnera böcker med tidsstämplar
- Spåra försenade lån med markering
- Filtrera efter aktiva, försenade eller alla lån

### 📌 Reservationer
- Reservera utlånade böcker för att få dem vid återlämning
- Endast andra medlemmar kan reservera (inte den som lånar)
- Avbryt reservation vid behov
- Reserverande medlem har förtur vid utlåning
- Skydd: kan inte låna ut till annan medlem om boken är reserverad

### 📊 Startsida med statistik
- Totalt antal böcker i katalogen
- Antal tillgängliga böcker
- Antal medlemmar
- Aktiva och försenade lån

## 🏗️ Projektstruktur (Version 2)

```
Bibblan/
├── Bibblan.Core/              # Kärnlogik från Version 1
│   ├── Book.cs                # Bokentitet med sökfunktionalitet
│   ├── Member.cs              # Medlemsentitet
│   ├── Loan.cs                # Låneentitet
│   ├── BookCatalog.cs         # Bokkatalogshantering
│   ├── MemberRegistry.cs      # Medlemsregister
│   ├── LoanManager.cs         # Lånehantering
│   ├── Library.cs             # Huvudfacade för systemet
│   ├── ISearchable.cs         # Sökinterface
│   └── Program.cs             # Konsolgränssnitt
│
├── Bibblan.Data/              # Data Access Layer (Entity Framework)
│   ├── Entities/              # EF-entiteter
│   │   ├── BookEntity.cs
│   │   ├── MemberEntity.cs
│   │   └── LoanEntity.cs
│   ├── Repositories/          # Repository Pattern
│   │   ├── IBookRepository.cs
│   │   ├── BookRepository.cs
│   │   ├── IMemberRepository.cs
│   │   ├── MemberRepository.cs
│   │   ├── ILoanRepository.cs
│   │   └── LoanRepository.cs
│   ├── Validation/            # Custom Validation Attributes
│   │   ├── IsbnHelper.cs      # ISBN-validering och formatering
│   │   └── DateValidation.cs  # Datumvalidering
│   └── LibraryContext.cs      # DbContext
│
├── Bibblan.Web/               # Blazor Server-projekt
│   ├── Components/
│   │   ├── Pages/             # Sidor
│   │   │   ├── Home.razor     # Startsida med statistik
│   │   │   ├── Books.razor    # Boklista med sök
│   │   │   ├── BookDetails.razor
│   │   │   ├── Members.razor  # Medlemslista
│   │   │   ├── MemberDetails.razor
│   │   │   └── Loans.razor    # Lånehantering
│   │   ├── Layout/            # Layout-komponenter
│   │   └── Shared/            # Återanvändbara komponenter
│   │       ├── BookCard.razor
│   │       ├── MemberCard.razor
│   │       └── LoanCard.razor
│   ├── wwwroot/               # Statiska filer
│   └── Program.cs             # Applikationskonfiguration
│
└── Bibblan.Tests/             # Enhetstester (203 tester)
    ├── BookTests.cs
    ├── MemberTests.cs
    ├── LoanTests.cs
    ├── BookCatalogTests.cs
    ├── MemberRegistryTests.cs
    ├── LoanManagerTests.cs
    ├── LibraryTests.cs
    ├── BookRepositoryTests.cs    # Repository-tester
    ├── MemberRepositoryTests.cs  # Repository-tester
    ├── LoanRepositoryTests.cs    # Repository-tester
    ├── LibraryIntegrationTests.cs # Integration med EF
    ├── BookCardTests.cs          # bUnit Blazor-tester
    ├── IsbnHelperTests.cs        # ISBN-valideringstester
    ├── DateValidationTests.cs    # Datumvalideringstester
    └── TestData.cs               # Testdata och konstanter
```

## 🗄️ Databasmodell

### ER-diagram

```
┌─────────────────┐       ┌─────────────────┐       ┌─────────────────┐
│   BookEntity    │       │   LoanEntity    │       │  MemberEntity   │
├─────────────────┤       ├─────────────────┤       ├─────────────────┤
│ Id (PK)         │       │ Id (PK)         │       │ Id (PK)         │
│ ISBN (Unique)   │       │ BookId (FK)     │───────│ MemberId (UK)   │
│ Title           │───────│ MemberId (FK)   │       │ Name            │
│ Author          │       │ LoanDate        │       │ Email (Unique)  │
│ PublishedYear   │       │ DueDate         │       │ MemberSince     │
│ IsAvailable     │       │ ReturnDate      │       └─────────────────┘
│ IsReserved      │       └─────────────────┘
│ ReservedByMemberId (FK) │
└─────────────────┘
```

### Relationer
- **Book → Loans**: En-till-många (En bok kan ha flera lån över tid)
- **Member → Loans**: En-till-många (En medlem kan ha flera lån)
- **Book → ReservedBy**: Många-till-en (En bok kan reserveras av en medlem)

## 🛠️ Teknisk Stack

- **.NET 10** - Modern .NET-plattform
- **C# 14** - Senaste C#-funktioner (GeneratedRegex, etc.)
- **Blazor Server** - Interaktivt webbgränssnitt med SignalR
- **Entity Framework Core 10** - ORM för databashantering
- **SQL Server LocalDB** - Lokal utvecklingsdatabas
- **Bootstrap 5** - Responsiv design (mobile-first)
- **xUnit v3** - Testramverk
- **bUnit 2.0** - Blazor-komponenttester

### Custom Validation Attributes
- `[Isbn]` - Validerar ISBN-10 eller ISBN-13
- `[PublishedYear]` - Validerar att år inte är i framtiden
- `[NotInFuture]` - Validerar att datum inte är i framtiden
- `[MustBeAfter]` - Validerar att datum är efter ett annat fält

## 📊 Testöversikt

Projektet innehåller **203 tester** som täcker:

| Kategori | Antal | Beskrivning |
|----------|-------|-------------|
| Core-tester | ~140 | Kärnlogik från Version 1 |
| Repository-tester | 18 | EF InMemory-databas |
| Integration-tester | 3 | Affärslogik med EF |
| bUnit-tester | 12 | Blazor-komponenter |
| Validerings-tester | 29 | ISBN och datumvalidering |

Kör alla tester:
```bash
dotnet test
```

## 🌐 Webbgränssnitt (Blazor-sidor)

| Sida | URL | Beskrivning |
|------|-----|-------------|
| Startsida | `/` | Statistik och snabbåtgärder |
| Böcker | `/books` | Lista, sök och lägg till böcker |
| Bokdetaljer | `/books/{id}` | Visa bok med lånehistorik, låna ut/returnera/reservera |
| Medlemmar | `/members` | Lista och registrera medlemmar |
| Medlemsdetaljer | `/members/{id}` | Visa medlem med lånehistorik |
| Utlåning | `/loans` | Hantera alla lån, skapa nya, återlämna |

## 🔒 Designprinciper (Version 2)

### Repository Pattern
- Separerar dataåtkomst från affärslogik
- Möjliggör enkel testning med InMemory-databas
- Interface-baserad design för dependency injection

### Responsiv Design
- Mobile-first approach med Bootstrap 5
- Kort-layout för böcker, medlemmar och lån
- Anpassad för alla skärmstorlekar

### Testbarhet
- Internal members exponeras till test-projektet via `InternalsVisibleTo`
- Dependency injection-vänlig design
- Alla metoder är testbara

---

# 📘 Version 1 - Konsolapplikation

Den ursprungliga konsolapplikationen med kärnlogik för bibliotekshantering.

## ✨ Funktioner (Version 1)

### 📖 Bokhantering
- Lägg till, ta bort och sök efter böcker
- Spåra bokstatus (tillgänglig/utlånad/reserverad)
- Sortera böcker efter titel eller utgivningsår
- Validering av ISBN, titel, författare och utgivningsår

### 👥 Medlemshantering
- Registrera och hantera biblioteksmedlemmar
- Spåra medlemmars lånehistorik
- Sök medlemmar baserat på namn, email eller medlems-ID

### 🔄 Lånehantering
- Skapa och hantera lån
- Returnera böcker med förseningsavgift
- Reservera böcker per medlem

## 🚀 Kör konsolappen

```bash
dotnet run --project Bibblan.Core
```

### Menyval

```text
--- Bibliotekssystem ---
1. Lägg till bok
2. Lista böcker
3. Lägg till medlem
4. Låna bok
5. Återlämna bok
6. Sök böcker
7. Sortera böcker (titel)
8. Sortera böcker (år)
9. Statistik
10. Reservera bok
11. Sätt förseningsavgift
0. Avsluta
```

## 🏗️ Projektstruktur (Version 1)

```
Bibblan.Core/
├── Book.cs                # Bokentitet med sökfunktionalitet
├── Member.cs              # Medlemsentitet
├── Loan.cs                # Låneentitet
├── BookCatalog.cs         # Bokkatalogshantering
├── MemberRegistry.cs      # Medlemsregister
├── LoanManager.cs         # Lånehantering
├── Library.cs             # Huvudfacade för systemet
├── ISearchable.cs         # Sökinterface
└── Program.cs             # Konsolgränssnitt
```

## 🔒 Designprinciper (Version 1)

### Inkapsling
- Alla collections exponeras som `IReadOnlyList<T>` för att förhindra extern modifiering
- Privata fält med publika readonly properties

### Separation of Concerns
- Tydlig separation mellan entiteter (Book, Member, Loan) och hanteringsklasser
- Library-klassen fungerar som facade för enkel användning

### Validering
- Alla input valideras med beskrivande felmeddelanden
- Argument null-kontroller
- Affärsregelvalidering (t.ex. böcker kan inte lånas ut två gånger)

---

# 🚀 Kom igång

### Förutsättningar
- .NET 10 SDK eller senare
- Visual Studio 2025 (eller annan C#-kompatibel IDE)
- SQL Server LocalDB (ingår i Visual Studio) - endast för Version 2

### Installation

```bash
# Klona repositoryt
git clone https://github.com/johnsjodin/Bibblan.git
cd Bibblan

# Bygg projektet
dotnet build

# Kör testerna
dotnet test

# Kör Version 2 (webbappen)
dotnet run --project Bibblan.Web

# Kör Version 1 (konsolappen)
dotnet run --project Bibblan.Core
```

Öppna webbläsaren och gå till: `https://localhost:5001`

---

## 📝 Licens

Detta projekt är skapat för utbildningssyfte (MIT License).

## 👨‍💻 Författare

**John Sjödin** - [@johnsjodin](https://github.com/johnsjodin)

---

*Byggt med ☕ och C#*
