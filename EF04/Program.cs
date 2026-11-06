using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using NationalBank;

using var db = new BankDbContext();

db.Database.Migrate();

while (true)
{
    Console.Clear();
    PrintMenu();

    var choice = Console.ReadLine()?.Trim();

    if (choice == "0")
        break;

    Console.WriteLine();

    try
    {
        switch (choice)
        {
            case "1":
                AddCustomer(db);
                break;

            case "2":
                OpenAccount(db);
                break;

            case "3":
                UpdateAccountStatus(db);
                break;

            case "4":
                RemoveAccountFromCustomer(db);
                break;

            case "5":
                ListCustomers(db);
                break;

            default:
                Error("Invalid choice. Please pick a number from the menu.");
                break;
        }
    }
    catch (DbUpdateException ex)
    {
        db.ChangeTracker.Clear();

        Error("Database error: " +
              (ex.InnerException?.Message ?? ex.Message));
    }
    catch (Exception ex)
    {
        Error("Unexpected error: " + ex.Message);
    }

    Pause();
}

db.Database.CloseConnection();

Console.WriteLine("Goodbye!");

// =============================== Menu ===============================

static void PrintMenu()
{
    Console.WriteLine("========================================");
    Console.WriteLine("   National Bank - Management");
    Console.WriteLine("========================================");

    Console.WriteLine("  1) Add a new Customer");
    Console.WriteLine("  2) Open a new Account for a Customer");
    Console.WriteLine("  3) Update Account Status (Active / Closed)");
    Console.WriteLine("  4) Remove an Account from a Customer");
    Console.WriteLine("  5) List all Customers (with accounts)");
    Console.WriteLine("  0) Exit");

    Console.WriteLine("----------------------------------------");
    Console.Write("  Enter choice: ");
}

// =============================== 1) Add Customer ===============================

static void AddCustomer(BankDbContext db)
{
    Console.WriteLine("--- Add New Customer ---");

    var name = ReadString(
        "Full Name",
        v => v.Length >= 3
            ? null
            : "Name must be at least 3 characters."
    );

    var nationalId = ReadString(
        "National ID",
        v => v.All(char.IsDigit) && v.Length is >= 10 and <= 14
            ? null
            : "National ID must be 10-14 digits."
    );

    if (db.Customers.Any(c => c.NationalId == nationalId))
    {
        Error("A customer with this National ID already exists.");
        return;
    }

    var dob = ReadDate("Date of Birth (yyyy-MM-dd)");

    var email = ReadString(
        "Email",
        v => MailAddress.TryCreate(v, out _)
            ? null
            : "Invalid email format."
    );

    var phone = ReadString(
        "Phone",
        v => v.All(char.IsDigit) && v.Length is >= 8 and <= 15
            ? null
            : "Phone must be 8-15 digits."
    );

    var address = ReadString(
        "Address",
        v => v.Length >= 5
            ? null
            : "Address is too short."
    );

    var type = (CustomerType)ReadMenuChoice(
        "Customer Type",
        "Individual",
        "Business"
    );

    var customer = new Customer
    {
        FullName = name,
        NationalId = nationalId,
        DateOfBirth = dob,
        Email = email,
        PhoneNumber = phone,
        Address = address,
        CustomerType = type
    };

    db.Customers.Add(customer);
    db.SaveChanges();

    Success($"Customer created successfully. CustomerId = {customer.Id}");
}

// =============================== 2) Open Account ===============================

static void OpenAccount(BankDbContext db)
{
    Console.WriteLine("--- Open New Account ---");

    var accountNumber = ReadString(
        "Account Number",
        v => v.Length is >= 3 and <= 20 && !v.Contains(' ')
            ? null
            : "Account number must be 3-20 characters, no spaces."
    );

    var accountType = (AccountType)ReadMenuChoice(
        "Account Type",
        "Savings",
        "Current",
        "Business"
    );

    var branchCode = ReadString(
        "Branch Code",
        v => v.Length > 0
            ? null
            : "Branch code is required."
    ).ToUpper();

    var customerId = ReadInt("Customer Id");

    var role = (OwnershipType)ReadMenuChoice(
        "Ownership Role",
        "Primary",
        "CoHolder"
    );

    Info($"Validating branch '{branchCode}' and customer #{customerId}...");

    if (!db.Branches.Any(b => b.Code == branchCode))
    {
        Error($"Branch '{branchCode}' does not exist.");
        return;
    }

    if (!db.Customers.Any(c => c.Id == customerId))
    {
        Error($"Customer #{customerId} does not exist.");
        return;
    }

    var account = db.Accounts
        .Include(a => a.CustomerAccounts)
        .FirstOrDefault(a => a.AccountNumber == accountNumber);

    if (account is null)
    {
        if (role == OwnershipType.CoHolder)
        {
            Error("A new account must start with a Primary owner.");
            return;
        }

        account = new Account
        {
            AccountNumber = accountNumber,
            AccountType = accountType,
            BranchCode = branchCode,
            OpeningDate = DateTime.Today,
            CurrentBalance = 0m
        };

        db.Accounts.Add(account);
    }
    else
    {
        // Existing account means joint account

        if (account.BranchCode != branchCode)
        {
            Error($"Account '{accountNumber}' belongs to branch '{account.BranchCode}'.");
            return;
        }

        if (account.CustomerAccounts.Any(ca => ca.CustomerId == customerId))
        {
            Error("This customer is already linked to the account.");
            return;
        }

        if (role == OwnershipType.Primary &&
            account.CustomerAccounts.Any(ca => ca.OwnershipType == OwnershipType.Primary))
        {
            Error("This account already has a Primary owner. Use CoHolder.");
            return;
        }
    }

    db.CustomerAccounts.Add(new CustomerAccount
    {
        CustomerId = customerId,
        Account = account,
        OwnershipType = role,
        OwnershipStartDate = DateTime.Today,
        AccountStatus = AccountStatus.Active
    });

    db.SaveChanges();

    Success($"Account '{accountNumber}' created and linked to customer {customerId} as {role} owner.");
}

