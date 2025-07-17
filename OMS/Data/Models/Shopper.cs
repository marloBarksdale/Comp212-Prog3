using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OMS.Data.Models
{
    [Table("Shopper")]
    public class Shopper
    {
        [Key]
        [Column("idShopper")]
        public int IdShopper { get; set; }

        [Column("Email")]
        [StringLength(25)]
        [Required]
        public string Email { get; set; } = string.Empty;

        [Column("FirstName")]
        [StringLength(15)]
        public string? FirstName { get; set; }

        [Column("LastName")]
        [StringLength(20)]
        public string? LastName { get; set; }

        [Column("Address")]
        [StringLength(40)]
        public string? Address { get; set; }

        [Column("City")]
        [StringLength(20)]
        public string? City { get; set; }

        [Column("StateProvince")]
        [StringLength(20)]
        public string? StateProvince { get; set; }

        [Column("Country")]
        [StringLength(20)]
        public string? Country { get; set; }

        [Column("ZipCode")]
        [StringLength(15)]
        public string? ZipCode { get; set; }

        // Navigation property
        public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();
    }
}