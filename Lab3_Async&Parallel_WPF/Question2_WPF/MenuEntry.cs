namespace Question2_WPF
{
    /// <summary>
    /// Represents a selectable menu item in the restaurant system.
    /// Contains the item's display name and price.
    /// </summary>
    public class MenuEntry
    {
        /// <summary>
        /// The name of the menu item (e.g., "Coffee", "Steak").
        /// This will be displayed in ComboBoxes and on the bill.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The unit price of the menu item in dollars.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Overrides ToString() to display the item name by default.
        /// Useful if the item is displayed in a ComboBox without a custom ItemTemplate.
        /// </summary>
        public override string ToString() => Name;
    }
}
