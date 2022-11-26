using ECommerce.Application.Core.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Application.Core.Entities
{
    public class Product: BaseEntity
    {
        [Required]
        [MaxLength(50)]
        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [DisplayName("Product Type")]
        public string Type { get; set; }

        [Required]
        [DisplayName("Quantity in stock")]
        public uint Quantity { get; set; }

        [Required]
        [DisplayName("Price")]
        public double Price { get; set; }

        [MaxLength(750)]
        [DisplayName("Product Description")]
        public string Description { get; set; }

        public string? Image { get; set; }

        [DisplayName("Days For Delivery")]
        public int DeliveryDays { get; set; }

        public string? UserId { get; set; }
    }
}
