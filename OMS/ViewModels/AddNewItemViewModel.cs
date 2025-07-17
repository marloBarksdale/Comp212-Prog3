using OMS.Commands;
using OMS.Data.Models;
using OMS.Data.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace OMS.ViewModels
{
    public class AddNewItemViewModel : BaseViewModel
    {
        private readonly OMSDataService _dataService;
        private Basket? _selectedBasket;
        private Product? _selectedProduct;
        private int _quantity = 1;
        private ObservableCollection<Basket> _baskets;
        private ObservableCollection<Product> _products;

        public AddNewItemViewModel()
        {
            _dataService = new OMSDataService();
            _baskets = new ObservableCollection<Basket>();
            _products = new ObservableCollection<Product>();

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);

            _ = LoadDataAsync();
        }

        public ObservableCollection<Basket> Baskets
        {
            get => _baskets;
            set => SetProperty(ref _baskets, value);
        }

        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        public Basket? SelectedBasket
        {
            get => _selectedBasket;
            set
            {
                SetProperty(ref _selectedBasket, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                SetProperty(ref _selectedProduct, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                SetProperty(ref _quantity, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private async Task LoadDataAsync()
        {
            try
            {
                // Load baskets
                var baskets = await _dataService.GetBasketsWithShopperInfoAsync();
                System.Diagnostics.Debug.WriteLine($"AddNewItem: Loaded {baskets.Count} baskets");

                Baskets.Clear();
                foreach (var basket in baskets)
                {
                    Baskets.Add(basket);
                }

                // Load products
                var products = await _dataService.GetAllProductsAsync();
                System.Diagnostics.Debug.WriteLine($"AddNewItem: Loaded {products.Count} products");

                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }

                System.Diagnostics.Debug.WriteLine($"AddNewItem: Collections updated - Baskets: {Baskets.Count}, Products: {Products.Count}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AddNewItem: Error loading data: {ex.Message}");
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanSave(object? parameter)
        {
            return SelectedBasket != null && SelectedProduct != null && Quantity > 0;
        }

        private async void Save(object? parameter)
        {
            if (SelectedBasket == null || SelectedProduct == null)
                return;

            try
            {
                System.Diagnostics.Debug.WriteLine($"Save clicked - Basket: {SelectedBasket.IdBasket}, Product: {SelectedProduct.IdProduct}, Quantity: {Quantity}");

                // Validate quantity
                if (Quantity <= 0 || Quantity > 255)
                {
                    MessageBox.Show("Quantity must be between 1 and 255.", "Invalid Quantity", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var basketItem = new BasketItem
                {
                    IdBasket = SelectedBasket.IdBasket,
                    IdProduct = SelectedProduct.IdProduct,
                    Quantity = (byte)Quantity
                };

                System.Diagnostics.Debug.WriteLine($"Created BasketItem object - IdBasket: {basketItem.IdBasket}, IdProduct: {basketItem.IdProduct}, Quantity: {basketItem.Quantity}");

                bool success = await _dataService.SaveBasketItemAsync(basketItem);

                if (success)
                {
                    MessageBox.Show("Item added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Reset form
                    SelectedBasket = null;
                    SelectedProduct = null;
                    Quantity = 1;
                }
                else
                {
                    MessageBox.Show("Failed to add item. Check the Output window for detailed error information.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in Save method: {ex.Message}");
                MessageBox.Show($"Error saving item: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel(object? parameter)
        {
            // Reset form
            SelectedBasket = null;
            SelectedProduct = null;
            Quantity = 1;
        }
    }
}