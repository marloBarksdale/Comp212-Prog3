using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OMS.Data.Models
{
    [Table("Basket")]
    public class Basket
    {
        [Key]
        [Column("idBasket")]
        public int IdBasket { get; set; }

        [Column("idShopper")]
        public int IdShopper { get; set; }

        [Column("Quantity")]
        public byte Quantity { get; set; }

        [Column("SubTotal", TypeName = "decimal(7,2)")]
        public decimal SubTotal { get; set; }

        [Column("OrderDate")]
        [Required]
        public DateTime OrderDate { get; set; }

        // Navigation properties
        [ForeignKey("IdShopper")]
        public virtual Shopper Shopper { get; set; } = null!;

        public virtual ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();

        // Display property for ComboBox
        public string DisplayInfo => $"{Shopper?.Email ?? "Unknown"} {IdBasket}";
    }
}