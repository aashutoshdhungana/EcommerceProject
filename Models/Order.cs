using ECommerce.Application.Core.Interfaces;
using ECommerceServer.Application.Core.DTOs.Enumerations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Application.Core.Entities

{
    public class Order: BaseEntity
    {
        [Required]
        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        [Required]
        public uint Quantity { get; set; }
        [DisplayName("Total")]
        public double TotalPrice { get; set; }
        public string? Status { get; set; }
        public string? UserId { get; set; }
    }
}
