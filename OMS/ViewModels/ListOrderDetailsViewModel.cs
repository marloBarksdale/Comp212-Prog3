using OMS.Data.Models;
using OMS.Data.Services;
using System.Collections.ObjectModel;

namespace OMS.ViewModels
{
    public class ListOrderDetailsViewModel : BaseViewModel
    {
        private readonly OMSDataService _dataService;
        private Basket? _selectedBasket;
        private ObservableCollection<Basket> _baskets;
        private ObservableCollection<BasketItem> _basketItems;

        public ListOrderDetailsViewModel()
        {
            _dataService = new OMSDataService();
            _baskets = new ObservableCollection<Basket>();
            _basketItems = new ObservableCollection<BasketItem>();

            _ = LoadBasketsAsync();
        }

        public ObservableCollection<Basket> Baskets
        {
            get => _baskets;
            set => SetProperty(ref _baskets, value);
        }

        public ObservableCollection<BasketItem> BasketItems
        {
            get => _basketItems;
            set => SetProperty(ref _basketItems, value);
        }

        public Basket? SelectedBasket
        {
            get => _selectedBasket;
            set
            {
                if (SetProperty(ref _selectedBasket, value))
                {
                    if (value != null)
                    {
                        _ = LoadBasketItemsAsync(value.IdBasket);
                    }
                    else
                    {
                        BasketItems.Clear();
                    }
                }
            }
        }

        private async Task LoadBasketsAsync()
        {
            try
            {
                var baskets = await _dataService.GetBasketsWithShopperInfoAsync();
                System.Diagnostics.Debug.WriteLine($"Loaded {baskets.Count} baskets from database");

                Baskets.Clear();
                foreach (var basket in baskets)
                {
                    System.Diagnostics.Debug.WriteLine($"Adding basket: {basket.IdBasket} - Email: {basket.Shopper?.Email}");
                    Baskets.Add(basket);
                }

                System.Diagnostics.Debug.WriteLine($"Baskets collection now has {Baskets.Count} items");
            }
            catch (Exception ex)
            {
                // Handle error - in a real application, you might want to show a message to the user
                System.Diagnostics.Debug.WriteLine($"Error loading baskets: {ex.Message}");
            }
        }

        private async Task LoadBasketItemsAsync(int basketId)
        {
            try
            {
                var basketItems = await _dataService.GetBasketItemsForBasketAsync(basketId);
                BasketItems.Clear();
                foreach (var item in basketItems)
                {
                    BasketItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                // Handle error
                System.Diagnostics.Debug.WriteLine($"Error loading basket items: {ex.Message}");
            }
        }

        public async Task RefreshDataAsync()
        {
            await LoadBasketsAsync();
            if (SelectedBasket != null)
            {
                await LoadBasketItemsAsync(SelectedBasket.IdBasket);
            }
        }
    }
}