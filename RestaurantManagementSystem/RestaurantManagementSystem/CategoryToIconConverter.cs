using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RestaurantManagementSystem
{
    public class CategoryToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string category = value as string;
            if (category == "Veg")
            {
                return "/Images/VegIcon.png"; // Provide the path to your Veg icon
            }
            else if (category == "Non-Veg")
            {
                return "/Images/NonVegIcon.png"; // Provide the path to your Non-Veg icon
            }
            else if (category == "Drinks")
            {
                return "/Images/DrinksIcon.png"; // Provide the path to your Drinks icon
            }
            else if (category == "Dessert")
            {
                return "/Images/DessertIcon.png"; // Provide the path to your Dessert icon
            }
            // Add similar conditions for other categories
            return null; // Default icon or empty image
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
