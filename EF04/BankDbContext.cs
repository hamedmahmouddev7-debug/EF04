//using Microsoft.EntityFrameworkCore;
//using System.Reflection.Emit;

//namespace NationalBank;

//public class BankDbContext : DbContext
//{
//    public DbSet<Branch> Branches => Set<Branch>();
//    public DbSet<Manager> Managers => Set<Manager>();
//    public DbSet<Customer> Customers => Set<Customer>();
//    public DbSet<Account> Accounts => Set<Account>();
//    public DbSet<CustomerAccount> CustomerAccounts => Set<CustomerAccount>();
//    public DbSet<Transaction> Transactions => Set<Transaction>();

//    protected override void OnConfiguring(DbContextOptionsBuilder options)
//    {
//        options.UseSqlServer(
//            "Server=.;Database=NationalBankDb;Trusted_Connection=True;TrustServerCertificate=True"
//        );
//    }

//    protected override void OnModelCreating(ModelBuilder mb)
//    {
//        // ---------- Manager ----------
//        mb.Entity<Manager>(e =>
//        {
//            e.HasKey(m => m.Id);

//            e.Property(m => m.FullName)
//                .IsRequired()
//                .HasMaxLength(150);

//            e.Property(m => m.Email)
//                .IsRequired()
//                .HasMaxLength(200);

//            e.Property(m => m.PhoneNumber)
//                .IsRequired()
//                .HasMaxLength(20);

//            e.Property(m => m.HireDate)
//                .IsRequired();

//            e.HasIndex(m => m.Email)
//                .IsUnique();
//        });

//        // ---------- Branch (1:1 with Manager) ----------
//        mb.Entity<Branch>(e =>
//        {
//            e.HasKey(b => b.Code);

//            e.Property(b => b.Code)
//                .HasMaxLength(10);

//            e.Property(b => b.Name)
//                .IsRequired()
//                .HasMaxLength(100);

//            e.Property(b => b.Address)
//                .IsRequired()
//                .HasMaxLength(250);

//            e.Property(b => b.PhoneNumber)
//                .IsRequired()
//                .HasMaxLength(20);

//            e.HasOne(b => b.Manager)
//                .WithOne(m => m.Branch)
//                .HasForeignKey<Branch>(b => b.ManagerId)
//                .IsRequired()
//                .OnDelete(DeleteBehavior.Restrict);
//        });

//        // ---------- Customer ----------
//        mb.Entity<Customer>(e =>
//        {
//            e.HasKey(c => c.Id);

//            e.Property(c => c.FullName)
//                .IsRequired()
//                .HasMaxLength(150);

//            e.Property(c => c.NationalId)
//                .IsRequired()
//                .HasMaxLength(20);

//            e.Property(c => c.Email)
//                .IsRequired()
//                .HasMaxLength(200);

//            e.Property(c => c.PhoneNumber)
//                .IsRequired()
//                .HasMaxLength(20);

//            e.Property(c => c.Address)
//                .IsRequired()
//                .HasMaxLength(250);

//            e.Property(c => c.DateOfBirth)
//                .IsRequired();

//            e.Property(c => c.CustomerType)
//                .HasConversion<string>()
//                .HasMaxLength(20);

//            e.HasIndex(c => c.NationalId)
//                .IsUnique();
//        });

//        // ---------- Account (Many-to-One with Branch) ----------
//        mb.Entity<Account>(e =>
//        {
//            e.HasKey(a => a.AccountNumber);

//            e.Property(a => a.AccountNumber)
//                .HasMaxLength(20);

//            e.Property(a => a.CurrentBalance)
//                .HasPrecision(18, 2);

//            e.Property(a => a.AccountType)
//                .HasConversion<string>()
//                .HasMaxLength(20);

//            e.Property(a => a.OpeningDate)
//                .IsRequired();

//            e.HasOne(a => a.Branch)
//                .WithMany(b => b.Accounts)
//                .HasForeignKey(a => a.BranchCode)
//                .IsRequired()
//                .OnDelete(DeleteBehavior.Restrict);
//        });

//        // ---------- CustomerAccount (Many-to-Many) ----------
//        mb.Entity<CustomerAccount>(e =>
//        {
//            e.HasKey(ca => new
//            {
//                ca.CustomerId,
//                ca.AccountNumber
//            });

//            e.Property(ca => ca.OwnershipType)
//                .HasConversion<string>()
//                .HasMaxLength(20);

//            e.Property(ca => ca.AccountStatus)
//                .HasConversion<string>()
//                .HasMaxLength(20);

//            e.Property(ca => ca.OwnershipStartDate)
//                .IsRequired();

