namespace Bibblan.Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var library = new Library(new BookCatalog(), new MemberRegistry(), new LoanManager());
            var running = true;

            while (running)
            {
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
                Console.WriteLine("0. Avsluta");
                Console.Write("Val: ");

                switch (Console.ReadLine())
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
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val.");
                        break;
                }
            }
        }

        static void AddBook(Library library)
        {
            Console.Write("ISBN: ");
            var isbn = Console.ReadLine();
            Console.Write("Titel: ");
            var title = Console.ReadLine();
            Console.Write("Författare: ");
            var author = Console.ReadLine();
            Console.Write("Utgivningsår: ");

            if (!int.TryParse(Console.ReadLine(), out var year))
            {
                Console.WriteLine("Ogiltigt år.");
                return;
            }

            try
            {
                var book = new Book(isbn ?? string.Empty, title ?? string.Empty, author ?? string.Empty, year);
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
            Console.Write("Medlems-ID: ");
            var memberId = Console.ReadLine();
            Console.Write("Namn: ");
            var name = Console.ReadLine();
            Console.Write("E-post: ");
            var email = Console.ReadLine();

            try
            {
                var member = new Member(memberId ?? string.Empty, name ?? string.Empty, email ?? string.Empty);
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
            Console.Write("Medlems-ID: ");
            var memberId = Console.ReadLine();
            Console.Write("ISBN: ");
            var isbn = Console.ReadLine();
            Console.Write("Antal dagar för lån: ");

            if (!int.TryParse(Console.ReadLine(), out var days))
            {
                Console.WriteLine("Ogiltigt antal dagar.");
                return;
            }

            var member = library.MemberRegistry.GetMemberById(memberId ?? string.Empty);
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
            Console.Write("ISBN: ");
            var isbn = Console.ReadLine();

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
                Console.WriteLine("Boken är återlämnad.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void SearchBooks(Library library)
        {
            Console.Write("Sökterm: ");
            var term = Console.ReadLine();
            var results = library.SearchBooks(term ?? string.Empty);
            ListBooks(results);
        }

        static void ListBooks(IEnumerable<Book> books)
        {
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
            Console.WriteLine($"Totalt antal böcker: {library.GetTotalBooks()}");
            Console.WriteLine($"Antal utlånade böcker: {library.GetBorrowedBooksCount()}");

            var mostActive = library.GetMostActiveBorrower();
            Console.WriteLine(mostActive == null
                ? "Ingen aktiv låntagare."
                : $"Mest aktiv låntagare: {mostActive.Name} ({mostActive.MemberId})");
        }
    }
}
