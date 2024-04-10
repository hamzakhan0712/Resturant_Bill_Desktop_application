using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RestaurantManagementSystem
{
    public class CategoryToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string category)
            {
                // Map category strings to image paths
                switch (category.ToLower())
                {
                    case "veg":
                        return "pack://application:,,,/RestaurantManagementSystem;component/Images/veg.png"; // Replace with your actual path to the Veg icon
                    case "non-veg":
                        return "pack://application:,,,/RestaurantManagementSystem;component/Images/non-veg.png"; // Replace with your actual path to the Non-Veg icon
                    case "dessert":
                        return "pack://application:,,,/RestaurantManagementSystem;component/Images/dessert.png"; // Replace with your actual path to the Dessert icon
                    case "drinks":
                        return "pack://application:,,,/RestaurantManagementSystem;component/Images/drinks.png"; // Replace with your actual path to the Drinks icon
                    default:
                        return null; // Return null if category not found
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
