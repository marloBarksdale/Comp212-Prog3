using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OMS.Data.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        [Column("idProduct")]
        public short IdProduct { get; set; }

        [Column("ProductName")]
        [StringLength(25)]
        public string? ProductName { get; set; }

        [Column("Description")]
        [StringLength(100)]
        public string? Description { get; set; }

        [Column("Price", TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }

        // Navigation property
        public virtual ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();

        // Display property for ComboBox
        public string DisplayInfo => $"{IdProduct} {ProductName}";
    }
}