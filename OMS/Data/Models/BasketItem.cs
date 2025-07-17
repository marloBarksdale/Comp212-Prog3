using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OMS.Data.Models
{
    [Table("BasketItem")]
    public class BasketItem
    {
        [Key]
        [Column("idBasketItem")]
        public int IdBasketItem { get; set; }

        [Column("idProduct")]
        public short IdProduct { get; set; }

        [Column("Quantity")]
        public byte Quantity { get; set; }

        [Column("idBasket")]
        public int IdBasket { get; set; }

        // Navigation properties
        [ForeignKey("IdProduct")]
        public virtual Product Product { get; set; } = null!;

        [ForeignKey("IdBasket")]
        public virtual Basket Basket { get; set; } = null!;

        // Calculated properties for display
        [NotMapped]
        public string ProductName => Product?.ProductName ?? string.Empty;

        [NotMapped]
        public decimal UnitPrice => Product?.Price ?? 0;

        [NotMapped]
        public decimal LineTotal => UnitPrice * Quantity;
    }
}