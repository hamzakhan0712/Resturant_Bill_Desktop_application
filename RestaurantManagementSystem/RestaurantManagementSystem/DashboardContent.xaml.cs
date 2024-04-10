using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RestaurantManagementSystem
{


    public partial class DashboardContent : UserControl
    {

        public ObservableCollection<TableItem> TableItems { get; set; }
        public ObservableCollection<CustomMenuItem> MenuItems { get; set; }
      

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (searchText != value)
                {
                    searchText = value;
                    OnPropertyChanged(nameof(SearchText)); // Implement INotifyPropertyChanged for this
                    FilterMenuItems();
                }
            }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler NewOrderClicked;

        private void NewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            NewOrderClicked?.Invoke(this, EventArgs.Empty); // Raise the event
        }

        public class TableItem : INotifyPropertyChanged
        {
            private string status;
            public int TableId { get; set; }
            public int SeatingCapacity { get; set; }
            public string TableType { get; set; }

            public string Status
            {
                get { return status; }
                set
                {
                    if (status != value)
                    {
                        status = value;
                        OnPropertyChanged(nameof(Status));
                    }
                }
            }

            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            // ... Other properties and methods
        }



        public DashboardContent()
        {
            InitializeComponent();
            TableItems = GetTablesFromDatabase();
            MenuItems = GetMenuItemsFromDatabase();
            DataContext = this;

        }

        public ObservableCollection<TableItem> GetTablesFromDatabase()
        {
            ObservableCollection<TableItem> tables = new ObservableCollection<TableItem>();

            using (SQLiteConnection connection = DatabaseUtility.CreateConnection())
            {
                string query = @"
                SELECT TableId, SeatingCapacity, TableType, Status, CreatedAt, UpdatedAt
                FROM Tables";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TableItem table = new TableItem
                            {
                                TableId = Convert.ToInt32(reader["TableId"]),
                                SeatingCapacity = Convert.ToInt32(reader["SeatingCapacity"]),
                                TableType = reader["TableType"].ToString(),
                                Status = reader["Status"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                            };

                            tables.Add(table);
                        }
                    }
                }
            }

            return tables;
        }
        public ObservableCollection<CustomMenuItem> GetMenuItemsFromDatabase()
        {
            ObservableCollection<CustomMenuItem> menuItems = new ObservableCollection<CustomMenuItem>();

            using (SQLiteConnection connection = DatabaseUtility.CreateConnection())
            {
                string query = @"
                    SELECT MenuItemId, Name, ACPriceHalf, ACPriceFull, NonACPriceHalf, NonACPriceFull, Note, Category, CreatedAt, UpdatedAt
                    FROM Menu";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CustomMenuItem menuItem = new CustomMenuItem
                            {
                                MenuItemId = Convert.ToInt32(reader["MenuItemId"]),
                                Name = reader["Name"].ToString(),
                                ACPriceHalf = Convert.ToDecimal(reader["ACPriceHalf"]),
                                ACPriceFull = Convert.ToDecimal(reader["ACPriceFull"]),
                                NonACPriceHalf = Convert.ToDecimal(reader["NonACPriceHalf"]),
                                NonACPriceFull = Convert.ToDecimal(reader["NonACPriceFull"]),
                                Note = reader["Note"].ToString(),
                                Category = reader["Category"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                            };

                            menuItems.Add(menuItem);
                        }
                    }
                }
            }

            return menuItems;
        }


        // Method to filter menu items based on the search text
        private void FilterMenuItems()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(MenuItems);
            if (view != null)
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    view.Filter = null; // Show all items if search text is empty
                }
                else
                {
                    view.Filter = item =>
                    {
                        if (item is CustomMenuItem menuItem)
                        {
                            // Perform case-insensitive search based on the item's name
                            return menuItem.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;
                        }
                        return false;
                    };
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                SearchText = textBox.Text;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            TableItems = GetTablesFromDatabase();
            MenuItems = GetMenuItemsFromDatabase();


        }

        private void ChangeStatusToAvailable_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                TableItem tableItem = button.DataContext as TableItem;
                if (tableItem != null)
                {
                    tableItem.Status = "Available"; // Update status
                    UpdateTableStatus(tableItem); // Call the method to update the status in the database
                   
                }
            }
        }

        private void ChangeStatusToOccupied_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                TableItem tableItem = button.DataContext as TableItem;
                if (tableItem != null)
                {
                    tableItem.Status = "Occupied"; // Update status
                    UpdateTableStatus(tableItem); // Call the method to update the status in the database
                     // Refresh the TableItems collection binding
                }
            }
        }

        private void ChangeStatusToReserved_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                TableItem tableItem = button.DataContext as TableItem;
                if (tableItem != null)
                {
                    tableItem.Status = "Reserved"; // Update status
                    UpdateTableStatus(tableItem); // Call the method to update the status in the database
                     // Refresh the TableItems collection binding
                }
            }
        }

       

        private void UpdateTableStatus(TableItem tableItem)
        {
            using (SQLiteConnection connection = DatabaseUtility.CreateConnection())
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string updateStatusQuery = @"
                    UPDATE Tables
                    SET Status = @Status,
                    UpdatedAt = CURRENT_TIMESTAMP
                    WHERE TableId = @TableId";

                        using (SQLiteCommand command = new SQLiteCommand(updateStatusQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@Status", tableItem.Status);
                            command.Parameters.AddWithValue("@TableId", tableItem.TableId);

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // Handle exception or display error message
                    }
                }
            }
        }



    }

}