//            e.HasOne(ca => ca.Customer)
//                .WithMany(c => c.CustomerAccounts)
//                .HasForeignKey(ca => ca.CustomerId)
//                .OnDelete(DeleteBehavior.Cascade);

//            e.HasOne(ca => ca.Account)
//                .WithMany(a => a.CustomerAccounts)
//                .HasForeignKey(ca => ca.AccountNumber)
//                .OnDelete(DeleteBehavior.Cascade);
//        });

//        // ---------- Transaction (Many-to-One with Account) ----------
//        mb.Entity<Transaction>(e =>
//        {
//            e.HasKey(t => t.TransactionNumber);

//            e.Property(t => t.Amount)
//                .HasPrecision(18, 2);

//            e.Property(t => t.TransactionDate)
//                .IsRequired();

//            e.Property(t => t.TransactionType)
//                .HasConversion<string>()
//                .HasMaxLength(20);

//            e.Property(t => t.Note)
//                .HasMaxLength(250);

//            e.HasOne(t => t.Account)
//                .WithMany(a => a.Transactions)
//                .HasForeignKey(t => t.AccountNumber)
//                .IsRequired()
//                .OnDelete(DeleteBehavior.Restrict);
//        });

//        // ================= SEEDING =================

//        // ---------- Managers ----------
//        mb.Entity<Manager>().HasData(
//            new Manager
//            {
//                Id = 1,
//                FullName = "Khaled Mansour",
//                Email = "khaled.m@nbg.com",
//                PhoneNumber = "0100000001",
//                HireDate = new DateTime(2015, 3, 1)
//            },
//            new Manager
//            {
//                Id = 2,
//                FullName = "Laila Fathy",
//                Email = "laila.f@nbg.com",
//                PhoneNumber = "0100000002",
//                HireDate = new DateTime(2018, 9, 15)
//            }
//        );

//        // ---------- Branches ----------
//        mb.Entity<Branch>().HasData(
//            new Branch
//            {
//                Code = "CAI-01",
//                Name = "Cairo Main Branch",
//                Address = "1 Tahrir Sq, Cairo",
//                PhoneNumber = "0225000001",
//                ManagerId = 1
//            },
//            new Branch
//            {
//                Code = "ALX-01",
//                Name = "Alexandria Branch",
//                Address = "5 Corniche Rd, Alexandria",
//                PhoneNumber = "0335000002",
//                ManagerId = 2
//            }
//        );

//        // ---------- Customers ----------
//        mb.Entity<Customer>().HasData(
//            new Customer
//            {
//                Id = 1,
//                FullName = "Ahmed Ali",
//                NationalId = "29001011234567",
//                DateOfBirth = new DateTime(1990, 1, 1),
//                Email = "ahmed@mail.com",
//                PhoneNumber = "01011111111",
//                Address = "Cairo",
//                CustomerType = CustomerType.Individual
//            },
//            new Customer
//            {
//                Id = 2,
//                FullName = "Nile Trading LLC",
//                NationalId = "30001019876543",
//                DateOfBirth = new DateTime(2000, 1, 1),
//                Email = "info@nile.com",
//                PhoneNumber = "01022222222",
//                Address = "Alexandria",
//                CustomerType = CustomerType.Business
//            }
//        );

//        // ---------- Accounts ----------
//        mb.Entity<Account>().HasData(
//            new Account
//            {
//                AccountNumber = "2001-CUR",
//                CurrentBalance = 15400m,
//                AccountType = AccountType.Current,
//                OpeningDate = new DateTime(2022, 1, 10),
//                BranchCode = "CAI-01"
//            },
//            new Account
//            {
//                AccountNumber = "3000-BUS",
//                CurrentBalance = 120000m,
//                AccountType = AccountType.Business,
//                OpeningDate = new DateTime(2022, 6, 1),
//                BranchCode = "ALX-01"
//            }
//        );

//        // ---------- Customer Accounts ----------
//        mb.Entity<CustomerAccount>().HasData(
//            new CustomerAccount
//            {
//                CustomerId = 1,
//                AccountNumber = "2001-CUR",
//                OwnershipType = OwnershipType.Primary,
//                OwnershipStartDate = new DateTime(2022, 1, 10),
//                AccountStatus = AccountStatus.Active
//            },
//            new CustomerAccount
//            {
//                CustomerId = 2,
//                AccountNumber = "3000-BUS",
//                OwnershipType = OwnershipType.Primary,
//                OwnershipStartDate = new DateTime(2022, 6, 1),
//                AccountStatus = AccountStatus.Active
//            }
//        );
//    }
//}