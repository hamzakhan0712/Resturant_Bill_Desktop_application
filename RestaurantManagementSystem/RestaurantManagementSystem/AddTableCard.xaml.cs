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
    /// <summary>
    /// Interaction logic for AddTableCard.xaml
    /// </summary>
    public partial class AddTableCard : UserControl
    {
        public AddTableCard()
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
            // Add clearing logic for table details fields
            TxtBoxTableSeatingCapacity.Text = string.Empty;
            TableTypeComboBox.SelectedIndex = -1;
            StatusComboBox.SelectedIndex = -1;
            // Add more fields if necessary
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TxtBoxTableSeatingCapacity.Text, out int seatingCapacity) || seatingCapacity <= 0)
            {
                MessageBox.Show("Please enter a valid positive integer for Seating Capacity.");
                return;
            }

            string selectedTableType = (TableTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string selectedStatus = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(selectedTableType) || string.IsNullOrEmpty(selectedStatus))
            {
                MessageBox.Show("Please fill in all the required fields.");
                return;
            }

            // Proceed with insertion into the database
            using (SQLiteConnection connection = DatabaseUtility.CreateConnection())
            {
                string insertQuery = @"
            INSERT INTO Tables (SeatingCapacity, TableType, Status)
            VALUES (@SeatingCapacity, @TableType, @Status)";

                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@SeatingCapacity", seatingCapacity);
                    command.Parameters.AddWithValue("@TableType", selectedTableType);
                    command.Parameters.AddWithValue("@Status", selectedStatus);

                    try
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        command.ExecuteNonQuery();
                        MessageBox.Show("Table added successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                }
            }

            ClearFormFields();
            CloseClick_Click(sender, e);
        }

    }
}

