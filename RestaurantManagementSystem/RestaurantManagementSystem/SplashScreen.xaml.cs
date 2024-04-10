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
using System.Windows.Shapes;

using System;
using System.Threading.Tasks;


namespace RestaurantManagementSystem
{
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();

            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            var timer = (System.Windows.Threading.DispatcherTimer)sender;
            timer.Stop();

            UserManage userManageWindow = new UserManage();
            userManageWindow.Show();
            this.Close();
        }
    }
}

