using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Question2_WPF
{
    public partial class MainWindow : Window
    {
        // Holds the current list of ordered items for the bill
        private ObservableCollection<OrderItem> currentBill = new();

        public MainWindow()
        {
            InitializeComponent();
            LoadMenu(); // Populate ComboBoxes with menu items
            DataGridOrders.ItemsSource = currentBill; // Bind the DataGrid to the current bill
        }

        // Loads menu items into each ComboBox by category
        private void LoadMenu()
        {
            ComboBoxBeverages.ItemsSource = RestaurantMenu.Menu["Beverages"];
            ComboBoxAppetizers.ItemsSource = RestaurantMenu.Menu["Appetizers"];
            ComboBoxMainCourses.ItemsSource = RestaurantMenu.Menu["Main Courses"];
            ComboBoxDesserts.ItemsSource = RestaurantMenu.Menu["Desserts"];
        }

        // Adds a selected menu item to the bill, or increases quantity if already present
        private void AddItem(string category, MenuEntry selected)
        {
            var existing = currentBill.FirstOrDefault(item => item.ItemName == selected.Name);

            if (existing != null)
            {
                existing.Quantity++; // Increase quantity if item already in bill
            }
            else
            {
                // Add new item to the bill
                currentBill.Add(new OrderItem
                {
                    Category = category,
                    ItemName = selected.Name,
                    UnitPrice = selected.Price,
                    Quantity = 1
                });
            }

            UpdateTotals(); // Refresh subtotal, tax, and total
        }

        // Calculates and updates the subtotal, tax, and total displayed in the UI
        private void UpdateTotals()
        {
            double subtotal = currentBill.Sum(i => i.LineTotal);
            double tax = subtotal * 0.1; // 10% tax
            double total = subtotal + tax;

            TextBlockSubtotal.Text = $"Subtotal: ${subtotal:F2}";
            TextBlockTax.Text = $"Tax: ${tax:F2}";
            TextBlockTotal.Text = $"Total: ${total:F2}";
        }

        // Clears the entire bill and updates totals
        private void ClearBill_Click(object sender, RoutedEventArgs e)
        {
            currentBill.Clear();
            UpdateTotals();
        }

        // Removes the currently selected item from the bill and updates totals
        private void RemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridOrders.SelectedItem is OrderItem selected)
            {
                currentBill.Remove(selected);
                UpdateTotals();
            }
        }

        // Handles selection changes in any ComboBox; adds the selected item to the bill
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            var comboBox = sender as ComboBox;
            var selected = (MenuEntry)comboBox.SelectedItem;

            // Determine the category based on which ComboBox triggered the event
            string category = comboBox == ComboBoxBeverages ? "Beverages" :
                              comboBox == ComboBoxAppetizers ? "Appetizers" :
                              comboBox == ComboBoxMainCourses ? "Main Courses" :
                              comboBox == ComboBoxDesserts ? "Desserts" : "";

            AddItem(category, selected);
            comboBox.SelectedItem = null; // Reset selection for user convenience
        }

        // Increases the quantity of the item in the DataGrid row where the "+" button was clicked
        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (GetRowItemFromSender(sender) is OrderItem item)
            {
                item.Quantity++;
                UpdateTotals();
            }
        }

        // Decreases the quantity of the item in the DataGrid row where the "−" button was clicked
        // Removes the item if quantity drops to zero
        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (GetRowItemFromSender(sender) is OrderItem item)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                    UpdateTotals();
                }
                else
                {
                    currentBill.Remove(item);
                    UpdateTotals();
                }
            }
        }

        // Removes the item from the bill when the "X" button is clicked in the DataGrid
        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (GetRowItemFromSender(sender) is OrderItem item)
            {
                currentBill.Remove(item);
                UpdateTotals();
            }
        }

        // Helper method to retrieve the OrderItem associated with a DataGrid row button click
        private OrderItem GetRowItemFromSender(object sender)
        {
            return ((FrameworkElement)sender).DataContext as OrderItem;
        }
    }
}
