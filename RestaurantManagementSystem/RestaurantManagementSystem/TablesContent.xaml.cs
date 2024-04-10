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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RestaurantManagementSystem
{
    public class TableItem
    {
        public int TableId { get; set; }
        public int SeatingCapacity { get; set; }
        public string TableType { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class TablesContent : UserControl
    {
        public ObservableCollection<TableItem> TableItems { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
                    FilterTableItems();
                }
            }
        }


        public TablesContent()
        {
            InitializeComponent();
            DataContext = this;
            TableItems = GetTablesFromDatabase();
            UpdateTableItemsUI();
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

        private void UpdateTableItemsUI()
        {
            // Bind the TableItems collection to the UI control (e.g., ItemsControl)
            // For example:
            ItemsControl.ItemsSource = TableItems;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            TableItems = GetTablesFromDatabase();
            UpdateTableItemsUI();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            var addTableCard = new AddTableCard();
            CardsContainer.Children.Add(addTableCard);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            TableItem selectedTable = TableItems.FirstOrDefault(table => table.IsSelected);

            if (selectedTable != null)
            {
                var editTableCard = new EditTableCard(selectedTable);
                CardsContainer.Children.Add(editTableCard);
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            List<TableItem> tablesToDelete = TableItems.Where(table => table.IsSelected).ToList();

            // Remove selected tables from the UI
            foreach (TableItem table in tablesToDelete.ToList())
            {
                TableItems.Remove(table);
            }

            // Remove selected tables from the database
            DeleteTablesFromDatabase(tablesToDelete);
        }

        private void DeleteTablesFromDatabase(List<TableItem> tables)
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
                        foreach (TableItem table in tables)
                        {
                            string deleteQuery = "DELETE FROM Tables WHERE TableId = @TableId";
                            using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@TableId", table.TableId);
                                command.ExecuteNonQuery();
                            }
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

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (checkBox.DataContext is TableItem table)
                {
                    table.IsSelected = checkBox.IsChecked ?? false;
                }
            }
        }


        // Method to filter table items based on the search text
        private void FilterTableItems()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(TableItems);
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
                        if (item is TableItem tableItem)
                        {
                            // Perform case-insensitive search based on the TableId and Status
                            return tableItem.TableId.ToString().IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                   tableItem.Status.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;
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

    }
}