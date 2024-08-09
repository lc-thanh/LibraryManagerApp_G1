using LibraryManagerApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagerApp.Data.Data
{
    public class LibraryManagerAppDbContext : DbContext
    {
        public LibraryManagerAppDbContext(DbContextOptions<LibraryManagerAppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .HasOne<Author>(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId);

            modelBuilder.Entity<Book>()
                .HasOne<Category>(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId);

            modelBuilder.Entity<Book>()
                .HasOne<BookShelf>(b => b.BookShelf)
                .WithMany(bs => bs.Books)
                .HasForeignKey(b => b.BookShelfId);

            modelBuilder.Entity<BookShelf>()
                .HasOne<Cabinet>(bs => bs.Cabinet)
                .WithMany(c => c.BookShelves)
                .HasForeignKey(bs => bs.CabinetId);

            modelBuilder.Entity<Loan>()
                .HasOne<Member>(l => l.Member)
                .WithMany(m => m.Loans)
                .HasForeignKey(l => l.MemberId);

            modelBuilder.Entity<LoanDetail>().HasKey(ld => new { ld.LoanId, ld.BookId });

            modelBuilder.Entity<LoanDetail>()
                .HasOne<Loan>(ld => ld.Loan)
                .WithMany(l => l.LoanDetails)
                .HasForeignKey(ld => ld.LoanId);

            modelBuilder.Entity<LoanDetail>()
                .HasOne<Book>(ld => ld.Book)
                .WithMany(b => b.LoanDetails)
                .HasForeignKey(ld => ld.BookId);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookShelf> BookShelves { get; set; }
        public DbSet<Cabinet> Cabinets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanDetail> LoanDetails { get; set; }
    }
}
