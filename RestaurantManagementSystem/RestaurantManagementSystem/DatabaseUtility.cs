using System;
using System.Data.SQLite;
using System.IO;

namespace RestaurantManagementSystem
{
    public static class DatabaseUtility
    {
        private static string connectionString = string.Empty;

        public static string InitializeDatabase()
        {
            string folderName = "RestaurantData";
            string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string folderPath = Path.Combine(documentsFolder, folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string relativePath = @"restaurant.db";
            string databasePath = Path.Combine(folderPath, relativePath);
            connectionString = $"Data Source={databasePath};Version=3;Pooling=true;Max Pool Size=100;";

            if (!File.Exists(databasePath))
            {
                SQLiteConnection.CreateFile(databasePath);
                CreateTables();
            }

            return connectionString;
        }


        private static void CreateTables()
        {
            using var connection = CreateConnection();

            CreateUsersTable(connection);
            CreateTablesTable(connection);
            CreateMenuTable(connection);
            CreateBillTable(connection);
            CreateOrderedItemsTable(connection);
        }

        public static SQLiteConnection CreateConnection()
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            return connection;
        }

        private static void CreateUsersTable(SQLiteConnection connection)
        {
            var createUsersTableQuery = @"
                CREATE TABLE IF NOT EXISTS Users (
                    UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    Password TEXT NOT NULL,
                    Role TEXT CHECK (Role IN ('admin', 'staff')) NOT NULL,
                    UNIQUE(Username, Email)
                )";

            var createIndexQuery = "CREATE UNIQUE INDEX IF NOT EXISTS UniqueUserIndex ON Users(Username, Email)";

            using var createUsersTableCommand = new SQLiteCommand(createUsersTableQuery, connection);
            using var createIndexCommand = new SQLiteCommand(createIndexQuery, connection);

            createUsersTableCommand.ExecuteNonQuery();
            createIndexCommand.ExecuteNonQuery();
        }

        private static void CreateTablesTable(SQLiteConnection connection)
        {
            var createTablesTableQuery = @"
             CREATE TABLE IF NOT EXISTS Tables (
                 TableId INTEGER PRIMARY KEY AUTOINCREMENT,
                 SeatingCapacity INTEGER,
                 TableType TEXT CHECK(TableType IN ('AC', 'Non-AC')),
                 Status TEXT CHECK(Status IN ('Available', 'Occupied', 'Reserved')),
                 CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                 UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
             )";

            using var command = new SQLiteCommand(createTablesTableQuery, connection);
            command.ExecuteNonQuery();
        }



        private static void CreateMenuTable(SQLiteConnection connection)
        {
            var createMenuTableQuery = @"
        CREATE TABLE IF NOT EXISTS Menu (
            MenuItemId INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            ACPriceHalf DECIMAL(10,2),
            ACPriceFull DECIMAL(10,2),
            NonACPriceHalf DECIMAL(10,2),
            NonACPriceFull DECIMAL(10,2),
            Note TEXT,
            Category TEXT CHECK(Category IN ('Veg', 'Non-Veg', 'Drinks', 'Dessert')) DEFAULT 'Unknown',
            CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
            UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
        )";

            using var command = new SQLiteCommand(createMenuTableQuery, connection);
            command.ExecuteNonQuery();
        }

        private static void CreateBillTable(SQLiteConnection connection)
        {
            var createBillTableQuery = @"
        CREATE TABLE IF NOT EXISTS Bill (
            BillId INTEGER PRIMARY KEY AUTOINCREMENT,
            TableId INTEGER NOT NULL,
            TotalAmount DECIMAL(10,2) NOT NULL,
            Tax DECIMAL(5,2) DEFAULT 0,
            Subtotal DECIMAL(10,2) NOT NULL,
            BillDate DATETIME DEFAULT CURRENT_TIMESTAMP,
            PaymentStatus TEXT CHECK(PaymentStatus IN ('Paid', 'Unpaid')) DEFAULT 'Unpaid',
            CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
            UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY (TableId) REFERENCES Tables(TableId)
        )";

            using var command = new SQLiteCommand(createBillTableQuery, connection);
            command.ExecuteNonQuery();
        }

        private static void CreateOrderedItemsTable(SQLiteConnection connection)
        {
            var createOrderedItemsTableQuery = @"
                CREATE TABLE IF NOT EXISTS OrderedItems (
                    OrderedItemId INTEGER PRIMARY KEY AUTOINCREMENT,
                    BillId INTEGER NOT NULL,
                    MenuItemId INTEGER NOT NULL,
                    Quantity INTEGER NOT NULL,
                    ItemTotal DECIMAL(10,2) NOT NULL,
                    OrderType TEXT CHECK(OrderType IN ('Home Delivery', 'Parcel', 'Dine-In')), -- New field
                    FOREIGN KEY (BillId) REFERENCES Bill(BillId),
                    FOREIGN KEY (MenuItemId) REFERENCES Menu(MenuItemId)
                )";

            using var command = new SQLiteCommand(createOrderedItemsTableQuery, connection);
            command.ExecuteNonQuery();
        }





        public static bool CheckFirstAdminExists()
        {
            using (var connection = CreateConnection())
            {
                using (var command = new SQLiteCommand("SELECT COUNT(*) FROM Users WHERE Role = 'admin'", connection))
                {
                    int adminCount = Convert.ToInt32(command.ExecuteScalar());

                    return adminCount > 0;
                }
            }
        }




    }
}
