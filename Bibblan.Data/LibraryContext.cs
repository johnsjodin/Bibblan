using Bibblan.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bibblan.Data;

/// <summary>
/// Databaskontexten för bibliotekssystemet.
/// Hanterar anslutning till SQL Server LocalDB.
/// </summary>
public class LibraryContext : DbContext
{
    public DbSet<BookEntity> Books { get; set; }
    public DbSet<MemberEntity> Members { get; set; }
    public DbSet<LoanEntity> Loans { get; set; }

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    // Konstruktor för design-time och migrationer
    public LibraryContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Används endast om options inte redan är konfigurerade (t.ex. migrationer)
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=BibblanDb;Trusted_Connection=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Konfigurerar Book-entiteten
        modelBuilder.Entity<BookEntity>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.HasIndex(b => b.ISBN).IsUnique();
            entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
            entity.Property(b => b.Author).IsRequired().HasMaxLength(100);
            entity.Property(b => b.ISBN).IsRequired().HasMaxLength(20);

            // Relation till reserverande medlem
            entity.HasOne(b => b.ReservedBy)
                  .WithMany(m => m.ReservedBooks)
                  .HasForeignKey(b => b.ReservedByMemberId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Konfigurerar Member-entiteten
        modelBuilder.Entity<MemberEntity>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.HasIndex(m => m.MemberId).IsUnique();
            entity.HasIndex(m => m.Email).IsUnique();
            entity.Property(m => m.Name).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Email).IsRequired().HasMaxLength(150);
            entity.Property(m => m.MemberId).IsRequired().HasMaxLength(50);
        });

        // Konfigurerar Loan-entiteten med relationer
        modelBuilder.Entity<LoanEntity>(entity =>
        {
            entity.HasKey(l => l.Id);

            entity.HasOne(l => l.Book)
                  .WithMany(b => b.Loans)
                  .HasForeignKey(l => l.BookId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(l => l.Member)
                  .WithMany(m => m.Loans)
                  .HasForeignKey(l => l.MemberId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed-data för testning
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Exempeldata för böcker
        modelBuilder.Entity<BookEntity>().HasData(
            new BookEntity { Id = 1, ISBN = "978-91-0-012345-6", Title = "Pippi Långstrump", Author = "Astrid Lindgren", PublishedYear = 1945, IsAvailable = true },
            new BookEntity { Id = 2, ISBN = "978-91-0-054321-9", Title = "Män som hatar kvinnor", Author = "Stieg Larsson", PublishedYear = 2005, IsAvailable = true },
            new BookEntity { Id = 3, ISBN = "978-91-7-001234-5", Title = "En man som heter Ove", Author = "Fredrik Backman", PublishedYear = 2012, IsAvailable = true }
        );

        // Exempeldata för medlemmar
        modelBuilder.Entity<MemberEntity>().HasData(
            new MemberEntity { Id = 1, MemberId = "M001", Name = "Anna Andersson", Email = "anna@example.com", MemberSince = new DateTime(2023, 1, 15) },
            new MemberEntity { Id = 2, MemberId = "M002", Name = "Erik Eriksson", Email = "erik@example.com", MemberSince = new DateTime(2023, 6, 20) }
        );
    }
}
