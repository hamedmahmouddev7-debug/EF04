//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

//namespace EF04.Migrations
//{
//    /// <inheritdoc />
//    public partial class InitialCreate : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.CreateTable(
//                name: "Customers",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
//                    NationalId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
//                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
//                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
//                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
//                    CustomerType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Customers", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Managers",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
//                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
//                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
//                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Managers", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Branches",
//                columns: table => new
//                {
//                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
//                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
//                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
//                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
//                    ManagerId = table.Column<int>(type: "int", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Branches", x => x.Code);
//                    table.ForeignKey(
//                        name: "FK_Branches_Managers_ManagerId",
//                        column: x => x.ManagerId,
//                        principalTable: "Managers",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Restrict);
//                });

//            migrationBuilder.CreateTable(
//                name: "Accounts",
//                columns: table => new
//                {
//                    AccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
//                    CurrentBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
//                    AccountType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
//                    OpeningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    BranchCode = table.Column<string>(type: "nvarchar(10)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Accounts", x => x.AccountNumber);
//                    table.ForeignKey(
//                        name: "FK_Accounts_Branches_BranchCode",
//                        column: x => x.BranchCode,
//                        principalTable: "Branches",
//                        principalColumn: "Code",
//                        onDelete: ReferentialAction.Restrict);
//                });

//            migrationBuilder.CreateTable(
//                name: "CustomerAccounts",
//                columns: table => new
//                {
//                    CustomerId = table.Column<int>(type: "int", nullable: false),
//                    AccountNumber = table.Column<string>(type: "nvarchar(20)", nullable: false),
//                    OwnershipType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
//                    OwnershipStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    AccountStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_CustomerAccounts", x => new { x.CustomerId, x.AccountNumber });
//                    table.ForeignKey(
//                        name: "FK_CustomerAccounts_Accounts_AccountNumber",
//                        column: x => x.AccountNumber,
//                        principalTable: "Accounts",
//                        principalColumn: "AccountNumber",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_CustomerAccounts_Customers_CustomerId",
//                        column: x => x.CustomerId,
//                        principalTable: "Customers",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "Transactions",
//                columns: table => new
//                {
//                    TransactionNumber = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
//                    TransactionType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
//                    Note = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
//                    AccountNumber = table.Column<string>(type: "nvarchar(20)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Transactions", x => x.TransactionNumber);
//                    table.ForeignKey(
//                        name: "FK_Transactions_Accounts_AccountNumber",
//                        column: x => x.AccountNumber,
//                        principalTable: "Accounts",
//                        principalColumn: "AccountNumber",
//                        onDelete: ReferentialAction.Restrict);
//                });

//            migrationBuilder.InsertData(
//                table: "Customers",
//                columns: new[] { "Id", "Address", "CustomerType", "DateOfBirth", "Email", "FullName", "NationalId", "PhoneNumber" },
//                values: new object[,]
//                {
//                    { 1, "Cairo", "Individual", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ahmed@mail.com", "Ahmed Ali", "29001011234567", "01011111111" },
//                    { 2, "Alexandria", "Business", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "info@nile.com", "Nile Trading LLC", "30001019876543", "01022222222" }
//                });

//            migrationBuilder.InsertData(
//                table: "Managers",
//                columns: new[] { "Id", "Email", "FullName", "HireDate", "PhoneNumber" },
//                values: new object[,]
//                {
//                    { 1, "khaled.m@nbg.com", "Khaled Mansour", new DateTime(2015, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0100000001" },
//                    { 2, "laila.f@nbg.com", "Laila Fathy", new DateTime(2018, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "0100000002" }
//                });

//            migrationBuilder.InsertData(
//                table: "Branches",
//                columns: new[] { "Code", "Address", "ManagerId", "Name", "PhoneNumber" },
//                values: new object[,]
//                {
//                    { "ALX-01", "5 Corniche Rd, Alexandria", 2, "Alexandria Branch", "0335000002" },
//                    { "CAI-01", "1 Tahrir Sq, Cairo", 1, "Cairo Main Branch", "0225000001" }
//                });

//            migrationBuilder.InsertData(
//                table: "Accounts",
//                columns: new[] { "AccountNumber", "AccountType", "BranchCode", "CurrentBalance", "OpeningDate" },
//                values: new object[,]
//                {
//                    { "2001-CUR", "Current", "CAI-01", 15400m, new DateTime(2022, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
//                    { "3000-BUS", "Business", "ALX-01", 120000m, new DateTime(2022, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
//                });

//            migrationBuilder.InsertData(
//                table: "CustomerAccounts",
//                columns: new[] { "AccountNumber", "CustomerId", "AccountStatus", "OwnershipStartDate", "OwnershipType" },
//                values: new object[,]
//                {
//                    { "2001-CUR", 1, "Active", new DateTime(2022, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Primary" },
//                    { "3000-BUS", 2, "Active", new DateTime(2022, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Primary" }
//                });

//            migrationBuilder.CreateIndex(
//                name: "IX_Accounts_BranchCode",
//                table: "Accounts",
//                column: "BranchCode");

//            migrationBuilder.CreateIndex(
//                name: "IX_Branches_ManagerId",
//                table: "Branches",
//                column: "ManagerId",
//                unique: true);

//            migrationBuilder.CreateIndex(
//                name: "IX_CustomerAccounts_AccountNumber",
//                table: "CustomerAccounts",
//                column: "AccountNumber");

//            migrationBuilder.CreateIndex(
//                name: "IX_Customers_NationalId",
//                table: "Customers",
//                column: "NationalId",
//                unique: true);

//            migrationBuilder.CreateIndex(
//                name: "IX_Managers_Email",
//                table: "Managers",
//                column: "Email",
//                unique: true);

//            migrationBuilder.CreateIndex(
//                name: "IX_Transactions_AccountNumber",
//                table: "Transactions",
//                column: "AccountNumber");
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "CustomerAccounts");

//            migrationBuilder.DropTable(
//                name: "Transactions");

//            migrationBuilder.DropTable(
//                name: "Customers");

//            migrationBuilder.DropTable(
//                name: "Accounts");

//            migrationBuilder.DropTable(
//                name: "Branches");

//            migrationBuilder.DropTable(
//                name: "Managers");
//        }
//    }
//}
