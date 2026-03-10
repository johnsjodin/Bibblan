
<img width="578" height="227" alt="Testresultat" src="https://github.com/user-attachments/assets/61b7a73b-8453-46b1-b530-6ebf82b7b0ed" />



# 📚 Bibblan - Bibliotekshanteringssystem

Ett komplett bibliotekshanteringssystem byggt i C# med .NET 10. Bibblan innehåller ett webbgränssnitt i Blazor Server med Entity Framework Core för databashantering.

## 🎯 Version 2.0 - Nu med Blazor och Entity Framework!

Den här versionen har utökats med:
- **Blazor Server** - Modernt webbgränssnitt med responsiv design
- **Entity Framework Core** - Databashantering med SQL Server LocalDB
- **Repository Pattern** - Separerad dataåtkomst för bättre testbarhet
- **bUnit-tester** - Enhetstester för Blazor-komponenter

## ✨ Funktioner

### 📖 Bokhantering
- Lägg till, ta bort och sök efter böcker
- Spåra bokstatus (tillgänglig/utlånad/reserverad)
- Sortera böcker efter titel, författare eller utgivningsår
- Validering av ISBN (10-13 siffror), titel, författare och utgivningsår
- Formaterad ISBN-visning med bindestreck
- Detaljvy med lånehistorik

### 👥 Medlemshantering
- Registrera och hantera biblioteksmedlemmar
- Spåra medlemmars lånehistorik
- Sök medlemmar baserat på namn, email eller medlems-ID
- Visa detaljerad medlemsinformation med aktiva lån

### 🔄 Lånehantering
- Skapa nya lån med automatisk validering
- Validering: lånedatum inte i framtiden, förfallodatum efter lånedatum
- Returnera böcker med tidsstämplar
- Spåra försenade lån med markering
- Filtrera efter aktiva, försenade eller alla lån

### 🗓️ Reservationer
- Reservera böcker per medlem
- Blockerar utlån om boken är reserverad av annan medlem

### 📊 Startsida med statistik
- Totalt antal böcker i katalogen
- Antal tillgängliga böcker
- Antal medlemmar
- Aktiva och försenade lån

## 🏗️ Projektstruktur

```
Bibblan/
├── Bibblan.Core/              # Kärnlogik från Del 1
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
└── Bibblan.Tests/             # Enhetstester (196 tester)
    ├── BookTests.cs
    ├── MemberTests.cs
    ├── LoanTests.cs
    ├── BookCatalogTests.cs
    ├── MemberRegistryTests.cs
    ├── LoanManagerTests.cs
    ├── LibraryTests.cs
    ├── BookRepositoryTests.cs    # Repository-tester
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

## 🚀 Kom igång

### Förutsättningar
- .NET 10 SDK eller senare
- Visual Studio 2025 (eller annan C#-kompatibel IDE)
- SQL Server LocalDB (ingår i Visual Studio)

### Installation

1. Klona repositoryt:
```bash
git clone https://github.com/johnsjodin/Bibblan.git
cd Bibblan
```

2. Bygg projektet:
```bash
dotnet build
```

3. Kör Blazor-webbappen:
```bash
dotnet run --project Bibblan.Web
```

4. Öppna webbläsaren och gå till: `https://localhost:5001` eller `http://localhost:5000`

### Kör testerna
```bash
dotnet test
```

### Kör konsolappen (Del 1)
```bash
dotnet run --project Bibblan.Core
```

## 📊 Testöversikt

Projektet innehåller **196 tester** som täcker:

| Kategori | Antal | Beskrivning |
|----------|-------|-------------|
| Core-tester | ~140 | Kärnlogik från Del 1 |
| Repository-tester | 12 | EF InMemory-databas |
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
| Böcker | `/books` | Lista, sök, sortera och lägg till böcker |
| Bokdetaljer | `/books/{id}` | Visa bok med lånehistorik, låna ut/returnera |
| Medlemmar | `/members` | Lista och registrera medlemmar |
| Medlemsdetaljer | `/members/{id}` | Visa medlem med lånehistorik |
| Utlåning | `/loans` | Hantera alla lån, skapa nya, återlämna |

## 📜 Licens

MIT License

## 💡 Användningsexempel

Kör programmet och följ menyvalen i konsolen:

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

Exempel: välj menyval `1` för att lägga till en bok och följ inmatningsstegen.

## 🧪 Testning

Projektet har enhetstester som täcker:

- ✅ Konstruktorvalidering
- ✅ Affärslogik
- ✅ Edge cases och felhantering
- ✅ Datavalidering
- ✅ Integrationstester

Kör alla tester:
```bash
dotnet test --logger "console;verbosity=detailed"
```

## 🔒 Designprinciper

### Inkapsling
- Alla collections exponeras som `IReadOnlyList<T>` för att förhindra extern modifiering
- Privata fält med publika readonly properties

### Repository Pattern
- Separerar dataåtkomst från affärslogik
- Möjliggör enkel testning med InMemory-databas
- Interface-baserad design för dependency injection

### Validering
- Custom validation attributes för återanvändbar validering
- Server-side validering via DataAnnotations
- Client-side validering i Blazor-formulär
- Argument null-kontroller
- Affärsregelvalidering (t.ex. böcker kan inte lånas ut två gånger)

### Responsiv Design
- Mobile-first approach med Bootstrap 5
- Kort-layout för böcker, medlemmar och lån
- Anpassad för alla skärmstorlekar

### Separation of Concerns
- Tydlig separation mellan entiteter (Book, Member, Loan) och hanteringsklasser (BookCatalog, MemberRegistry, LoanManager)
- Library-klassen fungerar som facade för enkel användning

### Testbarhet
- Internal members exponeras till test-projektet via `InternalsVisibleTo`
- Dependency injection-vänlig design
- Alla metoder är testbara

## 📝 Licens

Detta projekt är skapat för utbildningssyfte.

## 👨‍💻 Författare

**John Sjödin**
- GitHub: [@johnsjodin](https://github.com/johnsjodin)

## 🤝 Bidrag

Contributions, issues och feature requests är välkomna!

---

*Byggt med ❤️ och C#*
