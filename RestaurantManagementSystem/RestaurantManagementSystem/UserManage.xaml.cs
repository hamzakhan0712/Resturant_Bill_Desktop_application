using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Data.SQLite;
using System.IO;
using MahApps.Metro.Controls;

namespace RestaurantManagementSystem
{
    public partial class UserManage : MetroWindow
    {
        private readonly string defaultMAC = "38D57A973717";
        private bool userCreated = false;
        private string connectionString = string.Empty;

        public UserManage()
        {
            InitializeComponent();
            InitializeDatabase();
            CheckAndHideRegisterButton();
        }

        private void InitializeDatabase()
        {
            connectionString = DatabaseUtility.InitializeDatabase();
        }

        private string GetMACAddress()
        {
            string macAddress = string.Empty;
            try
            {
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                                            .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                                            .FirstOrDefault();
                macAddress = networkInterfaces?.GetPhysicalAddress().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to retrieve MAC address. Error: {ex.Message}");
            }
            return macAddress;
        }

        private async Task SendEmailAsync(string userEmail, string username, string password)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("hamza81khan81@gmail.com");
                mail.To.Add(userEmail);
                mail.Subject = "Password Reminder";

                mail.Body = $"Dear {username},\n\nHere are your login credentials for the Restaurant Management System:\n\nUsername: {username}\nPassword: {password}\n\nPlease keep this information secure and do not share it with anyone.\n\nBest regards,\nThe Restaurant Management Team";

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential("hamza81khan81@gmail.com", "oygigusfsoejfvsy");
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(mail);
                MessageBox.Show("Email sent successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send email. Error: {ex.Message}");
            }
        }


        private string RetrieveUsername(string enteredPassword)
        {
            string retrievedUsername = "";
            try
            {
                using (SQLiteConnection connection = DatabaseUtility.CreateConnection())
                {
                    string sql = "SELECT Username FROM Users WHERE Password = @Password";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Password", enteredPassword);
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            retrievedUsername = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving username: {ex.Message}");
            }

            return retrievedUsername;
        }


        private bool CheckCurrentPassword(string username, string currentPassword)
        {
            try
            {
                using (SQLiteConnection connection = DatabaseUtility.CreateConnection())
                {
                    string sql = "SELECT Password FROM Users WHERE Username = @Username";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            string storedPassword = result.ToString();
                            return currentPassword == storedPassword;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking current password: {ex.Message}");
            }

            return false;
        }


        private bool UpdatePasswordInDatabase(string username, string newPassword)
        {
            try
            {
                using (SQLiteConnection connection = DatabaseUtility.CreateConnection())

                {
                    string sql = "UPDATE Users SET Password = @NewPassword WHERE Username = @Username";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@NewPassword", newPassword);
                        command.Parameters.AddWithValue("@Username", username);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating password: {ex.Message}");
                return false;
            }
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = LoginUsernameTextBox.Text;
            string password = LoginPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter a username and password.");
                return;
            }

            string role = await Task.Run(() => AuthenticateAndGetRole(username, password));

            if (string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Invalid username or password.");
                return;
            }

            Navbar navbar = new Navbar(username, role);
            navbar.Show();
            this.Close();
        }

        private string AuthenticateAndGetRole(string username, string password)
        {
            string role = string.Empty;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Role FROM Users WHERE Username=@Username AND Password=@Password";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            role = result.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database connection error: {ex.Message}");
                }

            }

            return role;
        }



        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            LoginForm.Visibility = Visibility.Collapsed;
            ChangePasswordPanel.Visibility = Visibility.Visible;
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            CreateUserForm.Visibility = Visibility.Collapsed;
            ChangePasswordPanel.Visibility = Visibility.Collapsed;
            LoginForm.Visibility = Visibility.Visible;
        }


        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            string username = ""; 
            string currentPassword = CurrentPasswordBox.Password;
            string newPassword = NewPasswordBox.Password;
            string confirmNewPassword = ConfirmNewPasswordBox.Password;

            string retrievedUsername = RetrieveUsername(currentPassword);

            if (string.IsNullOrEmpty(retrievedUsername))
            {
                MessageBox.Show("Invalid credentials.");
                return;
            }

            bool isCurrentPasswordCorrect = CheckCurrentPassword(retrievedUsername, currentPassword);

            if (!isCurrentPasswordCorrect)
            {
                MessageBox.Show("Current password is incorrect.");
                return;
            }

            if (newPassword != confirmNewPassword)
            {
                MessageBox.Show("New password and confirm password do not match.");
                return;
            }

            bool isPasswordUpdated = UpdatePasswordInDatabase(retrievedUsername, newPassword);

            if (isPasswordUpdated)
            {
                MessageBox.Show("Password updated successfully!");
                LoginForm.Visibility = Visibility.Visible;
                ChangePasswordPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Failed to update password. Please try again.");
            }
        }

        private async void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;


            string role = "";
            if (AdminCheckBox.IsChecked == true && StaffCheckBox.IsChecked == true)
            {
                MessageBox.Show("Please select only one role.");
                return;
            }
            else if (AdminCheckBox.IsChecked == true)
            {
                role = "admin";
            }
            else if (StaffCheckBox.IsChecked == true)
            {
                role = "saff";
            }
            else
            {
                MessageBox.Show("Please select a role.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            string userMAC = GetMACAddress();
            if (userMAC != defaultMAC)
            {
                MessageBox.Show("Unauthorized access. Application can only be used on a specific machine.");
                return;
            }

            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            var insertUserQuery = "INSERT INTO Users (Username, Email, Password, Role) VALUES (@Username, @Email, @Password, @Role)";
            using var command = new SQLiteCommand(insertUserQuery, connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", password);
            command.Parameters.AddWithValue("@Role", role); 
            command.ExecuteNonQuery();

            userCreated = true;
            LoginForm.Visibility = userCreated ? Visibility.Visible : Visibility.Visible;
            CreateUserForm.Visibility = userCreated ? Visibility.Collapsed : Visibility.Collapsed;
            CheckAndHideRegisterButton();
            await SendEmailAsync(email, username, password); 
            
        }

        private void NewRegistration_Click(object sender, MouseButtonEventArgs e)
        {
            CreateUserForm.Visibility = Visibility.Visible;
            LoginForm.Visibility = Visibility.Collapsed;

        }

        private void CheckAndHideRegisterButton()
        {

            bool isFirstAdminCreated = CheckIfFirstAdminCreated();

            if (isFirstAdminCreated)
            {
                RegisterButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                RegisterButton.Visibility = Visibility.Visible;
            }
        }

        private bool CheckIfFirstAdminCreated()
        {
          
            bool isFirstAdminExists = DatabaseUtility.CheckFirstAdminExists();

            return isFirstAdminExists;
        }


    }
}
