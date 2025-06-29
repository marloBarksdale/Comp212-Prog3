using System;
using System.ComponentModel;

namespace Question2_WPF
{
    /// <summary>
    /// Represents a single item in the customer's bill.
    /// Used to display and update quantity and pricing dynamically in the UI.
    /// </summary>
    public class OrderItem : INotifyPropertyChanged
    {
        private int quantity;

        /// <summary>
        /// The category of the item (e.g., "Beverages", "Desserts").
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The name of the item (e.g., "Coffee", "Steak").
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// The unit price of the item in dollars.
        /// </summary>
        public double UnitPrice { get; set; }

        /// <summary>
        /// The number of units selected for this item.
        /// Changing this triggers UI updates for quantity and total.
        /// </summary>
        public int Quantity
        {
            get => quantity;
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(LineTotal));
                }
            }
        }

        /// <summary>
        /// The total price for this line item (UnitPrice * Quantity).
        /// Updates automatically when quantity changes.
        /// </summary>
        public double LineTotal => UnitPrice * Quantity;

        /// <summary>
        /// Event triggered when a property changes, allowing UI to update.
        /// Required by INotifyPropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="name">The name of the property that changed.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
