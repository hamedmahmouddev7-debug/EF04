//using System;
//using System.Collections.Generic;

//namespace NationalBank;

//public enum CustomerType
//{
//    Individual = 1,
//    Business = 2
//}

//public enum AccountType
//{
//    Savings = 1,
//    Current = 2,
//    Business = 3
//}

//public enum OwnershipType
//{
//    Primary = 1,
//    CoHolder = 2
//}

//public enum AccountStatus
//{
//    Active = 1,
//    Closed = 2
//}

//public enum TransactionType
//{
//    Deposit = 1,
//    Withdrawal = 2,
//    Transfer = 3
//}

//public class Manager
//{
//    public int Id { get; set; }
//    public string FullName { get; set; } = string.Empty;
//    public string Email { get; set; } = string.Empty;
//    public string PhoneNumber { get; set; } = string.Empty;
//    public DateTime HireDate { get; set; }

//    public Branch? Branch { get; set; }
//}

//public class Branch
//{
//    public string Code { get; set; } = string.Empty;
//    public string Name { get; set; } = string.Empty;
//    public string Address { get; set; } = string.Empty;
//    public string PhoneNumber { get; set; } = string.Empty;

//    public int ManagerId { get; set; }
//    public Manager Manager { get; set; } = null!;

//    public ICollection<Account> Accounts { get; set; } = new List<Account>();
//}

//public class Customer
//{
//    public int Id { get; set; }
//    public string FullName { get; set; } = string.Empty;
//    public string NationalId { get; set; } = string.Empty;
//    public DateTime DateOfBirth { get; set; }
//    public string Email { get; set; } = string.Empty;
//    public string PhoneNumber { get; set; } = string.Empty;
//    public string Address { get; set; } = string.Empty;
//    public CustomerType CustomerType { get; set; }

//    public ICollection<CustomerAccount> CustomerAccounts { get; set; }
//        = new List<CustomerAccount>();
//}

//public class Account
//{
//    public string AccountNumber { get; set; } = string.Empty;
//    public decimal CurrentBalance { get; set; }
//    public AccountType AccountType { get; set; }
//    public DateTime OpeningDate { get; set; }

//    public string BranchCode { get; set; } = string.Empty;
//    public Branch Branch { get; set; } = null!;

//    public ICollection<CustomerAccount> CustomerAccounts { get; set; }
//        = new List<CustomerAccount>();

//    public ICollection<Transaction> Transactions { get; set; }
//        = new List<Transaction>();
//}

//// Many-to-Many relationship between Customer and Account
//public class CustomerAccount
//{
//    public int CustomerId { get; set; }
//    public Customer Customer { get; set; } = null!;

//    public string AccountNumber { get; set; } = string.Empty;
//    public Account Account { get; set; } = null!;

//    public OwnershipType OwnershipType { get; set; }
//    public DateTime OwnershipStartDate { get; set; }
//    public AccountStatus AccountStatus { get; set; }
//}

//public class Transaction
//{
//    public int TransactionNumber { get; set; }
//    public DateTime TransactionDate { get; set; }
//    public decimal Amount { get; set; }
//    public TransactionType TransactionType { get; set; }
//    public string? Note { get; set; }

//    public string AccountNumber { get; set; } = string.Empty;
//    public Account Account { get; set; } = null!;
//}