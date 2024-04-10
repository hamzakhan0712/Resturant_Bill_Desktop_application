using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for AddMenuItemCard.xaml
    /// </summary>
    public partial class AddMenuItemCard : UserControl
    {
        public AddMenuItemCard()
        {
            InitializeComponent();

        }

        private void CloseClick_Click(object sender, RoutedEventArgs e)
        {
            // Close logic: Remove this UserControl from its parent container
            if (Parent is Panel parentPanel)
            {
                parentPanel.Children.Remove(this);
            }
        }

        private void ClearFormFields()
        {
            TxtBoxNewItemName.Text = string.Empty;
            TxtBoxNewItemACPriceHalf.Text = string.Empty;
            TxtBoxNewItemACPriceFull.Text = string.Empty;
            TxtBoxNewItemNonACPriceHalf.Text = string.Empty;
            TxtBoxNewItemNonACPriceFull.Text = string.Empty;
            CategoryComboBox.SelectedIndex = -1;
            TxtBoxNote.Text = string.Empty;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string itemName = TxtBoxNewItemName.Text;
            string acPriceHalfStr = TxtBoxNewItemACPriceHalf.Text;
            string acPriceFullStr = TxtBoxNewItemACPriceFull.Text;
            string nonACPriceHalfStr = TxtBoxNewItemNonACPriceHalf.Text;
            string nonACPriceFullStr = TxtBoxNewItemNonACPriceFull.Text;
            string selectedCategory = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Perform validation on required fields
            if (string.IsNullOrWhiteSpace(itemName) || string.IsNullOrWhiteSpace(acPriceFullStr) || string.IsNullOrWhiteSpace(nonACPriceFullStr) || string.IsNullOrEmpty(selectedCategory))
            {
                MessageBox.Show("Please fill in all the required fields.");
                return; // Exit the method if any required field is empty
            }

            // Validation for decimal values
            if (!decimal.TryParse(acPriceHalfStr, out decimal acPriceHalf) || !decimal.TryParse(acPriceFullStr, out decimal acPriceFull) ||
                !decimal.TryParse(nonACPriceHalfStr, out decimal nonACPriceHalf) || !decimal.TryParse(nonACPriceFullStr, out decimal nonACPriceFull))
            {
                MessageBox.Show("Please enter valid price values.");
                return; // Exit the method if price values are not valid
            }

            // Proceed with insertion into the database
            using (SQLiteConnection connection = DatabaseUtility.CreateConnection())
            {
                string insertQuery = @"
            INSERT INTO Menu (Name, ACPriceHalf, ACPriceFull, NonACPriceHalf, NonACPriceFull, Note, Category)
            VALUES (@Name, @ACPriceHalf, @ACPriceFull, @NonACPriceHalf, @NonACPriceFull, @Note, @Category)";

                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", itemName);
                    command.Parameters.AddWithValue("@ACPriceHalf", acPriceHalf);
                    command.Parameters.AddWithValue("@ACPriceFull", acPriceFull);
                    command.Parameters.AddWithValue("@NonACPriceHalf", nonACPriceHalf);
                    command.Parameters.AddWithValue("@NonACPriceFull", nonACPriceFull);
                    command.Parameters.AddWithValue("@Note", TxtBoxNote.Text);
                    command.Parameters.AddWithValue("@Category", selectedCategory);

                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Menu item added successfully!");
            ClearFormFields();
            CloseClick_Click(sender, e);
        }



    }
}
