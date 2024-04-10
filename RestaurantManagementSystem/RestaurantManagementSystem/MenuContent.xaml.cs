using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using static RestaurantManagementSystem.MenuContent;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace RestaurantManagementSystem
{

    public class CustomMenuItem
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public decimal ACPriceHalf { get; set; }
        public decimal ACPriceFull { get; set; }
        public decimal NonACPriceHalf { get; set; }
        public decimal NonACPriceFull { get; set; }
        public string Note { get; set; }
        public string Category { get; set; }
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
    public partial class MenuContent : UserControl
    {
        public ObservableCollection<CustomMenuItem> MenuItems { get; set; }

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
                    FilterMenuItems();
                }
            }
        }
        public MenuContent()
        {
            InitializeComponent();
            DataContext = this;
            MenuItems = GetMenuItemsFromDatabase();

        }
      

        public ObservableCollection<CustomMenuItem> GetMenuItemsFromDatabase()
        {
            ObservableCollection<CustomMenuItem> menuItems = new ObservableCollection<CustomMenuItem>();

            using (SQLiteConnection connection = DatabaseUtility.CreateConnection())
            {
                string query = @"
            SELECT 
                MenuItemId,
                Name,
                ACPriceHalf,
                ACPriceFull,
                NonACPriceHalf,
                NonACPriceFull,
                Note,
                Category,
                CreatedAt,
                UpdatedAt
            FROM 
                Menu";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
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
                                Category = reader["Category"].ToString(), // Store category as string
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

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            // Reload data
            MenuItems = GetMenuItemsFromDatabase();
         
            UpdateMenuItemsUI();
        }

        private void UpdateMenuItemsUI()
        {
            MenuItemsControl.ItemsSource = MenuItems; // Replace MenuItemsControl with the actual control name in your XAML
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            // Create an instance of the AddMenuItemCard UserControl
            var newCard = new AddMenuItemCard();

            // Add the new card to the CardsContainer
            CardsContainer.Children.Add(newCard);
        }

     


        private void DeleteItemsFromDatabase(List<CustomMenuItem> items)
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
                        foreach (CustomMenuItem item in items)
                        {
                            string deleteQuery = "DELETE FROM Menu WHERE MenuItemId = @MenuItemId";
                            using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@MenuItemId", item.MenuItemId);
                                command.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Handle exception or rollback transaction
                        transaction.Rollback();
                        // Log or display error message
                    }
                }
            }
        }

        // Event handler for a delete button
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Get selected items
            List<CustomMenuItem> itemsToDelete = MenuItems.Where(item => item.IsSelected).ToList();

            // Remove from UI
            foreach (CustomMenuItem item in itemsToDelete)
            {
                MenuItems.Remove(item);
            }

            // Remove from database
            DeleteItemsFromDatabase(itemsToDelete);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected item to edit
            CustomMenuItem selectedItem = MenuItems.FirstOrDefault(item => item.IsSelected);

            if (selectedItem != null)
            {
                // Create an instance of the EditMenuItemCard UserControl and pass the selected item
                var editCard = new EditMenuItemCard(selectedItem);

                // Add the edit card to the CardsContainer
                CardsContainer.Children.Add(editCard);
            }
        }


        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            // Find the CheckBox that was clicked
            if (sender is CheckBox checkBox)
            {
                // Get the corresponding CustomMenuItem from the DataContext of the CheckBox
                if (checkBox.DataContext is CustomMenuItem item)
                {
                    item.IsSelected = checkBox.IsChecked ?? false;
                }
            }
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




    }
}











