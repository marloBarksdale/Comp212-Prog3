using Microsoft.EntityFrameworkCore;
using OMS.Data.Models;

namespace OMS.Data.Services
{
    public class OMSDataService
    {
        private readonly OMSDbContext _context;

        public OMSDataService()
        {
            _context = new OMSDbContext();
        }

        public OMSDataService(OMSDbContext context)
        {
            _context = context;
        }

        // Get data for comboBox that displays the shopper's email & IdBasket
        public async Task<List<Basket>> GetBasketsWithShopperInfoAsync()
        {
            return await _context.Baskets
                .Include(b => b.Shopper)
                .OrderBy(b => b.IdBasket)
                .ToListAsync();
        }

        // Get data for the DataGrid (BasketItems for selected basket)
        public async Task<List<BasketItem>> GetBasketItemsForBasketAsync(int basketId)
        {
            return await _context.BasketItems
                .Include(bi => bi.Product)
                .Include(bi => bi.Basket)
                .ThenInclude(b => b.Shopper)
                .Where(bi => bi.IdBasket == basketId)
                .OrderBy(bi => bi.IdBasketItem)
                .ToListAsync();
        }

        // Get data for the comboBox that displays the product's information
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .OrderBy(p => p.IdProduct)
                .ToListAsync();
        }

        // Save data to database (add new basket item)
        public async Task<bool> SaveBasketItemAsync(BasketItem basketItem)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Attempting to save BasketItem - Basket: {basketItem.IdBasket}, Product: {basketItem.IdProduct}, Quantity: {basketItem.Quantity}");

                // Get the current maximum IdBasketItem and add 1
                var maxId = await _context.BasketItems.AnyAsync()
                    ? await _context.BasketItems.MaxAsync(bi => bi.IdBasketItem)
                    : 0;

                basketItem.IdBasketItem = maxId + 1;
                System.Diagnostics.Debug.WriteLine($"Assigned new IdBasketItem: {basketItem.IdBasketItem}");

                _context.BasketItems.Add(basketItem);
                var rowsAffected = await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine($"SaveChanges completed. Rows affected: {rowsAffected}");
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error saving BasketItem: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nInner exception: {ex.InnerException.Message}";
                }

                System.Diagnostics.Debug.WriteLine(errorMessage);
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                // SHOW ERROR IN MESSAGEBOX TOO
                System.Windows.MessageBox.Show(errorMessage, "Database Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);

                return false;
            }
        }

        // Additional helper method to get a specific basket
        public async Task<Basket?> GetBasketByIdAsync(int basketId)
        {
            return await _context.Baskets
                .Include(b => b.Shopper)
                .FirstOrDefaultAsync(b => b.IdBasket == basketId);
        }

        // Additional helper method to get a specific product
        public async Task<Product?> GetProductByIdAsync(short productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.IdProduct == productId);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}