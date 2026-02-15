namespace Bibblan.Core
{
    internal class Program
    {
        private const decimal DailyLateFee = 10m;

        static void Main(string[] args)
        {
            var library = new Library(new BookCatalog(), new MemberRegistry(), new LoanManager());
            var running = true;

            while (running)
            {
                // Enkel konsolmeny för bibliotekets funktioner.
                Console.WriteLine("\n--- Bibliotekssystem ---");
                Console.WriteLine("1. Lägg till bok");
                Console.WriteLine("2. Lista böcker");
                Console.WriteLine("3. Lägg till medlem");
                Console.WriteLine("4. Låna bok");
                Console.WriteLine("5. Återlämna bok");
                Console.WriteLine("6. Sök böcker");
                Console.WriteLine("7. Sortera böcker (titel)");
                Console.WriteLine("8. Sortera böcker (år)");
                Console.WriteLine("9. Statistik");
                Console.WriteLine("10. Reservera bok");
                Console.WriteLine("0. Avsluta");
                var choice = ReadRequiredInput("Val: ");

                switch (choice)
                {
                    case "1":
                        AddBook(library);
                        break;
                    case "2":
                        ListBooks(library.BookCatalog.GetAll());
                        break;
                    case "3":
                        AddMember(library);
                        break;
                    case "4":
                        LoanBook(library);
                        break;
                    case "5":
                        ReturnBook(library);
                        break;
                    case "6":
                        SearchBooks(library);
                        break;
                    case "7":
                        ListBooks(library.BookCatalog.SortByTitle());
                        break;
                    case "8":
                        ListBooks(library.BookCatalog.SortByYear());
                        break;
                    case "9":
                        ShowStatistics(library);
                        break;
                    case "10":
                        ReserveBook(library);
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val.");
                        break;
                }
            }
        }

        static void ReserveBook(Library library)
        {
            // Reserverar en bok för angiven medlem.
            var memberId = ReadRequiredInput("Medlems-ID: ");
            var isbn = ReadRequiredInput("ISBN: ");

            var member = library.MemberRegistry.GetMemberById(memberId);
            var book = library.BookCatalog.GetAll().FirstOrDefault(b => b.ISBN == isbn);

            if (member == null)
            {
                Console.WriteLine("Medlemmen hittades inte.");
                return;
            }

            if (book == null)
            {
                Console.WriteLine("Boken hittades inte.");
                return;
            }

            try
            {
                library.LoanManager.ReserveBook(book, member);
                Console.WriteLine("Boken är reserverad.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void AddBook(Library library)
        {
            // Samlar in bokdata och lägger till i katalogen.
            var isbn = ReadRequiredInput("ISBN: ");
            var title = ReadRequiredInput("Titel: ");
            var author = ReadRequiredInput("Författare: ");
            var year = ReadIntInput("Utgivningsår: ");

            try
            {
                var book = new Book(isbn, title, author, year);
                library.BookCatalog.AddBook(book);
                Console.WriteLine("Boken är tillagd.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void AddMember(Library library)
        {
            // Registrerar en ny medlem i systemet.
            var memberId = ReadRequiredInput("Medlems-ID: ");
            var name = ReadRequiredInput("Namn: ");
            var email = ReadRequiredInput("E-post: ");

            try
            {
                var member = new Member(memberId, name, email);
                library.MemberRegistry.AddMember(member);
                Console.WriteLine("Medlemmen är tillagd.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void LoanBook(Library library)
        {
            // Skapar lån om både bok och medlem finns.
            var memberId = ReadRequiredInput("Medlems-ID: ");
            var isbn = ReadRequiredInput("ISBN: ");
            var days = ReadIntInput("Antal dagar för lån: ");

            var member = library.MemberRegistry.GetMemberById(memberId);
            var book = library.BookCatalog.GetAll().FirstOrDefault(b => b.ISBN == isbn);

            if (member == null)
            {
                Console.WriteLine("Medlemmen hittades inte.");
                return;
            }

            if (book == null)
            {
                Console.WriteLine("Boken hittades inte.");
                return;
            }

            if (!book.IsAvailable)
            {
                Console.WriteLine("Boken är redan utlånad.");
                return;
            }

            try
            {
                var loanDate = DateTime.Now;
                var dueDate = loanDate.AddDays(days);
                library.LoanManager.CreateLoan(book, member, loanDate, dueDate);
                Console.WriteLine("Boken är utlånad.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void ReturnBook(Library library)
        {
            // Återlämnar bok och visar eventuell förseningsavgift.
            var isbn = ReadRequiredInput("ISBN: ");

            var loan = library.LoanManager.Loans
                .FirstOrDefault(l => l.Book.ISBN == isbn && !l.IsReturned);

            if (loan == null)
            {
                Console.WriteLine("Aktivt lån hittades inte.");
                return;
            }

            try
            {
                library.LoanManager.ReturnBook(loan, DateTime.Now);
                var fee = loan.CalculateLateFee(DailyLateFee);
                Console.WriteLine("Boken är återlämnad.");
                if (fee > 0)
                {
                    Console.WriteLine($"Förseningsavgift: {fee} kr");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void SearchBooks(Library library)
        {
            // Söker böcker utifrån titel, författare eller ISBN.
            var term = ReadRequiredInput("Sökterm: ");
            var results = library.SearchBooks(term);
            ListBooks(results);
        }

        static void ListBooks(IEnumerable<Book> books)
        {
            // Skriver ut bokinformation eller meddelar om listan är tom.
            var any = false;
            foreach (var book in books)
            {
                any = true;
                Console.WriteLine(book.GetInfo());
            }

            if (!any)
            {
                Console.WriteLine("Inga böcker att visa.");
            }
        }

        static void ShowStatistics(Library library)
        {
            // Visar statistik över böcker och aktiva lån.
            Console.WriteLine($"Totalt antal böcker: {library.GetTotalBooks()}");
            Console.WriteLine($"Antal utlånade böcker: {library.GetBorrowedBooksCount()}");

            var mostActive = library.GetMostActiveBorrower();
            Console.WriteLine(mostActive == null
                ? "Ingen aktiv låntagare."
                : $"Mest aktiv låntagare: {mostActive.Name} ({mostActive.MemberId})");
        }

        static string ReadRequiredInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();

                Console.WriteLine("Fältet får inte vara tomt.");
            }
        }

        static int ReadIntInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out var value))
                    return value;

                Console.WriteLine("Ogiltigt nummer.");
            }
        }
    }
}
