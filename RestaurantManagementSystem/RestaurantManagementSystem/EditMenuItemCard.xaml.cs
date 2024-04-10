using System;
using System.Collections.Generic;
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
    public partial class EditMenuItemCard : UserControl
    {
        public CustomMenuItem ItemToEdit { get; set; }

        public EditMenuItemCard(CustomMenuItem item)
        {
            InitializeComponent();
            ItemToEdit = item;
            DataContext = this;

            // Populate UI elements with existing values
            TxtBoxNewItemName.Text = ItemToEdit.Name;
            TxtBoxNote.Text = ItemToEdit.Note;
            TxtBoxNewItemACPriceHalf.Text = ItemToEdit.ACPriceHalf.ToString();
            TxtBoxNewItemACPriceFull.Text = ItemToEdit.ACPriceFull.ToString();
            TxtBoxNewItemNonACPriceHalf.Text = ItemToEdit.NonACPriceHalf.ToString();
            TxtBoxNewItemNonACPriceFull.Text = ItemToEdit.NonACPriceFull.ToString();

            // Set the existing category value in the ComboBox
            foreach (ComboBoxItem categoryItem in CategoryComboBox.Items)
            {
                if (categoryItem.Content.ToString() == ItemToEdit.Category)
                {
                    CategoryComboBox.SelectedItem = categoryItem;
                    break;
                }
            }
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
            if (string.IsNullOrWhiteSpace(itemName) || string.IsNullOrWhiteSpace(acPriceHalfStr) || string.IsNullOrWhiteSpace(acPriceFullStr) ||
                string.IsNullOrWhiteSpace(nonACPriceHalfStr) || string.IsNullOrWhiteSpace(nonACPriceFullStr) || string.IsNullOrEmpty(selectedCategory))
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

            // Update the properties of ItemToEdit with the values from UI elements
            ItemToEdit.Name = itemName;
            ItemToEdit.Category = selectedCategory;
            ItemToEdit.Note = TxtBoxNote.Text;
            ItemToEdit.ACPriceHalf = acPriceHalf;
            ItemToEdit.ACPriceFull = acPriceFull;
            ItemToEdit.NonACPriceHalf = nonACPriceHalf;
            ItemToEdit.NonACPriceFull = nonACPriceFull;

            // Update the database with the edited item's values
            using (SQLiteConnection connection = DatabaseUtility.CreateConnection())
            {
                string updateQuery = @"
            UPDATE Menu 
            SET Name = @Name,
                ACPriceHalf = @ACPriceHalf,
                ACPriceFull = @ACPriceFull,
                NonACPriceHalf = @NonACPriceHalf,
                NonACPriceFull = @NonACPriceFull,
                Note = @Note,
                Category = @Category,
                UpdatedAt = @UpdatedAt
            WHERE MenuItemId = @MenuItemId";

                using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", itemName);
                    command.Parameters.AddWithValue("@ACPriceHalf", acPriceHalf);
                    command.Parameters.AddWithValue("@ACPriceFull", acPriceFull);
                    command.Parameters.AddWithValue("@NonACPriceHalf", nonACPriceHalf);
                    command.Parameters.AddWithValue("@NonACPriceFull", nonACPriceFull);
                    command.Parameters.AddWithValue("@Note", TxtBoxNote.Text);
                    command.Parameters.AddWithValue("@Category", selectedCategory);
                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                    command.Parameters.AddWithValue("@MenuItemId", ItemToEdit.MenuItemId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Menu item updated successfully!");

            // Close the edit panel
            if (Parent is Panel parentPanel)
            {
                parentPanel.Children.Remove(this);
            }
        }


        private void CloseClick_Click(object sender, RoutedEventArgs e)
        {

            if (Parent is Panel parentPanel)
            {
                parentPanel.Children.Remove(this);
            }
        }
    }

}
