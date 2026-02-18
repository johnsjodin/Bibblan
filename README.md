
<img width="578" height="227" alt="Testresultat" src="https://github.com/user-attachments/assets/61b7a73b-8453-46b1-b530-6ebf82b7b0ed" />



# 📚 Bibblan - Bibliotekshanteringssystem

Ett komplett bibliotekshanteringssystem byggt i C# med .NET 10. Bibblan låter dig hantera böcker, medlemmar och utlåning på ett enkelt och effektivt sätt.

## ✨ Funktioner

### 📖 Bokhantering
- Lägg till, ta bort och sök efter böcker
- Spåra bokstatus (tillgänglig/utlånad/reserverad)
- Sortera böcker efter titel eller utgivningsår
- Validering av ISBN, titel, författare och utgivningsår

### 👥 Medlemshantering
- Registrera och hantera biblioteksmedlemmar
- Spåra medlemmars lånehistorik
- Sök medlemmar baserat på namn, email eller medlems-ID
- Visa detaljerad medlemsinformation med aktiva lån

### 🔄 Lånehantering
- Skapa nya lån med automatisk validering
- Returnera böcker med tidsstämplar
- Spåra försenade lån och beräkna förseningsavgift
- Visa endast aktiva lån

### 🗓️ Reservationer
- Reservera böcker per medlem
- Blockerar utlån om boken är reserverad av annan medlem

### 📊 Statistik
- Totalt antal böcker i katalogen
- Antal utlånade böcker
- Hitta mest aktiva låntagare
- Filtrera och sortera data

## 🏗️ Projektstruktur

```
Bibblan/
├── Bibblan.Core/              # Kärnlogik
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
└── Bibblan.Tests/             # Enhetstester
    ├── BookTests.cs           # Tester för Book
    ├── MemberTests.cs         # Tester för Member
    ├── LoanTests.cs           # Tester för Loan
    ├── BookCatalogTests.cs    # Tester för BookCatalog
    ├── MemberRegistryTests.cs # Tester för MemberRegistry
    ├── LoanManagerTests.cs    # Tester för LoanManager
    └── LibraryTests.cs        # Integrationstester
```

## 🛠️ Teknisk Stack

- **.NET 10** - Modern .NET-plattform
- **C# 14** - Senaste C#-funktioner
- **xUnit** - Testramverk med 100+ enhetstester
- **LINQ** - Kraftfull databearbetning

## 🚀 Kom igång

### Förutsättningar
- .NET 10 SDK eller senare
- Visual Studio 2025 (eller annan C#-kompatibel IDE)

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

3. Kör konsolappen:
```bash
dotnet run --project Bibblan.Core
```

4. Kör testerna:
```bash
dotnet test
```

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

### Validering
- Alla input valideras med beskrivande felmeddelanden
- Argument null-kontroller
- Affärsregelvalidering (t.ex. böcker kan inte lånas ut två gånger)

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
