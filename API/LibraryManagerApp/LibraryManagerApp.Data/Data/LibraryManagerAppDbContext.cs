using LibraryManagerApp.Data.Models;
using LibraryManagerApp.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagerApp.Data.Data
{
    public class LibraryManagerAppDbContext : DbContext
    {
        private readonly UserService _userService; // For hash password

        public LibraryManagerAppDbContext(DbContextOptions<LibraryManagerAppDbContext> options) : base(options)
        {
            _userService = new UserService();
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
        public DbSet<UserAction> UserActions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();


            modelBuilder.Entity<Book>()
                .HasOne<Author>(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .IsRequired(false);

            modelBuilder.Entity<Book>()
                .HasOne<Category>(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .IsRequired(false);

            modelBuilder.Entity<Book>()
                .HasOne<BookShelf>(b => b.BookShelf)
                .WithMany(bs => bs.Books)
                .HasForeignKey(b => b.BookShelfId)
                .IsRequired(false);

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

            modelBuilder.Entity<UserAction>()
                .HasOne<User>(ua => ua.User)
                .WithMany(u => u.UserActions)
                .HasForeignKey(ua => ua.UserId);


            // Seed data for Cabinets
            List<Cabinet> cabinets = new List<Cabinet>()
            {
                // 0
                new Cabinet() { Id = Guid.NewGuid(), Name = "Sách Chính trị - Xã hội", Location = "1-1", CreatedOn = new DateTime(2023, 05, 17) },
                // 1
                new Cabinet() { Id = Guid.NewGuid(), Name = "Sách Khoa học - Công nghệ", Location = "1-2", CreatedOn = new DateTime(2023, 05, 17) },
                // 2
                new Cabinet() { Id = Guid.NewGuid(), Name = "Văn học - Nghệ thuật", Location = "1-3", CreatedOn = new DateTime(2023, 05, 17) },
                // 3
                new Cabinet() { Id = Guid.NewGuid(), Name = "Sách Ngoại ngữ", Location = "2-1", CreatedOn = new DateTime(2023, 05, 17) },
                // 4
                new Cabinet() { Id = Guid.NewGuid(), Name = "SGK - Giáo trình", Location = "2-2", CreatedOn = new DateTime(2023, 08, 20) },
                // 5
                new Cabinet() { Id = Guid.NewGuid(), Name = "Sách Pháp luật", Location = "2-3", CreatedOn = new DateTime(2024, 01, 05) },
                // 6
                new Cabinet() { Id = Guid.NewGuid(), Name = "Truyện", Location = "3-1", CreatedOn = new DateTime(2024, 01, 06) },
                // 7
                new Cabinet() { Id = Guid.NewGuid(), Name = "Sách khác", Location = "3-2", CreatedOn = new DateTime(2023, 05, 17) },
            };
            modelBuilder.Entity<Cabinet>().HasData(cabinets);

            // Seed data for BookShelves
            List<BookShelf> bookShelfs = new List<BookShelf>()
            {
                // 0
                new BookShelf() { Id = Guid.NewGuid(), Name = "Chính trị - Xã hội 1", CabinetId = cabinets[0].Id, CreatedOn = new DateTime(2023, 05, 17) },
                // 1
                new BookShelf() { Id = Guid.NewGuid(), Name = "Chính trị - Xã hội 2", CabinetId = cabinets[0].Id, CreatedOn = new DateTime(2023, 05, 18) },
                // 2
                new BookShelf() { Id = Guid.NewGuid(), Name = "Khoa học - Công nghệ 1", CabinetId = cabinets[1].Id, CreatedOn = new DateTime(2023, 05, 17) },
                // 3
                new BookShelf() { Id = Guid.NewGuid(), Name = "Khoa học - Công nghệ 2", CabinetId = cabinets[1].Id, CreatedOn = new DateTime(2023, 05, 18) },
                // 4
                new BookShelf() { Id = Guid.NewGuid(), Name = "Văn học - Nghệ thuật 1", CabinetId = cabinets[2].Id, CreatedOn = new DateTime(2023, 05, 17) },
                // 5
                new BookShelf() { Id = Guid.NewGuid(), Name = "Tiếng Anh", CabinetId = cabinets[3].Id, CreatedOn = new DateTime(2023, 05, 17) },
                // 6
                new BookShelf() { Id = Guid.NewGuid(), Name = "Tiếng Trung", CabinetId = cabinets[3].Id, CreatedOn = new DateTime(2023, 05, 19) },
                // 7
                new BookShelf() { Id = Guid.NewGuid(), Name = "SGK", CabinetId = cabinets[4].Id, CreatedOn = new DateTime(2023, 08, 20) },
                // 8
                new BookShelf() { Id = Guid.NewGuid(), Name = "Giáo trình", CabinetId = cabinets[4].Id, CreatedOn = new DateTime(2023, 08, 21) },
                // 9
                new BookShelf() { Id = Guid.NewGuid(), Name = "Pháp luật 1", CabinetId = cabinets[5].Id, CreatedOn = new DateTime(2024, 01, 15) },
                // 10
                new BookShelf() { Id = Guid.NewGuid(), Name = "Truyện tranh", CabinetId = cabinets[6].Id, CreatedOn = new DateTime(2024, 01, 15) },
                // 11
                new BookShelf() { Id = Guid.NewGuid(), Name = "Tiểu thuyết", CabinetId = cabinets[6].Id, CreatedOn = new DateTime(2024, 01, 15) },
                // 12
                new BookShelf() { Id = Guid.NewGuid(), Name = "Sách khác 1", CabinetId = cabinets[7].Id, CreatedOn = new DateTime(2023, 05, 17) },
                // 13
                new BookShelf() { Id = Guid.NewGuid(), Name = "Sách khác 2", CabinetId = cabinets[7].Id, CreatedOn = new DateTime(2023, 05, 17) },
            };
            modelBuilder.Entity<BookShelf>().HasData(bookShelfs);

            // Seed data for Authors
            List<Author> authors = new List<Author>()
            {
                // 0
                new Author() { Id = Guid.NewGuid(), Name = "Hồ Chí Minh", CreatedOn = new DateTime(2023, 05, 17) },
                // 1
                new Author() { Id = Guid.NewGuid(), Name = "Phùng Quán", CreatedOn = new DateTime(2023, 05, 17) },
                // 2
                new Author() { Id = Guid.NewGuid(), Name = "Mark Raskino - Graham Waller", CreatedOn = new DateTime(2023, 05, 17) },
                // 3
                new Author() { Id = Guid.NewGuid(), Name = "Steven Levy", CreatedOn = new DateTime(2023, 08, 21) },
                // 4
                new Author() { Id = Guid.NewGuid(), Name = "Quốc hội nước CHXHCNVN", CreatedOn = new DateTime(2024, 01, 15) },
                // 5
                new Author() { Id = Guid.NewGuid(), Name = "Đào Hải", CreatedOn = new DateTime(2024, 01, 15) },
                // 6
                new Author() { Id = Guid.NewGuid(), Name = "Đoàn Hữu Nam", CreatedOn = new DateTime(2024, 01, 15) },
            };
            modelBuilder.Entity<Author>().HasData(authors);

            // Seed data for Categories
            List<Category> categories = new List<Category>()
            {
                new Category() { Id = Guid.NewGuid(), Name = "Sách, Chuyên khảo, Tuyển tập", CreatedOn = new DateTime(2023, 05, 17) },
                new Category() { Id = Guid.NewGuid(), Name = "Ấn phẩm định kỳ", CreatedOn = new DateTime(2023, 05, 17) },
                new Category() { Id = Guid.NewGuid(), Name = "Bài trích", CreatedOn = new DateTime(2023, 05, 17) },
            };
            modelBuilder.Entity<Category>().HasData(categories);

            // Seed data for Books
            List<Book> books = new List<Book>()
            {
                new Book() // 0
                { 
                    Id = Guid.NewGuid(), 
                    Title = "Đường Kách mệnh", 
                    Publisher = "Nhà xuất bản Trẻ", 
                    PublishedYear = 2020, 
                    Quantity = 10,
                    AvailableQuantity = 10,
                    TotalPages = 144,
                    ImageUrl = "duong_kack_menh.jpg",
                    Description = "Đường cách mệnh có giá trị thực tiễn lớn lao, tạo ra sự chuyển biến căn bản, nhanh chóng trong nhận thức và hành động cách mạng của cán bộ và đông đảo quần chúng, thể hiện rõ quan điểm tư tưởng của Nguyễn Ái Quốc trong thời kỳ chuẩn bị về chính trị, tư tưởng và tổ chức tiến tới việc thành lập Đảng Cộng sản Việt Nam, là một trong những văn kiện lý luận đầu tiên của Đảng ta, đặt cơ sở tư tưởng cho đường lối của cách mạng Việt Nam.",
                    CreatedOn = new DateTime(2023, 05, 17),
                    AuthorId = authors[0].Id,
                    CategoryId = categories[0].Id,
                    BookShelfId = bookShelfs[0].Id,
                },
                new Book() // 1
                {
                    Id = Guid.NewGuid(),
                    Title = "Đạo đức cách mạng",
                    Publisher = null,
                    PublishedYear = null,
                    Quantity = 5,
                    AvailableQuantity = 5,
                    TotalPages = 20,
                    ImageUrl = "dao_duc_cm.png",
                    Description = null,
                    CreatedOn = new DateTime(2023, 05, 17),
                    AuthorId = authors[0].Id,
                    CategoryId = categories[0].Id,
                    BookShelfId = bookShelfs[0].Id,
                },
                new Book() // 2
                {
                    Id = Guid.NewGuid(),
                    Title = "Văn hóa nghệ thuật cũng là một mặt trận",
                    Publisher = "Nhà xuất bản Văn hóa - Văn nghệ",
                    PublishedYear = 2020,
                    Quantity = 7,
                    AvailableQuantity = 7,
                    TotalPages = 576,
                    ImageUrl = "vhnt_cung_la_mot_mt.png",
                    Description = null,
                    CreatedOn = new DateTime(2023, 05, 18),
                    AuthorId = authors[0].Id,
                    CategoryId = categories[0].Id,
                    BookShelfId = bookShelfs[1].Id,
                },
                new Book() // 3
                {
                    Id = Guid.NewGuid(),
                    Title = "Tuổi thơ dữ dội - Tập 2",
                    Publisher = "Nhà xuất bản Kim đồng",
                    PublishedYear = 2022,
                    Quantity = 7,
                    AvailableQuantity = 7,
                    TotalPages = 400,
                    ImageUrl = "tuoi_tho_du_doi.jpeg",
                    Description = "\"Tuổi Thơ Dữ Dội\" không phải chỉ là một câu chuyện cổ tích, mà là một câu chuyện có thật ở trần gian, ở đó, những con người tuổi nhỏ đã tham gia vào cuộc kháng chiến chống xâm lược bảo vệ Tổ quốc với một chuỗi những chiến công đầy ắp li kì và hấp dẫn.",
                    CreatedOn = new DateTime(2023, 05, 18),
                    AuthorId = authors[1].Id,
                    CategoryId = categories[0].Id,
                    BookShelfId = bookShelfs[4].Id,
                },
                new Book() // 4
                {
                    Id = Guid.NewGuid(),
                    Title = "Tuổi thơ dữ dội - Tập 1",
                    Publisher = "Nhà xuất bản Kim đồng",
                    PublishedYear = 2021,
                    Quantity = 8,
                    AvailableQuantity = 8,
                    TotalPages = 404,
                    ImageUrl = "tuoi-tho-du-doi-t1.webp",
                    Description = "“Tuổi Thơ Dữ Dội” là một câu chuyện hay, cảm động viết về tuổi thơ. Sách dày 404 trang mà người đọc không bao giờ muốn ngừng lại, bị lôi cuốn vì những nhân vật ngây thơ có, khôn ranh có, anh hùng có, vì những sự việc khi thì ly kỳ, khi thì hài hước, khi thì gây xúc động đến ứa nước mắt...",
                    CreatedOn = new DateTime(2023, 05, 17),
                    AuthorId = authors[1].Id,
                    CategoryId = categories[0].Id,
                    BookShelfId = bookShelfs[4].Id,
                },
                new Book() // 5
                {
                    Id = Guid.NewGuid(),
                    Title = "Chuyển đổi số đến cốt lõi",
                    Publisher = "NXB Thông tin và Truyền thông",
                    PublishedYear = 2020,
                    Quantity = 3,
                    AvailableQuantity = 3,
                    TotalPages = 312,
                    ImageUrl = "khoa_hoc_1.jpeg",
                    Description = null,
                    CreatedOn = new DateTime(2023, 05, 17),
                    AuthorId = authors[2].Id,
                    CategoryId = categories[1].Id,
                    BookShelfId = bookShelfs[2].Id,
                },
                new Book() // 6
                {
                    Id = Guid.NewGuid(),
                    Title = "360 động từ bất quy tắc và 12 thì cơ bản trong tiếng Anh",
                    Publisher = "NXB Hồng Đức",
                    PublishedYear = 2020,
                    Quantity = 11,
                    AvailableQuantity = 11,
                    TotalPages = 92,
                    ImageUrl = "tieng_anh.jpeg",
                    Description = null,
                    CreatedOn = new DateTime(2023, 05, 17),
                    AuthorId = null,
                    CategoryId = categories[0].Id,
                    BookShelfId = bookShelfs[5].Id,
                },
                new Book() // 7
                {
                    Id = Guid.NewGuid(),
                    Title = "Tập viết chữ Hán - phiên bản mới 2018",
                    Publisher = "NXB Hồng Đức",
                    PublishedYear = 2020,
                    Quantity = 5,
                    AvailableQuantity = 5,
                    TotalPages = 120,
                    ImageUrl = "chu_han.png",
                    Description = null,
                    CreatedOn = new DateTime(2023, 05, 19),
                    AuthorId = null,
                    CategoryId = categories[0].Id,
                    BookShelfId = bookShelfs[6].Id,
                },
                new Book() // 8
                {
                    Id = Guid.NewGuid(),
                    Title = "SGK Tiếng Việt Lớp 1 - Tập một",
                    Publisher = "Nhà xuất bản Giáo dục",
                    PublishedYear = 2003,
                    Quantity = 15,
                    AvailableQuantity = 15,
                    TotalPages = 172,
                    ImageUrl = "SGK_1.jpg",
                    Description = null,
                    CreatedOn = new DateTime(2023, 08, 20),
                    AuthorId = null,
                    CategoryId = categories[0].Id,
                    BookShelfId = bookShelfs[7].Id,
                },
                new Book() // 9
                {
                    Id = Guid.NewGuid(),
                    Title = "Hacker lược sử",
                    Publisher = "NXB Công Thương",
                    PublishedYear = 2019,
                    Quantity = 3,
                    AvailableQuantity = 3,
                    TotalPages = 640,
                    ImageUrl = "hacker.jpeg",
                    Description = "Hacker lược sử nói về những nhân vật, cỗ máy, sự kiện định hình cho văn hóa và đạo đức hacker từ những hacker đời đầu ở đại học MIT. Câu chuyện hấp dẫn bắt đầu từ các phòng thí nghiệm nghiên cứu máy tính đầu tiên đến các máy tính gia đình.",
                    CreatedOn = new DateTime(2023, 08, 21),
                    AuthorId = authors[3].Id,
                    CategoryId = categories[2].Id,
                    BookShelfId = bookShelfs[8].Id,
                },
                new Book() // 10
                {
                    Id = Guid.NewGuid(),
                    Title = "Luật an ninh mạng",
                    Publisher = "NXB Công An Nhân Dân",
                    PublishedYear = 2019,
                    Quantity = 3,
                    AvailableQuantity = 3,
                    TotalPages = 64,
                    ImageUrl = "phap_luat.jpeg",
                    Description = "Luật An ninh mạng được Quốc hội thông qua ngày 12/6/2018. Luật gồm 7 Chương, 43 Điều.",
                    CreatedOn = new DateTime(2024, 01, 15),
                    AuthorId = authors[4].Id,
                    CategoryId = categories[1].Id,
                    BookShelfId = bookShelfs[9].Id,
                },
                new Book() // 11
                {
                    Id = Guid.NewGuid(),
                    Title = "Tý Quậy - Tập 11",
                    Publisher = "NXB Kim Đồng",
                    PublishedYear = 2015,
                    Quantity = 10,
                    AvailableQuantity = 10,
                    TotalPages = 100,
                    ImageUrl = "ty_quay.jpeg",
                    Description = "“Tý quậy là một phần tuổi thơ tôi, của bạn bè tôi. Không có ý mong Tý trở thành nhân vật điển hình, tôi chỉ ước sao Tý quậy là một người bạn gần gũi, quen thuộc và sống đúng nghĩa tuổi thơ.Thật không khôn ngoan khi dạy trẻ phải suy nghĩ những gì, mà nên hướng cho tuổi thơ cách suy nghĩ. Một đứa trẻ không hoạt bát thì tuyệt đối sẽ không trở thành người thông minh.Có câu danh ngôn rằng: việc độc nhất vô nhị và không có gì thay thế, đó là hồi ức về tuổi thơ. Vậy khi làm sách Tý quậy, bên cạnh tình yêu thương trong tôi có cả sự trân trọng...” (Tác giả Đào Hải)",
                    CreatedOn = new DateTime(2024, 01, 15),
                    AuthorId = authors[5].Id,
                    CategoryId = categories[0].Id,
                    BookShelfId = bookShelfs[10].Id,
                },
                new Book() // 12
                {
                    Id = Guid.NewGuid(),
                    Title = "GIỮA VÒNG VÂY NÚI - Tiểu thuyết",
                    Publisher = "NXB Văn hóa dân tộc",
                    PublishedYear = 2023,
                    Quantity = 6,
                    AvailableQuantity = 6,
                    TotalPages = 292,
                    ImageUrl = "tieu_thuyet.jpeg",
                    Description = "Cuốn sách viết về những mâu thuẫn giữa họ Sầm và họ Hồ, con đường đưa Sầm Lay đến với cách mạng và quá trình Sầm Lay cùng bộ đội Cụ Hồ đưa ánh sáng cách mạng về giải phóng quê hương Suối Hoa của anh.",
                    CreatedOn = new DateTime(2024, 01, 15),
                    AuthorId = authors[6].Id,
                    CategoryId = categories[0].Id,
                    BookShelfId = bookShelfs[11].Id,
                },
                new Book() // 13
                {
                    Id = Guid.NewGuid(),
                    Title = "Thần số học: Sức mạnh của những con số",
                    Publisher = "NXB Công thương",
                    PublishedYear = 2022,
                    Quantity = 2,
                    AvailableQuantity = 2,
                    TotalPages = 348,
                    ImageUrl = "than_so_hoc.jpeg",
                    Description = "Thần số học bắt đầu phổ biến mạnh tại Việt Nam từ đầu năm 2020 trùng với thời điểm dịch Covid bắt đầu xâm nhập (chắc hẳn là có lý do của nó) và kéo dài cho đến ngày hôm nay. Quyển sách này dành cho tất cả những ai quan tâm và muốn nghiên cứu bài bản Thần số học nhằm mục đích ứng dụng hiệu quả.",
                    CreatedOn = new DateTime(2023, 05, 17),
                    AuthorId = null,
                    CategoryId = categories[1].Id,
                    BookShelfId = bookShelfs[12].Id,
                },
            };
            modelBuilder.Entity<Book>().HasData(books);

            List<Admin> admins = new List<Admin>()
            {
                new Admin()
                {
                    Id = Guid.NewGuid(),
                    FullName = "Lê Công Thành",
                    Email = "lcthanh.htvn@gmail.com",
                    Phone = "0834361811",
                    Address = "Hà Tĩnh",
                    DateOfBirth = new DateTime(2003, 02, 02),
                    Role = Enum.RoleEnum.Admin,
                    Password = _userService.HashPassword("Abc123!@#"),
                }
            };
            modelBuilder.Entity<Admin>().HasData(admins);

            List<Librarian> librarians = new List<Librarian>()
            {
                new Librarian()
                {
                    Id = Guid.NewGuid(),
                    FullName = "Nguyễn Văn A",
                    Email = "nguyenvana@ex.com",
                    Address = "Hà Nội",
                    DateOfBirth = new DateTime(2004, 07, 08),
                    Role = Enum.RoleEnum.Librarian,
                    Password = _userService.HashPassword("Abc123!@#")
                }
            };
            modelBuilder.Entity<Librarian>().HasData(librarians);

            List<Member> members = new List<Member>()
            {
                new Member()
                {
                    Id = Guid.NewGuid(),
                    FullName = "Lê Thị B",
                    Email = "lethib@ex.com",
                    Address = "Hải Dương",
                    DateOfBirth = new DateTime(2002, 11, 18),
                    Role = Enum.RoleEnum.Member,
                    Password = _userService.HashPassword("Abc123!@#")
                },
                new Member()
                {
                    Id = Guid.NewGuid(),
                    FullName = "Trần Văn C",
                    Email = "tranvanc@ex.com",
                    Address = "Quảng Ninh",
                    DateOfBirth = new DateTime(2004, 10, 05),
                    Role = Enum.RoleEnum.Member,
                    Password = _userService.HashPassword("Abc123!@#")
                }
            };
            modelBuilder.Entity<Member>().HasData(members);

            base.OnModelCreating(modelBuilder);
        }
    }
}
