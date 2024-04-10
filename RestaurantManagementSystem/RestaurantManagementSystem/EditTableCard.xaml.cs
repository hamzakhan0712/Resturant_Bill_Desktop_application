using System;
using System.Collections.Generic;
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

using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;
using System.Data;

namespace RestaurantManagementSystem
{
    public partial class EditTableCard : UserControl
    {
        private TableItem currentTableItem;

        public EditTableCard(TableItem tableItem)
        {
            InitializeComponent();
            currentTableItem = tableItem;

            // Display existing values before editing
            DisplayExistingValues();
        }

        private void DisplayExistingValues()
        {
            // Display existing values in the fields
            if (currentTableItem != null)
            {
                TxtBoxEditTableId.Text = currentTableItem.TableId.ToString();
                TxtBoxEditTableSeatingCapacity.Text = currentTableItem.SeatingCapacity.ToString();

                // Find the corresponding ComboBoxItem and select it
                var tableTypeComboBoxItem = TableTypeComboBox.Items.OfType<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == currentTableItem.TableType);
                if (tableTypeComboBoxItem != null)
                {
                    TableTypeComboBox.SelectedItem = tableTypeComboBoxItem;
                }

                var statusComboBoxItem = StatusComboBox.Items.OfType<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == currentTableItem.Status);
                if (statusComboBoxItem != null)
                {
                    StatusComboBox.SelectedItem = statusComboBoxItem;
                }
            }
        }

        private void CloseClick_Click(object sender, RoutedEventArgs e)
        {
            // Close the edit card
            (this.Parent as Panel)?.Children.Remove(this);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Update table item with edited values
            if (currentTableItem != null)
            {
                currentTableItem.SeatingCapacity = int.Parse(TxtBoxEditTableSeatingCapacity.Text);

                // Update the selected table type and status
                if (TableTypeComboBox.SelectedItem is ComboBoxItem selectedTableType)
                {
                    currentTableItem.TableType = selectedTableType.Content.ToString();
                }

                if (StatusComboBox.SelectedItem is ComboBoxItem selectedStatus)
                {
                    currentTableItem.Status = selectedStatus.Content.ToString();
                }

                // Perform the update operation in the database
                UpdateTableInDatabase(currentTableItem);

                // Clear fields after editing
                ClearFields();

                // Close the edit card
                (this.Parent as Panel)?.Children.Remove(this);
            }
        }

        private void UpdateTableInDatabase(TableItem tableItem)
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
                        string updateQuery = @"
                UPDATE Tables
                SET SeatingCapacity = @SeatingCapacity,
                    TableType = @TableType,
                    Status = @Status,
                    UpdatedAt = CURRENT_TIMESTAMP
                WHERE TableId = @TableId";

                        using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@SeatingCapacity", tableItem.SeatingCapacity);
                            command.Parameters.AddWithValue("@TableType", tableItem.TableType);
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


        private void ClearFields()
        {
            // Clear all input fields after editing
            TxtBoxEditTableId.Text = string.Empty;
            TxtBoxEditTableSeatingCapacity.Text = string.Empty;
            TableTypeComboBox.SelectedItem = null;
            StatusComboBox.SelectedItem = null;
        }
    }
}