// =============================== 3) Update Status ===============================

static void UpdateAccountStatus(BankDbContext db)
{
    Console.WriteLine("--- Update Account Status ---");

    var accountNumber = ReadString(
        "Account Number",
        v => v.Length > 0
            ? null
            : "Account number is required."
    );

    var customerId = ReadInt("Customer Id");

    // Composite key order: CustomerId, AccountNumber
    var link = db.CustomerAccounts.Find(customerId, accountNumber);

    if (link is null)
    {
        Error("No ownership link found for this account and customer.");
        return;
    }

    var newStatus = (AccountStatus)ReadMenuChoice(
        "New Status",
        "Active",
        "Closed"
    );

    link.AccountStatus = newStatus;

    db.SaveChanges();

    Success($"Status updated to {newStatus}.");
}

// =============================== 4) Remove Account ===============================

static void RemoveAccountFromCustomer(BankDbContext db)
{
    Console.WriteLine("--- Remove Account From Customer ---");

    var accountNumber = ReadString(
        "Account Number",
        v => v.Length > 0
            ? null
            : "Account number is required."
    );

    var customerId = ReadInt("Customer Id");

    var link = db.CustomerAccounts.Find(customerId, accountNumber);

    if (link is null)
    {
        Error("No ownership link found for this account and customer.");
        return;
    }

    db.CustomerAccounts.Remove(link);
    db.SaveChanges();

    Success("Ownership link deleted.");

    // If this was the last owner, remove the account
    // only if it has no transaction history.

    if (!db.CustomerAccounts.Any(ca => ca.AccountNumber == accountNumber))
    {
        if (db.Transactions.Any(t => t.AccountNumber == accountNumber))
        {
            Info($"No owners left, but account '{accountNumber}' has transaction history, so it was kept.");
        }
        else
        {
            var account = db.Accounts.Find(accountNumber);

            if (account is not null)
            {
                db.Accounts.Remove(account);
                db.SaveChanges();

                Info($"That was the last owner - account '{accountNumber}' was also removed.");
            }
        }
    }
}

// =============================== 5) List Customers ===============================

static void ListCustomers(BankDbContext db)
{
    Console.WriteLine("--- All Customers ---\n");

    var customers = db.Customers
        .AsNoTracking()
        .Include(c => c.CustomerAccounts)
            .ThenInclude(ca => ca.Account)
                .ThenInclude(a => a.Branch)
        .OrderBy(c => c.Id)
        .ToList();

    if (customers.Count == 0)
    {
        Console.WriteLine("  (no customers)");
        return;
    }

    foreach (var c in customers)
    {
        Console.WriteLine($"  #{c.Id} {c.FullName} ({c.CustomerType})");

        if (c.CustomerAccounts.Count == 0)
        {
            Console.WriteLine("      (no accounts)");
            continue;
        }

        foreach (var ca in c.CustomerAccounts)
        {
            var a = ca.Account;

            Console.WriteLine(
                $"      {a.AccountNumber,-10} {a.AccountType,-9} Balance: {a.CurrentBalance,12:N2}   " +
                $"{ca.OwnershipType,-8} {ca.AccountStatus,-7} @ {a.Branch.Name}"
            );
        }
    }
}

// =============================== Input Helpers ===============================

static string ReadString(string label, Func<string, string?> validate)
{
    while (true)
    {
        Console.Write($"{label,-14}: ");

        var input = Console.ReadLine()?.Trim() ?? "";

        var error = validate(input);

        if (error is null)
            return input;

        Error(error);
    }
}

static int ReadInt(string label)
{
    while (true)
    {
        Console.Write($"{label,-14}: ");

        if (int.TryParse(Console.ReadLine(), out var value) && value > 0)
            return value;

        Error("Please enter a valid positive number.");
    }
}

static DateTime ReadDate(string label)
{
    while (true)
    {
        Console.Write($"{label}: ");

        if (DateTime.TryParseExact(
                Console.ReadLine()?.Trim(),
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var date)
            && date < DateTime.Today
            && date.Year >= 1900)
        {
            return date;
        }

        Error("Invalid date. Use yyyy-MM-dd and a date in the past.");
    }
}

// Returns 1..n according to the selected option

static int ReadMenuChoice(string label, params string[] options)
{
    Console.WriteLine($"{label}:");

    for (int i = 0; i < options.Length; i++)
    {
        Console.WriteLine($"    {i + 1}) {options[i]}");
    }

    while (true)
    {
        Console.Write("  Choice: ");

        if (int.TryParse(Console.ReadLine(), out var n) &&
            n >= 1 &&
            n <= options.Length)
        {
            return n;
        }

        Error($"Please enter a number between 1 and {options.Length}.");
    }
}

// =============================== Output Helpers ===============================

static void Success(string msg) =>
    Colored(msg, ConsoleColor.Green);

static void Info(string msg) =>
    Colored(msg, ConsoleColor.Cyan);

static void Error(string msg) =>
    Colored(msg, ConsoleColor.Red);

static void Colored(string msg, ConsoleColor color)
{
    var old = Console.ForegroundColor;

    Console.ForegroundColor = color;

    Console.WriteLine(msg);

    Console.ForegroundColor = old;
}

static void Pause()
{
    Console.WriteLine("\nPress any key to return to the menu...");
    Console.ReadKey(true);
}