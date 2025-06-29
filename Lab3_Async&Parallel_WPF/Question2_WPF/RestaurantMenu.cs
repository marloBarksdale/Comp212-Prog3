using System.Collections.Generic;

namespace Question2_WPF
{
    /// <summary>
    /// Provides a static, categorized menu for the restaurant.
    /// Each category contains a list of MenuEntry items with names and prices.
    /// </summary>
    public static class RestaurantMenu
    {
        /// <summary>
        /// The complete menu organized by category.
        /// Key = category name (e.g., "Beverages"), 
        /// Value = list of MenuEntry items in that category.
        /// </summary>
        public static readonly Dictionary<string, List<MenuEntry>> Menu = new()
        {
            // ---------- Beverage Menu ----------
            ["Beverages"] = new()
            {
                new MenuEntry { Name = "Soda", Price = 1.95 },
                new MenuEntry { Name = "Tea", Price = 1.50 },
                new MenuEntry { Name = "Coffee", Price = 1.25 },
                new MenuEntry { Name = "Mineral Water", Price = 2.25 },
                new MenuEntry { Name = "Juice", Price = 2.50 },
                new MenuEntry { Name = "Milk", Price = 1.50 }
            },

            // ---------- Appetizer Menu ----------
            ["Appetizers"] = new()
            {
                new MenuEntry { Name = "Buffalo Wings", Price = 5.95 },
                new MenuEntry { Name = "Buffalo Fingers", Price = 6.95 },
                new MenuEntry { Name = "Potato Skins", Price = 8.95 },
                new MenuEntry { Name = "Nachos", Price = 8.95 },
                new MenuEntry { Name = "Mushroom Caps", Price = 10.95 },
                new MenuEntry { Name = "Shrimp Cocktail", Price = 12.95 },
                new MenuEntry { Name = "Chips and Salsa", Price = 6.95 }
            },

            // ---------- Main Course Menu ----------
            ["Main Courses"] = new()
            {
                new MenuEntry { Name = "Seafood Alfredo", Price = 15.95 },
                new MenuEntry { Name = "Chicken Alfredo", Price = 13.95 },
                new MenuEntry { Name = "Chicken Piccata", Price = 13.95 },
                new MenuEntry { Name = "Turkey Club", Price = 11.95 },
                new MenuEntry { Name = "Lobster Pie", Price = 19.95 },
                new MenuEntry { Name = "Prime Rib", Price = 20.95 },
                new MenuEntry { Name = "Shrimp Scampi", Price = 18.95 },
                new MenuEntry { Name = "Turkey Dinner", Price = 13.95 },
                new MenuEntry { Name = "Stuffed Chicken", Price = 15.95 }
            },

            // ---------- Dessert Menu ----------
            ["Desserts"] = new()
            {
                new MenuEntry { Name = "Apple Pie", Price = 5.95 },
                new MenuEntry { Name = "Sundae", Price = 3.95 },
                new MenuEntry { Name = "Carrot Cake", Price = 5.95 },
                new MenuEntry { Name = "Mud Pie", Price = 5.95 },
                new MenuEntry { Name = "Apple Crisp", Price = 5.95 }
            }
        };
    }
}
