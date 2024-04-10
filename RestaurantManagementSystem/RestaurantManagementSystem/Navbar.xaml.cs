using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Data.SQLite;
using System.IO;
using MahApps.Metro.Controls;
using System.Data;
using System.Windows.Media;

namespace RestaurantManagementSystem
{
    /// <summary>
    /// Interaction logic for Navbar.xaml
    /// </summary>
    public partial class Navbar : MetroWindow
    {

        private string Username { get; }
        private string UserRole { get; }
        private string connectionString = string.Empty;
        private DataTable menuDataTable;

        public Navbar(string username, string role)
        {
            InitializeComponent();
            connectionString = DatabaseUtility.InitializeDatabase();
            Username = username;
            UserRole = role;

            DashboardContent dashboardContent = new DashboardContent();
            MainPageBorder.Child = null;
            MainPageBorder.Child = dashboardContent;

            // Subscribe to the NewOrderClicked event in the DashboardContent instance
            dashboardContent.NewOrderClicked += DashboardContent_NewOrderClicked;
        }



        private int totalTabsCreated = 0;

        private void DashboardContent_NewOrderClicked(object sender, EventArgs e)
        {
            try
            {
                UserControl newTabContent = new Bill(); // Replace with your content control

                string tabHeader = $"Tab1 {++totalTabsCreated}"; // Increment totalTabsCreated and use it for the tab header

                AddNewTab(tabHeader, newTabContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while creating a new tab: {ex.Message}");
                // You can log or handle the exception as needed
            }
        }



        // Method to add a new tab
        private void AddNewTab(string tabHeader, UserControl tabContent)
        {
            var newTab = new TabItem();
            newTab.Header = tabHeader;
            newTab.Content = tabContent;

            // Assuming your right area is named 'BillCardTabControl'
            OrderTabControl.Items.Add(newTab); // Add the new tab to the TabControl
            OrderTabControl.SelectedItem = newTab; // Select the newly added tab

            // Additional logic for tab management if needed
        }

        private void CloseTab(object sender, RoutedEventArgs e)
        {
            if (sender is Button closeButton)
            {
                var tabItem = FindParent<TabItem>(closeButton);

                if (tabItem != null)
                {
                    // Remove the tab from the TabControl
                    OrderTabControl.Items.Remove(tabItem);

                    // Decrement totalTabsCreated when a tab is closed
                    totalTabsCreated--;
                }
            }
        }

        // Helper method to find parent of a specific type
        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;

            if (parentObject is T parent)
                return parent;
            else
                return FindParent<T>(parentObject);
        }


        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            DashboardContent dashboardContent = new DashboardContent(); // Replace DashboardContent with your actual dashboard content control

            MainPageBorder.Child = null; // Clear the current content of MainPageBorder

            MainPageBorder.Child = dashboardContent; // Set the new content (dashboardContent)

            // Subscribe to the NewOrderClicked event in the DashboardContent instance
            dashboardContent.NewOrderClicked += DashboardContent_NewOrderClicked;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MenuContent menuContent = new MenuContent();

            MainPageBorder.Child = null;

            MainPageBorder.Child = menuContent;
        }

        private void TablesButton_Click(object sender, RoutedEventArgs e)
        {
            TablesContent tablesContent = new TablesContent();

            MainPageBorder.Child = null;

            MainPageBorder.Child = tablesContent;
        }

        private void BillsButton_Click(object sender, RoutedEventArgs e)
        {
            BillsContent billsContent = new BillsContent();

            MainPageBorder.Child = null;

            MainPageBorder.Child = billsContent;
        }

        private void SalesButton_Click(object sender, RoutedEventArgs e)
        {
            SalesContent salesContent = new SalesContent();

            MainPageBorder.Child = null;

            MainPageBorder.Child = salesContent;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserManage userManageWindow = new UserManage();
            userManageWindow.Show();
            this.Close();
        }



    }
}
